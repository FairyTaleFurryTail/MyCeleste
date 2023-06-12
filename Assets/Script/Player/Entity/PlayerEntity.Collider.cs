using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerEntity;
using static Calc;
using System.Drawing;

public partial class PlayerEntity: MonoBehaviour
{
    //这里一开始是直接移动碰撞体来检测，发现不行
    public Collider2D CheckCollider(Vector2 pos,BoxCollider2D col, Vector2 dir, float dist = 0,float scale=1, string mask = "Solid")
    {
        dir = dir.normalized;//以防万一
        //pos是玩家位置，所以要加上碰撞体本地位置
        pos += dir * (dist + ColSet.OffsetDistance) + (Vector2)col.transform.localPosition;
        Vector2 size = col.size;
        if(scale<1)
        {
            float moveMulti = (1 - scale) / 2;
            pos += new Vector2(size.x * moveMulti * dir.x, size.y * moveMulti * dir.y);
            size *= scale;
        }
        Collider2D collider = Physics2D.OverlapBox(pos, size, 0, LayerMask.GetMask(mask));
        //Debug.Log(dir + " " + Physics2D.OverlapBoxAll(pos, col.size, 0, LayerMask.GetMask(mask)).Length);
        return collider;
    }

    public bool WallJumpCheck(int dir)
    {
        return CheckCollider(Position, bodyBox, Vector2.right * dir, ColSet.WallJumpCheckDist, scale: 0.8f) ;
    }

    /// <summary>注重方向，使用了Cast的碰撞检测，注意dir只能有一维</summary>
    public bool CastCheckCollider(Vector2 offset,Vector2 dir,float additionDis=0,float sizeMulti=1, string mask = "Solid")
    {
        Vector2 pos= (Vector2)bodyBox.transform.position + offset;
        foreach(var hit in Physics2D.BoxCastAll(pos, bodyBox.size* sizeMulti, 0, dir, ColSet.OffsetDistance+additionDis, LayerMask.GetMask(mask)))
            if (hit.normal == dir * -1)
                return true;
        return false;
    }

    private bool BoxFreeAt(Vector2 pos,BoxCollider2D col)
    {
        bool saveActive = col.gameObject.activeSelf;
        Vector2 saveSize = col.size;
        col.gameObject.SetActive(true);
        col.size *= 0.95f;
        bool res = !CheckCollider(pos,col, Vector2.zero);
        col.gameObject.SetActive(saveActive);
        col.size = saveSize;
        return res;
    }

    private Queue<CollisionData> collisionXQue = new Queue<CollisionData>();
    private Queue<CollisionData> collisionYQue = new Queue<CollisionData>();
    private void OnCollisionEnter2D(Collision2D collider)
    {
        //normal是指我相对碰撞点的位置
        Vector2 normal = collider.GetContanctDirection();
        //存在队列里，这一帧最后统一执行，防止混乱
        if (normal.x != 0)
            collisionXQue.Enqueue(new CollisionData(collider, normal ,speed));
        if (normal.y!=0)
            collisionYQue.Enqueue(new CollisionData(collider, normal , speed));

    }
    private void OnCollisionDatas()
    {
        CollisionData data;
        while (collisionXQue.Count > 0)
        {
            data = collisionXQue.Peek();
            ProcessCollisionDataX(data);
            collisionXQue.Dequeue();
        }
        while (collisionYQue.Count > 0)
        {
            data = collisionYQue.Peek();
            ProcessCollisionDataY(data);
            collisionYQue.Dequeue();
        }
    }

    private void ProcessCollisionDataX(CollisionData data)
    {
        if (stateMachine.state == (int)State.Dash)
        {
            if (onGround && BoxFreeAt(Position + Vector2.right * data.speed * Time.deltaTime,duckBox))
            {
                Ducking = true;
                return;
            }
            else if (data.speed.y == 0 && data.speed.x != 0)
            {
                for (int i = 1; i <= ColSet.DashCornerCorrection; i++)
                    for (int d = 1; d >= -1; d -= 2)
                        if (!CheckCollider(Position + Vector2.up * d * i * ColSet.CorrectXStep, bodyBox ,Vector2.zero))
                        {
                            Position += Vector2.up * d * i * ColSet.CorrectXStep;
                            speed = data.speed;
                            //Debug.Log("XSuccess");
                            return;
                        }
            }
        }
    }

    private void ProcessCollisionDataY(CollisionData data)
    {
        if (data.speed.y < 0)//下落时
        {
            //一般玩家向下冲是不想落地的（否则直接落地就好了），所以要绕开地面。原地下冲不算
            //不能往移动的反向偏移
            if (stateMachine.state == (int)State.Dash && !dashStartedOnGround)
            {
                for (int i = 1; i <= ColSet.DashCornerCorrection; i++)
                    for (int d = 1; d >= -1; d -= 2)
                    {
                        if (d * data.speed.x < 0) continue;
                        Vector2 offset = new Vector2(i * d * ColSet.CorrectYStep, 0);
                        if (!CheckCollider(Position+offset,bodyBox,Vector2.down,dist:0.5f))
                        {
                            Position += offset;
                            speed = data.speed;
                            return;
                        }
                    }
            }
            //Dash Slide  改变滑向，并且Ducking
            if (dashDir.x != 0 && dashDir.y < 0)
            {
                Ducking = true;
                dashDir.y = 0;
                speed.y = 0;
                speed.x = data.speed.x * SpdSet.DodgeSlideSpeedMult;//模拟y轴速度转换为x轴速度，凌波微步会用到这个加成
            }

            if (stateMachine.state != (int)State.Climb)
            {
                float squish = Mathf.Min(data.speed.y / SpdSet.FastMaxFall, 1);
                scale.x = Mathf.Lerp(1, 1.6f, squish);
                scale.y = Mathf.Lerp(1, .4f, squish);
                GameManager.sem.PlayOnce("LandDust", transform.position + PosSet.foot);
            }
        }
        else if(data.speed.y > 0)
        {
            //不需要Dash，因为向上就是不想撞墙
            {
                for (int i = 1; i <= ColSet.DashCornerCorrection; i++)
                    for (int d = 1; d >= -1; d -= 2)
                    {
                        if (data.speed.x * d < 0) continue;
                        Vector2 newPos = Position + new Vector2(i * d * ColSet.CorrectYStep,0);
                        if (!CheckCollider(newPos, bodyBox, data.speed.normalized))
                        {
                            Position = newPos;
                            speed = data.speed;
                            //Debug.Log("YSuccess");
                            return;
                        }
                    }
            }
        }
    }

    public struct CollisionData
    {
        public Collision2D col;
        public Vector2 dir;
        public Vector2 speed;

        public CollisionData(Collision2D col, Vector2 dir, Vector2 speed)
        {
            this.col = col;
            this.dir = dir;
            this.speed = speed;
        }
    }
}
