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
    #region ����
    public PlayerInput input;
    [HideInInspector]public Rigidbody2D rd;
    private ActionStateMechine stateMachine;
    private IntroTypes introType;
    [Header("����")]
    [Title("��������"), SerializeField] private Animator sprite;
    //public BoxCollider2D footBox;
    [HideInInspector]public BoxCollider2D bodyBox;
    public BoxCollider2D normalBox;
    public BoxCollider2D duckBox;
    //public BoxCollider2D handBox;
    //public BoxCollider2D frontWallCheckBox;
    //public BoxCollider2D backWallCheckBox;
    public int scaleMult;
    #endregion

    #region vars
    /// <summary>�ٶȣ�����Update���ֵ</summary>
    public Vector2 speed;
    public Vector2 dashDir;
    public float maxFall;
    /// <summary>�����ٶ����ڼ��㳤��</summary>
    [HideInInspector] public float varJumpSpeed;
    /// <summary>��Ծһ��ʱ���ڲ���ת��</summary>
    [HideInInspector] public int forceMoveX;
    [HideInInspector] public Face facing;
    [HideInInspector] public int wallSlideDir;
    public Vector3 scale;

    //״̬��������
    private bool _onGround;
    public bool onWall;
    #endregion

    #region ��ʱ��
    [HideInInspector] public float varJumpTimer;
    /// <summary>����ʱ��</summary>
    [HideInInspector] public float jumpGraceTimer;
    [HideInInspector] public float forceMoveXTimer;
    [HideInInspector] public float climbButtonTimer;
    [HideInInspector] public float dashAttackTimer;
    [HideInInspector] public float wallSlideTimer;
    public bool DashAttacking
    {
        get
        {
            return dashAttackTimer > 0;
        }
    }
    #endregion

    #region �����ź�
    [Header("����")]
    public Vector2 input_move;
    #endregion

    private Queue<Collider2D>colliderQueue=new Queue<Collider2D>();

    #region ��ʼ������
    private void Awake()
    {
        input = new PlayerInput();
        stateMachine = new ActionStateMechine();
        stateMachine.states.Add(new NormalState(this));
        stateMachine.states.Add(new ClimbState(this));
        stateMachine.states.Add(new DashState(this));
        rd = GetComponent<Rigidbody2D>();
        Ducking = false;

        scale = Vector3.one;
        facing = Face.Right;
        maxFall = SpdSet.MaxFall;
    }
    private void OnEnable()
    {
        input.Enable();
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

        //��������
        input_move = input.GamePlay.Move.ReadValue<Vector2>();
        input_move.x = input_move.x > 0 ? 1 : input_move.x < 0 ? -1 : 0;
        input_move.y = input_move.y > 0 ? 1 : input_move.y < 0 ? -1 : 0;

        //��������
        onGround = CheckGround(Vector2.zero);

        //���ּ�ʱ��
        if (varJumpTimer > 0) varJumpTimer -= Time.deltaTime;
        if(dashAttackTimer>0) dashAttackTimer-=Time.deltaTime;

        if (onGround)
        {
            jumpGraceTimer = Times.JumpGraceTime;
        }
        else if (jumpGraceTimer > 0)
            jumpGraceTimer -= Time.deltaTime;

        //����Ϸ����input.x
        if (forceMoveXTimer > 0)
        {
            forceMoveXTimer -= Time.deltaTime;
            input_move.x = forceMoveX;
        }

        // ��������
        if(input_move.x!=0&&stateMachine.state!=(int)State.Climb)
        {
            facing = (Face)input_move.x;
        }

        UpdateSprite();

        //״̬��
        stateMachine.Update();

        
        AnimUpdate();
            
        rd.velocity = speed;
        scale.x *= (int)facing;
        sprite.transform.localScale = scale*scaleMult;
        scale.x *= (int)facing;
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        ContactPoint2D[] contactPoint = new ContactPoint2D[1];//�洢��ײ����ĵ�λ����
        collider.GetContacts(contactPoint);//��ȡ��ײ��
        //normal��ָ�������ײ���λ��
        Vector2 normal = contactPoint[0].normal;
        if(normal.y>0)
        {
            if (speed.y < 0)
            {
                float squish = Mathf.Min(speed.y / SpdSet.FastMaxFall, 1);
                scale.x = Mathf.Lerp(1, 1.6f, squish);
                scale.y = Mathf.Lerp(1, .4f, squish);
            }
        }
        //colliderQueue.
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
