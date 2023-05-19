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

public class ActionStateMechine
{
    private int _state;
    public List<BaseState> states;
    //private Coroutine coroutine;
    public ActionStateMechine()
    {
        states=new List<BaseState>();

    }
    public void Update()
    {
        state = (int)states[state].Update();
        /*if (coroutine.Active)
        {
            coroutine.Update(deltaTime);
        }*/
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
        }
    }

}
