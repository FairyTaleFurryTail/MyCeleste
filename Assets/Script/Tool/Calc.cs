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