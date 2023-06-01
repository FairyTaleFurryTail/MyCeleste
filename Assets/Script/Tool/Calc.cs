using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum MyEnum
{
    Value1,
    Value2,
    Value3
}
public static class Calc
{
    public static void TimePassBy(ref this float w)
    {
        if (w > 0) w -= Time.deltaTime;
    }

}