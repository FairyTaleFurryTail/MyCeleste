using System;
using System.Collections;
using UnityEngine;


public partial class PlayerEntity : MonoBehaviour
{
    public bool CheckCollider(BoxCollider2D col,string mask="Solid")
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask(mask));
        return col.OverlapCollider(contactFilter, new Collider2D[1])>0;
    }


}