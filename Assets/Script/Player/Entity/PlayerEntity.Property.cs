using System.Collections;
using UnityEngine;
using static PlayerEntity;

public partial class PlayerEntity: MonoBehaviour
{
    //我为什么要写这个属性。。。
    public bool onGround
    {
        get { return _onGround; }
        set{_onGround = value;}
    }

    public Vector2 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public bool IsTired
    {
        get
        {
            return Stamina < ClimbSet.ClimbTiredThreshold;
        }
    }

    public bool CanDash
    {
        get 
        {
            return input.GamePlay.Dash.WasPressedThisFrame() && dashCooldownTimer <= 0 && dashes > 0; 
         }
    }
    public bool DashAttacking
    {
        get
        {
            return dashAttackTimer > 0;
        }
    }

}
