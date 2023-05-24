using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Normal,
    Climb,
    Dash
}

public partial class ActionStateMechine
{
    private int _state;
    public List<BaseState> states;
    private CoroutineManager coroutineManager;
    //private Coroutine coroutine;
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
            _state = value;
            states[_state].OnEnter();
            if (states[_state].haveCoroutine)
                coroutineManager.StartCoroutine(states[_state].Coroutine());
        }
    }

}
