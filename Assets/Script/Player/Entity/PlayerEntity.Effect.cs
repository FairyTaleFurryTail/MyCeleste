using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public partial class PlayerEntity : MonoBehaviour
{
    public void PlayJumpDust()=> GameManager.sem.PlayOnce("JumpDust", PosSet.foot+transform.position);
    public void PlayWallJumpDust(float dir) =>
        GameManager.sem.PlayOnce("JumpDust",PosSet.foot+transform.position,new Vector3(0, dir * 90, 0));
    public void PlaySlideDust() { GameManager.sem.PlayKeep("SlideDust", (float)facing * PosSet.front + transform.position); }
    public void PlayDashShadow()
    {
        if (GameManager.sem.dashEffect.CanCreate())
            GameManager.sem.dashEffect.Create(sprite.sprite, 
                (int)facing, dashColors[lastDashIndex], transform.position+anim.transform.localPosition,scale);
        //用ParticleSystem我不知道怎么 改变且仅改变 一张图片
        /*ParticleSystem ps = GameManager.sem.PlayKeep("DashShadow", transform.position, scale: new Vector3(lastDashFacing, 1, 1));
        //ps.GetComponent<ParticleSystemRenderer>().material.mainTexture = anim.GetComponent<SpriteRenderer>().sprite.texture;
        ps.startColor = dashColors[lastDashIndex];*/
    }

    private void PlaySpeedRing()
    {
        Vector2 dir = speed.normalized;
        if (dir.y < 0) dir *= -1;
        float angle = Vector2.Angle(Vector2.right, dir);
        if (Mathf.Abs(90-angle)>=35)
        {
            ParticleSystem ps = GameManager.sem.PlayKeep("SpeedRing", transform.position, speed.normalized,time:0.1f);
            if (ps == null) return;
            ps.startRotation = angle * Mathf.PI / 180;
        }  
    }
        
}
