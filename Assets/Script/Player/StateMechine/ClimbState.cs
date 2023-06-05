using System.Collections;
using UnityEngine;
using static PlayerEntity;
public class ClimbState : BaseState
{
    private float climbNoMoveTimer;

    public ClimbState(PlayerEntity p) : base(p)
    {
    }

    public override State state => State.Climb;

    public override bool haveCoroutine => false;

    public override IEnumerator Coroutine()
    {
        yield return null;
    }

    public override void OnEnd()
    {
        
    }

    public override void OnEnter()
    {
        pe.speed.x = 0;
        pe.speed.y = TimeSet.ClimbGrabYMult;
        //pe.climbButtonTimer = 0;
        climbNoMoveTimer = TimeSet.ClimbNoMoveTime;
        pe.rd.gravityScale = 0;
        //pe.rd.isKinematic = true;
    }

    public override State Update()
    {
        if (climbNoMoveTimer > 0) climbNoMoveTimer -= Time.deltaTime;
        if (pe.onGround) pe.Stamina = ClimbSet.ClimbMaxStamina;

        //处理跳跃
        if ((pe.input.GamePlay.Jump.WasPressedThisFrame()))
        {
            if (pe.input_move.x == (int)pe.facing * -1)
            {
                pe.WallJump((int)pe.input_move.x);
                pe.PlayWallJumpDust(-(int)pe.input_move.x);
            }
            else
                pe.ClimbJump();
            return State.Normal;
        }


        if (!pe.input.GamePlay.Climb.IsPressed())
            return State.Normal;

        if(!pe.CheckCollider(pe.bodyBox,(int)pe.facing*Vector2.right))
        {
            if(pe.speed.y>=0)
            {
                pe.ClimbHop();
            }
            
            return State.Normal;
        }

        //pe.speed.y = Mathf.MoveTowards(pe.speed.y,0, pe.ClimbGrapReduce * UnityEngine.Time.deltaTime);
        float target=0;

        if (climbNoMoveTimer<=0)
        {
            if(pe.input_move.y==1)
            {
                target = pe.ClimbUpSpeed;
            }
            else if(pe.input_move.y==-1)
            {
                if(!pe.onGround)
                {
                    target = pe.ClimbDownSpeed;
                }
            }
        }
        
        pe.speed.y = Mathf.MoveTowards(pe.speed.y, target, SpdSet.ClimbAccel * Time.deltaTime);


        if (climbNoMoveTimer <= 0)
        {
            if(target>0)
            {
                pe.Stamina -= Time.deltaTime * ClimbSet.ClimbUpCost;
            }
            else if(target==0)
            {
                pe.Stamina -= Time.deltaTime * ClimbSet.ClimbStillCost;
            }
        }

        if (pe.Stamina <= 0)
            return State.Normal;

        return State.Climb; 
    }
}
