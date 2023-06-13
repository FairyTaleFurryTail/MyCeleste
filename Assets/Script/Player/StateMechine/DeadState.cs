using System.Collections;
using UnityEngine;


public class DeadState : BaseState
{
    public override State state => State.Dead;

    public DeadState(PlayerEntity p) : base(p) { }

    public override bool haveCoroutine => false;

    public override IEnumerator Coroutine()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnd()
    {
        
    }

    Vector2 targetPos;
    private const float upSpeed = 15;
    private const float upDistance = 2.5f;
    private const float deadTime = 0.5f;
    public override void OnEnter()
    {
        pe.speed = Vector2.zero;
        pe.bodyBox.isTrigger = true;
        targetPos = pe.Position + Vector2.up * upDistance;
        pe.deadTimer = deadTime;
    }

    public override State Update()
    {
        pe.Position=Vector2.MoveTowards(pe.Position, targetPos, upSpeed*Time.deltaTime);
        pe.speed= Vector2.zero;
        return State.Dead;
    }
}
