using System.Collections;
using UnityEngine;
using static Consts;

public class NormalState : BaseState
{
    public override State state => (int)State.Normal;

    public override bool haveCoroutine => false;

    public NormalState(PlayerEntity p):base(p)
    {

    }

    public override IEnumerator Coroutine()
    {
        yield return null;
    }

    public override void OnEnd()
    {
        
    }

    public override void OnEnter()
    {
        
    }

    public override State Update()
    {
        {
            if (pe.input.GamePlay.Climb.WasPressedThisFrame())
                pe.climbButtonTimer = pe.climbButtonTime;

            if (pe.climbButtonTimer > 0 && GamePhysics.CheckCollider(pe.handBox))
            {
                return State.Climb;
            }

            if(pe.input.GamePlay.Dash.WasPerformedThisFrame())
            {
                return State.Dash;
            }

            if (pe.Ducking)
            {
                if(pe.onGround&&pe.input_move.y >= 0)
                {
                    if (pe.CanUnDuck)
                    {
                        pe.Ducking= false;
                        //scale
                    }
                }
            }
            else if (pe.onGround && pe.input_move.y < 0 && pe.speed.y <= 0)
            {
                //scale
                pe.Ducking = true;
            }

        }

        //x轴速度计算
        if(pe.Ducking&&pe.onGround)
        {
            pe.speed.x = Mathf.MoveTowards(pe.speed.x, 0, SpdSet.DuckFriction*Time.deltaTime);
        }
        else
        {
            float mult = pe.onGround ? 1 : PhySet.AirMult;
            float max = pe.MaxRun;
            float moveX = pe.input_move.x;
            
            float acc = pe.RunAccel;
            //可能会速度过快
            if (Mathf.Abs(pe.speed.x) > max && Mathf.Sign(pe.speed.x) == moveX)
                acc= pe.RunReduce;

            pe.speed.x = Mathf.MoveTowards(pe.speed.x, max * moveX, acc * mult * Time.deltaTime);
        }


        //y轴速度计算

        if (pe.varJumpTimer > 0)
        {
            //有可能速度比跳跃快，所以是取Max
            if (pe.input.GamePlay.Jump.IsPressed())
                pe.speed.y = Mathf.Max(pe.speed.y, pe.varJumpSpeed);
            else
                pe.varJumpTimer = 0;
        }

        if (pe.input.GamePlay.Jump.WasPressedThisFrame())
        {
            if (pe.jumpGraceTimer > 0)
            {
                pe.Jump();
            }
/*            else if (CanUnDuck)
            {

            }*/
        }

        return State.Normal;
    }
}
