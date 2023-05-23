using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine;
using static Consts;

public class DashState : BaseState
{
    public DashState(PlayerEntity p):base(p) 
    { }
    public override State state => State.Dash;

    public override bool haveCoroutine => true;

    
    public override void OnEnd()
    {
        pe.rd.gravityScale = PhySet.Gravity;
    }

    private State returnState;
    private Vector2 dir;
    public override void OnEnter()
    {
        returnState = state;
        dir = pe.input_move;
        if (dir == Vector2.zero)
            dir = new Vector2((float)pe.facing, 0);
        CoroutineManager.Instance.StartCoroutine(Coroutine());
    }

    public override State Update()
    {
        if (pe.input.GamePlay.Jump.WasPressedThisFrame())
            return State.Normal;
        return returnState;
    }
    public override IEnumerator Coroutine()
    {
        Vector2 dashDir = dir.normalized;
        Vector2 newSpeed = dashDir * pe.DashSpeed;

        if(dashDir.x==Mathf.Sign(pe.speed.x)&& Mathf.Abs(newSpeed.x)< Mathf.Abs(pe.speed.x))
            newSpeed.x=pe.speed.x;
        pe.speed=newSpeed;

        pe.rd.gravityScale = 0;
        yield return new WaitTime(Times.DashTime);

        if(dashDir.y>=0)
        {
            pe.speed = dashDir * pe.EndDashSpeed;
        }
        if(pe.speed.y>0)
        {
            pe.speed.y *= SpdSet.EndDashUpMult;
        }

        returnState = State.Normal;

    }
}
