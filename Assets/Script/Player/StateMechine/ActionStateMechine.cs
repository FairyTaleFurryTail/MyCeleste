using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Normal,
    Climb,
    Dash,
    Dead
}

public partial class ActionStateMechine
{
    private int _state;
    public List<BaseState> states;
    private CoroutineManager coroutineManager;
    public ActionStateMechine()
    {
        states=new List<BaseState>();
        coroutineManager = new CoroutineManager();
    }
    public void Update()
    {
        state = (int)states[state].Update();
        coroutineManager.Update();
    }
    

    public int state
    {
        get
        {
            return _state;
        }
        set
        {
            if (_state == value)
                return;
            states[_state].OnEnd();
            if (states[_state].haveCoroutine)
                coroutineManager.CloseCoroutine(states[_state].coroutine);
            _state = value;
            states[_state].OnEnter();
            if (states[_state].haveCoroutine)
                states[_state].coroutine = coroutineManager.StartCoroutine(states[_state].Coroutine()) ;
        }
    }

}
