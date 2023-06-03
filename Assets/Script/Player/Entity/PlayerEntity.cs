using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerEntity;

public enum IntroTypes { Transition, Respawn, WalkInRight, WalkInLeft, Jump, WakeUp, Fall, None }
public enum Face {Left=-1,Right=1}

public partial class PlayerEntity: MonoBehaviour
{
    public Vector2 idleOffset,runOffset,jumpUpOffset,jumpDownOffset,duckOffset,dashOffset,climbUpOffset,climbDownOffset;

    #region ����
    public PlayerInput input;
    [HideInInspector]public Rigidbody2D rd;
    [SerializeField]private Tail tail;
    private ActionStateMechine stateMachine;
    private IntroTypes introType;
    [Header("����")]
    [Title("��������"), SerializeField] private Animator anim;
    [SerializeField] private Color flashColor;
    private SpriteRenderer sprite;
    [HideInInspector]public BoxCollider2D bodyBox;
    public BoxCollider2D normalBox;
    public BoxCollider2D duckBox;
    public int scaleMult;
    #endregion

    #region vars
    /// <summary>�ٶȣ�����Update���ֵ</summary>
    public Vector2 speed;
    [HideInInspector] public Vector2 dashDir;
    [HideInInspector] public int dashes;
    public int maxDashes;
    [HideInInspector] public float maxFall;
    /// <summary>�����ٶ����ڼ��㳤��</summary>
    [HideInInspector] public float varJumpSpeed;
    /// <summary>��Ծһ��ʱ���ڲ���ת��</summary>
    [HideInInspector] public int forceMoveX;
    [HideInInspector] public Face facing;
    [HideInInspector] public int wallSlideDir;
    [HideInInspector] public bool dashStartedOnGround;
    [HideInInspector] public Vector3 scale;
    

    //״̬��������
    private bool _onGround;
    public float Stamina;
    #endregion

    #region ��ʱ��
    [HideInInspector] public float varJumpTimer;
    /// <summary>����ʱ��</summary>
    [HideInInspector] public float jumpGraceTimer;
    [HideInInspector] public float forceMoveXTimer;

    [HideInInspector] public float dashAttackTimer;
    [HideInInspector] public float dashCooldownTimer;
    [HideInInspector] public float wallSlideTimer;
    #endregion

    [Header("����")]
    [HideInInspector] public Vector2 input_move;

    #region ��ʼ������
    private void Awake()
    {
        input = new PlayerInput();
        stateMachine = new ActionStateMechine();
        stateMachine.states.Add(new NormalState(this));
        stateMachine.states.Add(new ClimbState(this));
        stateMachine.states.Add(new DashState(this));
        rd = GetComponent<Rigidbody2D>();
        sprite=anim.GetComponent<SpriteRenderer>();

        scale = Vector3.one;
        facing = Face.Right;
    }
    private void OnEnable()
    {
        input.Enable();
        maxFall = SpdSet.MaxFall;
        Ducking = false;
        Stamina = ClimbSet.ClimbMaxStamina;
    }
    private void OnDisable()
    {
        input.Disable();
    }
    
    void Start()//�������ͼ��
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

        //����
        input_move = input.GamePlay.Move.ReadValue<Vector2>();
        input_move.x = input_move.x > 0 ? 1 : input_move.x < 0 ? -1 : 0;
        input_move.y = input_move.y > 0 ? 1 : input_move.y < 0 ? -1 : 0;

        //��������
        onGround = CastCheckCollider(Vector2.zero,Vector2.down);

        #region ���ּ�ʱ��
        varJumpTimer.TimePassBy();
        dashAttackTimer.TimePassBy();
        dashCooldownTimer.TimePassBy();

        if (onGround)
        {
            jumpGraceTimer = TimeSet.JumpGraceTime;
            dashes = maxDashes;
        }
        else
        { jumpGraceTimer.TimePassBy(); }

        //����Ϸ����input.x
        if (forceMoveXTimer > 0)
        {
            forceMoveXTimer -= Time.deltaTime;
            input_move.x = forceMoveX;
        }

        //wallSlideTimer
        if (wallSlideDir != 0)
        {
            wallSlideTimer = Math.Max(wallSlideTimer - Time.deltaTime, 0);
            wallSlideDir = 0;
        }
        #endregion

        #region  var 
        // ����
        if (input_move.x!=0&&stateMachine.state!=(int)State.Climb)
        {
            facing = (Face)input_move.x;
        }
        //Climb���
        if(onGround&& stateMachine.state != (int)State.Climb)
        {
            Stamina = ClimbSet.ClimbMaxStamina;
            wallSlideTimer = TimeSet.WallSlideTime;
        }
        //��ǽ�ȴ���ֵX�ٶ�
        if (hopWaitX != 0)
        {
            if (speed.x * hopWaitX < 0)
                hopWaitX = 0;
            if (!CheckCollider(bodyBox, Vector2.right * (int)facing))
            {
                speed += hopWaitX * hopWaitXSpeed * Vector2.right;
                hopWaitX = 0;
            }
        }

        #endregion

        UpdateAnimAndTail();

        stateMachine.Update();

        ProcessCollisionDatas();

        UpdateSprite();
            
        rd.velocity = speed;
        scale.x *= (int)facing;
        sprite.transform.localScale = scale*scaleMult;
        scale.x *= (int)facing;
    }


    #region ����
    [Header("����")]
    public float MaxRun;
    public float RunReduce;
    public float RunAccel;
    public float Gravity = 90f;
    //��Ծ
    public float JumpSpeed;
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

    //���
    public float DashSpeed = 30f;
    public float EndDashSpeed = 20f;
    #endregion

}
