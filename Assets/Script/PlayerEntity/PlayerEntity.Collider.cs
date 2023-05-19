using System;
using System.Collections;
using UnityEngine;


public partial class PlayerEntity : MonoBehaviour
{
    public bool CheckGroud()
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Solid"));
        return footBox.OverlapCollider(contactFilter, new Collider2D[1])>0?true:false;
    }

}