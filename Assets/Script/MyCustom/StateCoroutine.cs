using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class StateCoroutine
{
    private IEnumerator _routine;

    public StateCoroutine(IEnumerator routine)
    {
        _routine = routine;
    }

    public bool MoveNext()
    {
        if (_routine == null)
            return false;

        IWait wait = _routine.Current as IWait;
        bool waiting = false;
        if (wait != null)
            waiting = !wait.Done();
        if (waiting)
        {
            return true;
        }
        return _routine.MoveNext();
    }

    public void Stop()
    {
        _routine = null;
    }

}

public interface IWait
{
    bool Done();
}
public class WaitTime : IWait
{
    public float waitTime = 0;

    public WaitTime(float time)
    {
        waitTime = time;
    }
    bool IWait.Done()
    {
        waitTime -= Time.deltaTime;
        return waitTime <= 0;
    }
}