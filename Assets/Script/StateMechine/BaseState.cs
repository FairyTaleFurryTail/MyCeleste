using System.Collections;
using UnityEngine;

public abstract class BaseState
{
    protected PlayerEntity pe;
    public BaseState(PlayerEntity p)
    {
        pe = p;
    }
    public abstract State state { get; }
    public abstract State Update();
    public abstract void OnEnter();
    public abstract void OnEnd();
    public abstract IEnumerator Coroutine();
    public abstract bool haveCoroutine { get; }

}