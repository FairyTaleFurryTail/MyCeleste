using System;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine;
using static PlayerEntity;

public class DashState : BaseState
{
    public DashState(PlayerEntity p):base(p) 
    { }
    public override State state => State.Dash;

    public override bool haveCoroutine => true;

    
    public override void OnEnd()
    {
        //pe.rd.gravityScale = PhySet.Gravity;
    }

    private State returnState;
    
    public override void OnEnter()
    {
        returnState = state;
        pe.dashDir = pe.input_move;
        pe.dashes--;
        pe.lastDashIndex = 1-pe.dashes;
        pe.lastDashFacing = (float)pe.facing;
        pe.dashStartedOnGround = pe.onGround;
        pe.dashCooldownTimer = TimeSet.DashCooldown;
        if (pe.dashDir == Vector2.zero)
            pe.dashDir = new Vector2((float)pe.facing, 0);
        pe.dashAttackTimer = TimeSet.DashAttackTime;
        pe.dashEffectTimer = TimeSet.DashEffectTime;

        spdDir = pe.dashDir.normalized;
        GameManager.sem.dashEffect.ResetInterval();
        GameManager.sem.pauseTimer = 0.05f;
        GameManager.sem.CameraShake(5, 3, 0.1f,spdDir.x*12,spdDir.y*12);
    }

    public override State Update()
    {
        //爬墙
        if (pe.input.GamePlay.Climb.IsPressed() & !pe.IsTired && !pe.Ducking)
        {
            //为了爬跳不会卡住，要往下落才能爬（我本来以为要个禁止爬墙计时器的，结果居然是这样- -）
            if (pe.speed.y <= 0 && pe.speed.x * (int)pe.facing >= 0)
            {
                //if(pe.CheckCollider(pe.bodyBox, Vector2.right * (int)pe.facing))
                if (pe.CastCheckCollider(Vector2.zero, Vector2.right * (int)pe.facing))
                {
                    pe.Ducking = false;
                    return State.Climb;
                }
            }
        }

        if (pe.dashDir.y == 0)//横冲
        {
            if (pe.input.GamePlay.Jump.IsPressed()&& pe.jumpGraceTimer > 0)
            {
                pe.SuperJump();
                return State.Normal;
            }
        }
        else
        {
            if (pe.input.GamePlay.Jump.WasPressedThisFrame())
            {
                int wallJumpDir = pe.WallJumpCheck(1) ? -1 : pe.WallJumpCheck(-1) ? 1 : 0;

                if (wallJumpDir != 0)
                {
                    if (pe.dashDir.y == 1 && pe.dashDir.x == 0)//上冲
                        pe.SuperWallJump(wallJumpDir);
                    else
                        pe.WallJump(wallJumpDir);
                    pe.PlayWallJumpDust(-wallJumpDir);
                    return State.Normal;
                }
            } 
        }

        return returnState;
    }
    Vector2 spdDir;
    public override IEnumerator Coroutine()
    {
        Vector2 newSpeed = spdDir * SpdSet.DashSpeed;
        if(spdDir.x==Mathf.Sign(pe.speed.x)&& Mathf.Abs(newSpeed.x)< Mathf.Abs(pe.speed.x))
            newSpeed.x=pe.speed.x;
        pe.speed=newSpeed;

        pe.rd.gravityScale = 0;
        yield return new WaitTime(TimeSet.DashTime);

        if(spdDir.y>=0)
        {
            pe.speed = spdDir * SpdSet.EndDashSpeed;
        }
        if(pe.speed.y>0)
        {
            pe.speed.y *= SpdSet.EndDashUpMult;
        }

        returnState = State.Normal;

    }
}
