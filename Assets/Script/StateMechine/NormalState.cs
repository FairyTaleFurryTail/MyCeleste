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

            if (pe.climbButtonTimer > 0 && pe.CheckCollider(pe.bodyBox,Vector2.right*(int)pe.facing))
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
        float mf = SpdSet.MaxFall;
        float fmf = SpdSet.FastMaxFall;

        if (pe.input_move.y<0&& pe.speed.y <= mf)
        {
            pe.maxFall = Mathf.MoveTowards(pe.maxFall, fmf, SpdSet.FastMaxAccel * Time.deltaTime);

            //Scale变化：加速到fmf的一半的时候scale开始发生变化
            float half = (mf + fmf) / 2;
            if(pe.speed.y<=half)
            {
                float spriteLerp = Mathf.Min(1, (pe.speed.y - half) / (fmf - half));
                pe.scale.x = Mathf.Lerp(1f, 0.5f, spriteLerp);
                pe.scale.y = Mathf.Lerp(1f, 1.5f, spriteLerp);
            }
        }
        else
            pe.maxFall = Mathf.MoveTowards(pe.maxFall, mf, SpdSet.FastMaxAccel * Time.deltaTime);

        //重力 下落
        if (!pe.onGround)
        {
            float falls = pe.maxFall;

            //计算滑墙
            if(pe.input_move.x*(int)pe.facing>1)
            {
                if(pe.speed.y<=0&&pe.wallSlideTimer>0 && pe.CheckCollider(pe.bodyBox,Vector2.right*(int)pe.facing))
                {
                    pe.Ducking = false;
                    pe.wallSlideDir = (int)pe.facing;
                }
                if(pe.wallSlideDir!=0)
                {
                    falls = Mathf.Lerp(SpdSet.MaxFall, SpdSet.WallSlideStartMax, pe.wallSlideTimer / Times.WallSlideTime);
                    if (pe.wallSlideTimer / Times.WallSlideTime > .65f)
                    {
                        //特效
                    }
                }
            }

            pe.speed.y = Mathf.MoveTowards(pe.speed.y, falls, pe.Gravity * Time.deltaTime);
        }



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
            else if (true)
            {
                if (pe.WallJumpCheck((int)pe.facing))
                {
                    if (pe.dashAttackTimer > 0 && pe.dashDir.y > 0 && pe.dashDir.x == 0)
                    {
                        pe.SuperWallJump((int)pe.facing * -1);
                    }
                    else
                    {
                        pe.WallJump((int)pe.facing * -1);
                    }
                }
                else if (pe.WallJumpCheck((int)pe.facing * -1))
                {
                    if (pe.dashAttackTimer > 0 && pe.dashDir.y > 0 && pe.dashDir.x == 0)
                    {
                        pe.SuperWallJump((int)pe.facing);
                    }
                    else
                    {
                        pe.WallJump((int)pe.facing);
                    }

                }
            }
        }

        return State.Normal;
    }
}
