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
        if (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) > 2)
            dir = dir.normalized;
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask(mask));

        Collider2D[] temp=new Collider2D[1];
        col.offset += dir * (dist + ColSet.OffsetDistance);
        col.OverlapCollider(contactFilter, temp);
        col.offset -= dir * (dist + ColSet.OffsetDistance);
        return temp[0];
    }

    public Collider2D CheckCollider(Vector2 pos,BoxCollider2D col, Vector2 dir, float dist = 0, string mask = "Solid") //单层检测使用
    {
        Vector3 savePos = col.transform.position;
        col.transform.position = (Vector3)pos + col.transform.localPosition;
        Collider2D res = CheckCollider(col, dir, dist, mask);
        col.transform.position = savePos;
        //Debug.Log(pos); Debug.Log(col.transform.localPosition); Debug.Log(col.transform.position);
        return res;
    }

    public bool WallJumpCheck(int dir)
    {
        return CheckCollider(bodyBox, Vector2.right * dir, ColSet.WallJumpCheckDist);
    }

    private bool CheckGround(Vector2 offset, string mask = "Solid")
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask(mask));
        RaycastHit2D[] hit = new RaycastHit2D[1];
        //使用的是向下cast碰撞体实现（也可以通过碰撞点的方向判断）
        bodyBox.transform.position += (Vector3)offset;
        bodyBox.Cast(Vector2.down,contactFilter, hit,ColSet.OffsetDistance);
        bodyBox.transform.position -= (Vector3)offset;
        return hit.Length > 0 && hit[0].normal == Vector2.up;
    }

    private bool DuckFreeAt(Vector2 pos)
    {
        bool saveActive = duckBox.gameObject.activeSelf;
        Vector3 savePos = duckBox.transform.position;
        Vector2 saveSize = duckBox.size;
        duckBox.gameObject.SetActive(true);
        duckBox.transform.position = (Vector3)pos + duckBox.transform.localPosition;
        duckBox.size *= 0.95f;
        bool res = !CheckCollider(duckBox, Vector2.zero);
        duckBox.transform.position = savePos;
        duckBox.size = saveSize;
        duckBox.gameObject.SetActive(saveActive);
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
            if (onGround && DuckFreeAt(Position + Vector2.right * data.speed * Time.deltaTime))
            {
                Ducking = true;
                return;
            }
            else if (data.speed.y == 0 && data.speed.x != 0)
            {
                for (int i = 1; i <= ColSet.DashCornerCorrection; i++)
                    for (int d = 1; d >= -1; d -= 2)
                        if (!CheckCollider(Position + Vector2.up * d * i * ColSet.CorrectStep, bodyBox, data.speed.normalized))
                        {
                            Position += Vector2.up * d * i * ColSet.CorrectStep;
                            speed = data.speed;
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
            if(stateMachine.state == (int)State.Dash&&!dashStartedOnGround)
            {
                for (int i = 1; i <= ColSet.DashCornerCorrection; i++)
                    for (int d = 1; d >= -1; d -= 2)
                        if (!CheckGround(new Vector2(0, i * d * ColSet.CorrectStep)))
                        {
                            Position += new Vector2(0, i * d * ColSet.CorrectStep);
                            speed = data.speed;
                            return;
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

        }
    }

    public class CollisionData
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
