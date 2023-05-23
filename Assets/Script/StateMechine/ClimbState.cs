using System.Collections;
using UnityEngine;
using static Consts;
using Times = Consts.Times;

public class ClimbState : BaseState
{
    private float climbNoMoveTimer;

    public ClimbState(PlayerEntity p) : base(p)
    {
    }

    public override State state => State.Climb;

    public override bool haveCoroutine => false;

    public override IEnumerator Coroutine()
    {
        yield return null;
    }

    public override void OnEnd()
    {
        //pe.rd.isKinematic = false;
        pe.rd.gravityScale = PhySet.Gravity;
        //pe.jumpGraceTimer = Times.JumpGraceTime;
    }

    public override void OnEnter()
    {
        pe.speed.x = 0;
        pe.speed.y = Times.ClimbGrabYMult;
        pe.climbButtonTimer = 0;
        climbNoMoveTimer = Times.ClimbNoMoveTime;
        pe.rd.gravityScale = 0;
        //pe.rd.isKinematic = true;
    }

    public override State Update()
    {
        if (climbNoMoveTimer > 0) climbNoMoveTimer -= UnityEngine.Time.deltaTime;

        //处理跳跃
        if((pe.input.GamePlay.Jump.WasPressedThisFrame()))
        {
            if(pe.input_move.x == (int)pe.facing*-1)//Wall Jump
            {
                pe.WallJump((int)pe.input_move.x);
            }
            else//ClimbJump
            {

            }
            return State.Normal;
        }


        if (!pe.input.GamePlay.Climb.IsPressed())
            return State.Normal;

        pe.speed.y = Mathf.MoveTowards(pe.speed.y,0, pe.ClimbGrapReduce * UnityEngine.Time.deltaTime);

        if (climbNoMoveTimer<=0)
        {
            if(pe.input_move.y==1)
            {
                pe.speed.y = pe.ClimbUpSpeed;
            }
            else if(pe.input_move.y==-1)
            {
                if(!pe.onGround)
                {
                    pe.speed.y = pe.ClimbDownSpeed;
                }
                else
                {
                    pe.speed.y = 0;
                }
            }
        }
        


        return State.Climb; 
    }
}
