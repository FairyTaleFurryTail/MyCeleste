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
    [HideInInspector]public Rigidbody2D rd;
    private ActionStateMechine stateMachine;
    private IntroTypes introType;
    [Header("设置")]
    public BoxCollider2D footBox;
    [HideInInspector]public BoxCollider2D bodyBox;
    public BoxCollider2D normalBox;
    public BoxCollider2D duckBox;
    public BoxCollider2D handBox;
    public BoxCollider2D frontWallCheckBox;
    public BoxCollider2D backWallCheckBox;
    public int scaleMult;
    #endregion

    #region vars
    /// <summary>速度，会在Update最后赋值</summary>
    public Vector2 speed;
    public Vector2 dashDir;
    /// <summary>保留速度用于计算长按</summary>
    [HideInInspector] public float varJumpSpeed;
    /// <summary>跳跃一定时间内不能转向</summary>
    [HideInInspector] public int forceMoveX;
    [HideInInspector] public Face facing;
    public Vector3 scale;

    //状态计算所需
    public bool onGround;
    public bool onWall;
    #endregion

    #region 计时器
    [HideInInspector] public float varJumpTimer;
    /// <summary>土狼时间</summary>
    [HideInInspector] public float jumpGraceTimer;
    [HideInInspector] public float forceMoveXTimer;
    [HideInInspector] public float climbButtonTimer;
    [HideInInspector] public float dashAttackTimer;
    public bool DashAttacking
    {
        get
        {
            return dashAttackTimer > 0;
        }
    }
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
        stateMachine.states.Add(new DashState(this));
        rd = GetComponent<Rigidbody2D>();
        Ducking = false;

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
        onGround =GamePhysics.CheckCollider(footBox);
        onWall = GamePhysics.CheckCollider(handBox);

        //各种计时器
        if (varJumpTimer > 0) varJumpTimer -= Time.deltaTime;
        if(dashAttackTimer>0) dashAttackTimer-=Time.deltaTime;

        if (onGround)
        {
            jumpGraceTimer = Times.JumpGraceTime;
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
    public float SuperWallJumpX { get { return MaxRun + JumpXBoost * 2; } }
    public float SuperWallJumpSpeed = 20f;
    public float SuperJumpX = 260 / 10f;
    public float WallJumpXBoost = 16.125f; //MaxRun+JumpHBoost
    public float ClimbUpSpeed = 45/8f;
    public float ClimbDownSpeed = -10f;
    public float ClimbGrapReduce = 100;
    public float climbButtonTime = .1f;
    public const float DuckSuperJumpXMult = 1.25f;
    public const float DuckSuperJumpYMult = 1.25f;

    //冲刺
    public float DashSpeed = 30f;
    public float EndDashSpeed = 20f;
    #endregion

}
