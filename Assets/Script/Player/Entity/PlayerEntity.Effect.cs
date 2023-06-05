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
        ParticleSystem ps = GameManager.sem.PlayKeep("DashShadow", transform.position, scale: new Vector3(lastDashFacing, 1, 1));
        //ps.GetComponent<ParticleSystemRenderer>().material.mainTexture = anim.GetComponent<SpriteRenderer>().sprite.texture;
        ps.startColor = dashColors[lastDashIndex];
    }
}
