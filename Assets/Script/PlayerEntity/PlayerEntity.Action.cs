using System.Collections;
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
        speed.x += WallJumpXBoost;
        speed.y = JumpSpeed;
        varJumpSpeed = speed.y;
        forceMoveX = dir;
        forceMoveXTimer = Times.WallJumpForceTime;
    }

    #endregion

}