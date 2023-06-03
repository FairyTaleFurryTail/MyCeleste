using System.Collections;
using UnityEngine;

public partial class PlayerEntity : MonoBehaviour
{
    #region Animations' name & Setting
    private const string JumpDown=nameof(JumpDown);
    private const string JumpUp = nameof(JumpUp);
    private const string Run = nameof(Run);
    private const string Duck = nameof(Duck);
    private const string Dash = nameof(Dash);
    private const string Idle = nameof(Idle);
    private const string Climb = nameof(Climb);

    private const float AnimaRunThreshold = 3;
    #endregion

    //既然我已经写了一个动作状态机了，就没必要用Unity的动画状态机连线了
    private void UpdateAnim()
    {
        switch (stateMachine.state)
        {
            case (int)State.Normal:
                if (speed.y < 0)
                {
                    anim.Play(JumpDown);
                    break;
                }
                else if (speed.y>0)
                {
                    anim.Play(JumpUp);
                    break;
                }
                if (Ducking)
                {
                    anim.Play(Duck);
                    break;
                }
                if(Mathf.Abs(speed.x)> AnimaRunThreshold)
                    anim.Play(Run);
                else 
                    anim.Play(Idle);
                break;
            case (int)State.Dash:
                anim.Play(Dash); 
                break;
            case (int)State.Climb :
                if (speed.y > 0)
                {
                    anim.Play(Climb);
                    anim.SetFloat("ClimbSpeed", Mathf.Lerp(0, 1, speed.y / ClimbUpSpeed));
                }
                else if (speed.y <= 0)//只播放一帧
                {
                    //climbSpeed = Mathf.Lerp(0, -1, speed.y / ClimbDownSpeed);
                    if (speed.y == 0)
                        anim.Play(Climb, 0, 0.4f);
                    else
                        anim.Play(Climb, 0, 0);
                    anim.SetFloat("ClimbSpeed", 0);
                }
                break;
        }
    }

    private IntervalTimer flashInterval = new IntervalTimer(TimeSet.FlashInterval);
    private bool flashing = false;
    private void UpdateSprite()
    {
        scale.x = Mathf.MoveTowards(scale.x, 1, 1.75f * Time.deltaTime);
        scale.y = Mathf.MoveTowards(scale.y, 1, 1.75f * Time.deltaTime);

        flashInterval.start = IsTired;
        flashing = flashInterval.GetStatus();
        if (flashing)
            sprite.color = flashColor;
        else
            sprite.color = Color.white;
    }
}
