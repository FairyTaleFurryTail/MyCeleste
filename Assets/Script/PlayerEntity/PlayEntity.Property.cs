using System.Collections;
using UnityEngine;
using static PlayerEntity;

public partial class PlayerEntity: MonoBehaviour
{
    public bool onGround
    {
        get { return _onGround; }
        set
        {
            /*if (speed.y<0)
            if(value==true&& _onGround==false)
            {
                float squish = Mathf.Min(speed.y / SpdSet.FastMaxFall, 1);
                scale.x = Mathf.Lerp(1, 1.6f, squish);
                scale.y = Mathf.Lerp(1, .4f, squish);
            }*/
            _onGround = value;
        }
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
