using System.Collections;
using UnityEngine;


public static class GamePhysics
{

    public static bool CheckCollider(BoxCollider2D col, string mask = "Solid") //单层检测使用
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask(mask));
        return col.OverlapCollider(contactFilter, new Collider2D[1]) > 0;
    }
    public static bool CheckCollider(BoxCollider2D col, string []masks)
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask(masks));
        return col.OverlapCollider(contactFilter, new Collider2D[1]) > 0;
    }

}
