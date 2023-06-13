using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;
using static PlayerEntity;

public partial class PlayerEntity: MonoBehaviour
{
    #region Jump
    public void JumpReset()
    {
        varJumpTimer = TimeSet.SuperWallJumpVarTime;
        wallSlideTimer = TimeSet.WallSlideTime;
        jumpGraceTimer = 0;
        dashAttackTimer = 0;
        scale = new Vector2( 0.6f, 1.4f);
    }

    public void Jump()
    {
        JumpReset();
        speed.x += SpdSet.JumpXBoost * input_move.x;
        speed.y = SpdSet.JumpSpeed;
        varJumpSpeed = speed.y;
        PlayJumpDust();
    }

    public void SuperJump()
    {
        JumpReset();
        speed.x = SpdSet.SuperJumpX * (int)facing;
        speed.y = SpdSet.JumpSpeed;

        launchTimer = TimeSet.launchTime;

        if (Ducking)
        {
            Ducking = false;
            speed.x *= SpdSet.DuckSuperJumpXMult;
            speed.y *= SpdSet.DuckSuperJumpYMult;
        }
        PlayJumpDust();
        varJumpSpeed = speed.y;
    }

    public void WallJump(int dir)
    {
        Ducking = false;
        JumpReset();
        speed.x = SpdSet.WallJumpXBoost * dir;
        speed.y = SpdSet.JumpSpeed;
        if (input_move.x != 0)
        {
            forceMoveX = dir;
            forceMoveXTimer = TimeSet.WallJumpForceTime;
        }
        
        varJumpSpeed = speed.y;
    }
    public void SuperWallJump(int dir)
    {
        Ducking = false;
        JumpReset();
        speed.x = SpdSet.SuperWallJumpX * dir;
        speed.y = SpdSet.SuperWallJumpSpeed;

        launchTimer = TimeSet.launchTime;

        if (input_move.x != 0)
        {
            forceMoveX = dir;
            forceMoveXTimer = TimeSet.WallJumpForceTime;
        }
        varJumpSpeed = speed.y;
    }


    public void ClimbJump()
    {
        if (!onGround)
        {
            Stamina -= ClimbSet.ClimbJumpCost;
        }
        Jump();
    }

    #endregion

    #region Duck

    public bool Ducking
    { 
        get { return bodyBox == duckBox; }
        set 
        {
            if(value) 
            {
                duckBox.gameObject.SetActive(true);
                bodyBox = duckBox;
                normalBox.gameObject.SetActive(false);
            }
            else 
            {
                normalBox.gameObject.SetActive(true);
                bodyBox = normalBox;
                duckBox.gameObject.SetActive(false);
            }
        }
    }

    public bool CanUnDuck
    {
        get
        {
            if (!Ducking)
                return false;
            bool was = normalBox.gameObject.activeSelf;
            normalBox.gameObject.SetActive(true);
            bool res = BoxFreeAt(Position, bodyBox);
            normalBox.gameObject.SetActive(was);
            return res;
        }
    }

    #endregion

    [HideInInspector] public float hopWaitX;
    [HideInInspector] public float hopWaitXSpeed;
    [HideInInspector] public Collider2D climbHopSolid;
    #region Climb
    public void ClimbHop()
    {
        climbHopSolid = CheckCollider(Position,bodyBox, Vector2.right * (int)facing);
        if(climbHopSolid != null )
        {
            hopWaitX = (int)facing;
            hopWaitXSpeed= (int)facing * SpdSet.ClimbHopX;
        }
        else
        {
            hopWaitX = 0;
            speed.x = (int)facing * SpdSet.ClimbHopX; 
        }
        speed.y = Mathf.Min(speed.y, SpdSet.ClimbHopY);
        forceMoveXTimer = TimeSet.ClimbHopForceTime;
        forceMoveX = 0;
    }
    #endregion

    private void Dead()
    {
        Destroy(gameObject);
        GameManager.Instance.Respawn();
    }
}