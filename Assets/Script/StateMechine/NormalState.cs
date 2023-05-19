using System.Collections;
using UnityEngine;
using static Consts;

public class NormalState : BaseState
{
    public override State state => (int)State.Normal;

    public override bool haveCoroutine => false;

    public NormalState(PlayerEntity p):base(p)
    {

    }

    public override IEnumerator Coroutine()
    {
        yield return null;
    }

    public override void OnEnd()
    {
        
    }

    public override void OnEnter()
    {
        
    }

    public override State Update()
    {
        Vector2 speed = pe.rd.velocity;

        //x轴速度计算
        {
            float mult = pe.onGround ? 1 : PlayConst.AirMult;
            float max = pe.MaxRun;
            float moveX = pe.input_move.x;

            
            float acc = pe.RunAccel;
            //可能会速度过快
            
            if (Mathf.Abs(speed.x) > max && Mathf.Sign(speed.x) == moveX)
                acc= pe.RunReduce;
            speed.x = Mathf.MoveTowards(speed.x, max * moveX, acc * mult * Time.deltaTime);
        }

        //y轴速度计算
        {

        }

        pe.rd.velocity = speed;

        //跳跃
        if (pe.input.GamePlay.Jump.ReadValue<bool>())
        {
            pe.Jump();
        }

        return State.Normal;
    }
}
