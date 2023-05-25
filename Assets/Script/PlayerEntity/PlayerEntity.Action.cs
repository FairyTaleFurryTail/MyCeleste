using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static Consts;

public partial class PlayerEntity: MonoBehaviour
{
    #region Jump

    public void Jump()
    {
        varJumpTimer = Times.VarJumpTime;
        jumpGraceTimer = 0;
        speed.x += JumpXBoost * input_move.x;
        speed.y = JumpSpeed;
        varJumpSpeed = speed.y;
    }

    public void SuperJump()
    {
        varJumpTimer = Times.VarJumpTime;
        jumpGraceTimer = 0;
        speed.x = SuperJumpX * (int)facing;
        speed.y = JumpSpeed;

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
        varJumpTimer = Times.VarJumpTime;
        jumpGraceTimer = 0;
        speed.x = WallJumpXBoost * dir;
        speed.y = JumpSpeed;
        if (input_move.x != 0)
        {
            forceMoveX = dir;
            forceMoveXTimer = Times.WallJumpForceTime;
        }
        
        varJumpSpeed = speed.y;
        
        
    }
    public void SuperWallJump(int dir)
    {
        Ducking = false;
        varJumpTimer = Times.SuperWallJumpVarTime;
        jumpGraceTimer = 0;
        speed.x = SuperWallJumpX * dir;
        speed.y = SuperWallJumpSpeed;
        if (input_move.x != 0)
        {
            forceMoveX = dir;
            forceMoveXTimer = Times.WallJumpForceTime;
        }
        varJumpSpeed = speed.y;
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
            bool res = !CheckCollider(normalBox,Vector2.up);
            normalBox.gameObject.SetActive(was);
            return res;
        }
    }

    #endregion

    #region Climb
    public void ClimbHop()
    {
        speed.x = (int)facing *  SpdSet.ClimbHopX;
        speed.y = Mathf.Min(speed.y, SpdSet.ClimbHopY);
        forceMoveXTimer = Times.ClimbHopForceTime;
        forceMoveX = 0;
    }
    #endregion
}