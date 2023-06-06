using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;
using static PlayerEntity;

public partial class PlayerEntity: MonoBehaviour
{
    #region Jump

    public void Jump()
    {
        varJumpTimer = TimeSet.VarJumpTime;
        wallSlideTimer = TimeSet.WallSlideTime;
        jumpGraceTimer = 0;
        dashAttackTimer = 0;
        speed.x += JumpXBoost * input_move.x;
        speed.y = JumpSpeed;
        varJumpSpeed = speed.y;
    }

    public void SuperJump()
    {
        varJumpTimer = TimeSet.VarJumpTime;
        wallSlideTimer = TimeSet.WallSlideTime;
        jumpGraceTimer = 0;
        dashAttackTimer = 0;
        speed.x = SuperJumpX * (int)facing;
        speed.y = JumpSpeed;

        launchTimer = TimeSet.launchTime;

        if (Ducking)
        {
            Ducking = false;
            speed.x *= DuckSuperJumpXMult;
            speed.y *= DuckSuperJumpYMult;
            //特效1
        }
        else
        {
            ;
            //特效2
        }

        varJumpSpeed = speed.y;
    }

    public void WallJump(int dir)
    {
        Ducking = false;
        varJumpTimer = TimeSet.VarJumpTime;
        wallSlideTimer = TimeSet.WallSlideTime;
        jumpGraceTimer = 0;
        dashAttackTimer = 0;
        speed.x = WallJumpXBoost * dir;
        speed.y = JumpSpeed;
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
        varJumpTimer = TimeSet.SuperWallJumpVarTime;
        wallSlideTimer = TimeSet.WallSlideTime;
        jumpGraceTimer = 0;
        dashAttackTimer = 0;
        speed.x = SuperWallJumpX * dir;
        speed.y = SuperWallJumpSpeed;

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
        climbHopSolid = CheckCollider(bodyBox, Vector2.right * (int)facing);
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
}