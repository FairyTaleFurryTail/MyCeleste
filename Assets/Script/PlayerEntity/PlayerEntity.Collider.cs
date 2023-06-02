using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static PlayerEntity;

public partial class PlayerEntity: MonoBehaviour
{

    public Collider2D CheckCollider(BoxCollider2D col, Vector2 dir, float dist = 0, string mask = "Solid") //单层检测使用
    {
        return CheckCollider(Position, col, dir, dist, mask);
    }
    //这里一开始是直接移动碰撞体来检测，发现不行
    public Collider2D CheckCollider(Vector2 pos,BoxCollider2D col, Vector2 dir, float dist = 0, string mask = "Solid") //单层检测使用
    {
        dir = dir.normalized;
        //pos是玩家位置，所以要加上碰撞体本地位置
        pos += dir * (dist + ColSet.OffsetDistance) + (Vector2)col.transform.localPosition;
        return Physics2D.OverlapBox(pos, col.size, 0, LayerMask.GetMask(mask));
    }

    public bool WallJumpCheck(int dir)
    {
        return CheckCollider(bodyBox, Vector2.right * dir, ColSet.WallJumpCheckDist);
    }

    private bool CheckGround(Vector2 offset, string mask = "Solid")
    {
        //使用的是向下cast碰撞体实现
        Vector2 pos= bodyBox.transform.position + (Vector3)offset;
        RaycastHit2D hit = Physics2D.BoxCast(pos, bodyBox.size, 0, Vector2.down, ColSet.OffsetDistance, LayerMask.GetMask(mask));
        return hit && hit.normal == Vector2.up;
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
        //Debug.Log(speed.y);
        ContactPoint2D[] contactPoint = new ContactPoint2D[1];//碰撞方向的单位向量
        collider.GetContacts(contactPoint);//获取碰撞点
        //normal是指我相对碰撞点的位置
        Vector2 normal = contactPoint[0].normal;
        
        //存在队列里，这一帧最后统一执行，防止混乱
        if (normal.x != 0)
            collisionXQue.Enqueue(new CollisionData(collider, normal ,speed));
        if (normal.y!=0)
            collisionYQue.Enqueue(new CollisionData(collider, normal , speed));

    }
    private void ProcessCollisionDatas()
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
                        if (!CheckCollider(Position + Vector2.up * d * i * ColSet.CorrectXStep, bodyBox, data.speed.normalized))
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
            if(stateMachine.state == (int)State.Dash&&!dashStartedOnGround)
            {
                for (int i = 1; i <= ColSet.DashCornerCorrection; i++)
                    for (int d = 1; d >= -1; d -= 2)
                    {
                        if (d * data.speed.x < 0) continue;
                        Vector2 offset = new Vector2(i * d * ColSet.CorrectYStep,0 );
                        if (!CheckGround(offset))
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
                speed.x = data.speed.x * SpdSet.DodgeSlideSpeedMult;
            }

            if (stateMachine.state != (int)State.Climb)
            {
                float squish = Mathf.Min(data.speed.y / SpdSet.FastMaxFall, 1);
                scale.x = Mathf.Lerp(1, 1.6f, squish);
                scale.y = Mathf.Lerp(1, .4f, squish);
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
