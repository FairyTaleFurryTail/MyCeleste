using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Consts;

public enum IntroTypes { Transition, Respawn, WalkInRight, WalkInLeft, Jump, WakeUp, Fall, TempleMirrorVoid, None }
public enum Face {Left=-1,Right=1}

public partial class PlayerEntity: MonoBehaviour
{
    #region 配置
    public PlayerInput input;
    public Rigidbody2D rd;
    private ActionStateMechine stateMachine;
    private IntroTypes introType;
    [Header("设置")]
    public BoxCollider2D footBox;
    public BoxCollider2D bodyBox;
    public BoxCollider2D handBox;
    public int scaleMult;
    #endregion

    #region vars
    /// <summary>速度，会在Update最后赋值</summary>
    public Vector2 speed;
    /// <summary>保留速度用于计算长按</summary>
    public float varJumpSpeed;
    public int forceMoveX;
    public Face facing;
    public Vector3 scale;

    //状态计算所需
    public bool onGround;
    public bool onWall;
    #endregion

    #region 计时器
    public float varJumpTimer;
    public float jumpGraceTimer;
    public float forceMoveXTimer;
    public float climbButtonTimer;
    #endregion

    #region 控制信号
    [Header("输入")]
    public Vector2 input_move;
    #endregion

    #region 初始化函数
    private void Awake()
    {
        input = new PlayerInput();
        stateMachine = new ActionStateMechine();
        stateMachine.states.Add(new NormalState(this));
        stateMachine.states.Add(new ClimbState(this));
        rd = GetComponent<Rigidbody2D>();

        scale = Vector2.one;
        facing = Face.Right;
    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }
    
    void Start()//被加入地图后
    {
        switch (introType)
        {
            case IntroTypes.Transition:
                stateMachine.state = (int)State.Normal;
                break;
            default:break;
        }
    }
    #endregion
    
    void Update()
    {
        speed = rd.velocity;

        //各种输入
        input_move = input.GamePlay.Move.ReadValue<Vector2>();
        input_move.x = input_move.x > 0 ? 1 : input_move.x < 0 ? -1 : 0;
        input_move.y = input_move.y > 0 ? 1 : input_move.y < 0 ? -1 : 0;

        //变量计算
        onGround =CheckCollider(footBox);
        onWall = CheckCollider(handBox);

        //各种计时器
        if (varJumpTimer > 0) varJumpTimer -= Time.deltaTime;

        if (onGround)
        {
            jumpGraceTimer = Times.JumpGraceTime;
            speed.y = 0;
        }
        else if (jumpGraceTimer > 0)
            jumpGraceTimer -= Time.deltaTime;

        //跳跃后一定时间不能转向
        if (forceMoveXTimer > 0)
        {
            forceMoveXTimer -= Time.deltaTime;
            input_move.x = forceMoveX;
        }

        // 动作计算
        if(input_move.x!=0&&stateMachine.state!=(int)State.Climb)
        {
            facing = (Face)input_move.x;
        }

        //状态机
        stateMachine.Update();

        AnimUpdate();


        rd.velocity = speed;
        scale.x *= (int)facing;
        transform.localScale = scale*scaleMult;
        scale.x *= (int)facing;
    }


    #region 常量
    [Header("常量")]
    public float MaxRun;
    public float RunReduce;
    public float RunAccel;
    //跳跃
    public float JumpSpeed = 13.125f;
    public float JumpXBoost = 5f;
    public float WallJumpXBoost = 16.125f; //MaxRun+JumpHBoost
    public float ClimbUpSpeed = 45/8f;
    public float ClimbDownSpeed = -10f;
    public float ClimbGrapReduce = 100;
    public float climbButtonTime = .1f;
    #endregion

}
