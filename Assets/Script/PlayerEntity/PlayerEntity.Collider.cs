using System.Collections;
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

    private void OnCollisionX(Collider2D col,int dire)
    {

    }

    private void OnCollisionY(Collider2D col,int dire)
    {

    }

}
