using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class IntervalTimer
{
    private float maxTime;
    private float curTime;
    private bool positive;
    private bool _start;
    public bool start
    {
        get { return _start; }
        set 
        {
            if(_start==false&&_start!=value)
            {
                positive = true;
                curTime = maxTime;
            }
            _start = value;
        }
    }
    public IntervalTimer(float timeSet)
    {
        curTime = maxTime = timeSet;
        positive = true;
    }

    public bool GetStatus()
    {
        if (!start) return false;
        curTime -= Time.deltaTime;
        if(curTime < 0) 
        {
            curTime = maxTime;
            positive = !positive;
        }
        return positive;
    }
    
}
