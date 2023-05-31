using System.Collections;
using UnityEngine;


public static class Calc
{
    public static void TimePassBy(ref this float w)
    {
        if (w > 0) w -= Time.deltaTime;
    }

    
}