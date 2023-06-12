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
    public static Color GetLightColor (Color c1,float intensity)
    {
        float factor = Mathf.Pow(2, intensity);
        return new Color(c1.r * factor, c1.g * factor, c1.b * factor);
    }

    public static void TimePassBy(ref this float w,bool unscaled=false)
    {
        float timePass = unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
        if (w > 0) w -= timePass;
    }

    public static Vector2 GetContanctDirection(this Collider2D collider)
    {
        ContactPoint2D[] contactPoint = new ContactPoint2D[1];//碰撞方向的单位向量
        collider.GetContacts(contactPoint);//获取碰撞点
        return contactPoint[0].normal;
    }
    public static Vector2 GetContanctDirection(this Collision2D collision)
    {
        return collision.collider.GetContanctDirection();
    }

}