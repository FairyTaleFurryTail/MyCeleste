using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : MonoBehaviour
{
    private float curTime;
    private float maxTime;
    [SerializeField]private SpriteRenderer sprite;

    public void Init(float time,Sprite sp,int facing,Vector3 scale,Color color)
    {
        curTime = maxTime = time;
        sprite.sprite = sp;
        sprite.color = color;
        scale.x *= facing;
        transform.localScale = scale;
    }


    private void Update()
    {
        if(curTime>0) 
        {
            Color c = sprite.color;
            c.a = Mathf.Lerp(0,1, curTime / maxTime);
            sprite.color = c;
        }
        else
            Destroy(gameObject);
        curTime.TimePassBy();
    }

}
