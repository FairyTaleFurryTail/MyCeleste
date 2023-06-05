﻿using System;
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

        GameManager.sem.pauseTimer = 0.05f;
    }

    public override State Update()
    {
        if (pe.input.GamePlay.Climb.IsPressed() && pe.CastCheckCollider(Vector2.zero, Vector2.right * (int)pe.facing))
        {
            return State.Climb;
        }

        if (pe.dashDir.y == 0)//横冲
        {
            if (pe.input.GamePlay.Jump.WasPressedThisFrame()&& pe.jumpGraceTimer > 0)
            {
                pe.SuperJump();
                pe.PlayJumpDust();
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
    public override IEnumerator Coroutine()
    {
        Vector2 spdDir = pe.dashDir.normalized;
        Vector2 newSpeed = spdDir * pe.DashSpeed;

        if(spdDir.x==Mathf.Sign(pe.speed.x)&& Mathf.Abs(newSpeed.x)< Mathf.Abs(pe.speed.x))
            newSpeed.x=pe.speed.x;
        pe.speed=newSpeed;

        pe.rd.gravityScale = 0;
        yield return new WaitTime(TimeSet.DashTime);

        if(spdDir.y>=0)
        {
            pe.speed = spdDir * pe.EndDashSpeed;
        }
        if(pe.speed.y>0)
        {
            pe.speed.y *= SpdSet.EndDashUpMult;
        }

        returnState = State.Normal;

    }
}