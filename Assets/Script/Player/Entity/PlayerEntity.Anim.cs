using System.Collections;
using System.Data.Common;
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
    private void UpdateAnimAndTail()
    {
        Vector2 tailOffset = TailMove.idleOffset;
        float tailSpeed= TailMove.idleSpeed;
        switch (stateMachine.state)
        {
            case (int)State.Normal:
                if (speed.y < 0)
                {
                    anim.Play(JumpDown);
                    tailOffset = TailMove.jumpDownOffset;
                    tailSpeed = TailMove.jumpDownSpeed;
                    break;
                }
                else if (speed.y>0)
                {
                    anim.Play(JumpUp);
                    tailOffset = TailMove.jumpUpOffset;
                    tailSpeed = TailMove.jumpUpSpeed;
                    break;
                }
                if (Ducking)
                {
                    anim.Play(Duck);
                    tailOffset = TailMove.duckOffset;
                    tailSpeed = TailMove.duckSpeed;
                    break;
                }
                if (Mathf.Abs(speed.x) > AnimaRunThreshold)
                {
                    anim.Play(Run);
                    tailOffset = TailMove.runOffset;
                    tailSpeed = TailMove.runSpeed;
                }
                else
                {
                    anim.Play(Idle);
                    tailOffset = TailMove.idleOffset;
                    tailSpeed=TailMove.idleSpeed;
                }
                break;
            case (int)State.Dash:
                anim.Play(Dash);
                tailOffset = TailMove.dashOffset.x * speed.normalized;
                tailSpeed = TailMove.dashSpeed;
                break;
            case (int)State.Climb :
                if (speed.y > 0)
                {
                    anim.Play(Climb);
                    anim.SetFloat("ClimbSpeed", Mathf.Lerp(0, 1, speed.y / ClimbUpSpeed));
                    tailOffset = TailMove.climbUpOffset;
                    tailSpeed=TailMove.climbUpSpeed;
                }
                else if (speed.y <= 0)//只播放一帧
                {
                    //climbSpeed = Mathf.Lerp(0, -1, speed.y / ClimbDownSpeed);
                    if (speed.y == 0)
                    {
                        anim.Play(Climb, 0, 0.4f);
                        tailOffset = TailMove.idleOffset;
                        tailSpeed = TailMove.idleSpeed;
                    }
                    else
                    {
                        anim.Play(Climb, 0, 0);
                        tailOffset = TailMove.climbDownOffset;
                        tailSpeed= TailMove.climbDownSpeed;
                    }
                    anim.SetFloat("ClimbSpeed", 0);
                }
                break;
        }

        if(stateMachine.state!=(int)State.Dash)
            tailOffset.x *= (float)facing*-1;
        tail.UpdateShape(tailOffset, tailSpeed);
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
