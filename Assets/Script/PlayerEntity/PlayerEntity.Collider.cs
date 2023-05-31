using Microsoft.SqlServer.Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerEntity;

public partial class PlayerEntity: MonoBehaviour
{

    public Collider2D CheckCollider(BoxCollider2D col, Vector2 dir, float dist = 0, string mask = "Solid") //单层检测使用
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask(mask));

        Collider2D[] temp=new Collider2D[1];
        col.offset += dir * (dist + ColSet.OffsetDistance);
        col.OverlapCollider(contactFilter, temp);
        col.offset -= dir * (dist + ColSet.OffsetDistance);
        return temp[0];
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
        bodyBox.Cast(Vector2.down,contactFilter, hit,ColSet.OffsetDistance);
        return hit.Length > 0 && hit[0].normal == Vector2.up;
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

    private void ProcessCollisionDataX()
    {
        while (collisionXQue.Count > 0)
        {
            CollisionData data = collisionXQue.Peek();

            collisionXQue.Dequeue();
        }
    }

    private void ProcessCollisionDataY()
    {
        while(collisionYQue.Count > 0) 
        {
            CollisionData data=collisionYQue.Peek();
            if (data.speed.y < 0)
            {
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
                    float squish = Mathf.Min(speed.y / SpdSet.FastMaxFall, 1);
                    scale.x = Mathf.Lerp(1, 1.6f, squish);
                    scale.y = Mathf.Lerp(1, .4f, squish);
                }

            }
            collisionYQue.Dequeue();
        }
    }

    private class CollisionData
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
