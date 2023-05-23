using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Consts;
using Times = Consts.Times;

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

    public void WallJump(int dir)
    {
        varJumpTimer = Times.VarJumpTime;
        jumpGraceTimer = 0;
        speed.x += WallJumpXBoost * input_move.x;
        speed.y = JumpSpeed;
        varJumpSpeed = speed.y;
        forceMoveX = dir;
        forceMoveXTimer = Times.WallJumpForceTime;
        
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
            bool res = !GamePhysics.CheckCollider(normalBox);
            normalBox.gameObject.SetActive(was);
            return res;
        }
    }

    #endregion
}