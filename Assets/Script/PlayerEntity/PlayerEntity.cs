using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum IntroTypes { Transition, Respawn, WalkInRight, WalkInLeft, Jump, WakeUp, Fall, TempleMirrorVoid, None }

public partial class PlayerEntity: MonoBehaviour
{
    public PlayerInput input;
    public Rigidbody2D rd;
    [Header("设置")]
    public BoxCollider2D footBox;
    public BoxCollider2D bodyBox;
    #region vars

    private ActionStateMechine stateMachine;
    private IntroTypes introType;

    //状态计算所需
    public bool onGround;

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
        rd = GetComponent<Rigidbody2D>();
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
        //各种输入
        input_move = input.GamePlay.Move.ReadValue<Vector2>();

        //各种计时器

        //变量计算
        onGround=CheckGroud();

        stateMachine.Update();

        AnimUpdate();

    }

    #region 许多Jump
    public void Jump()
    {
        
    }
    #endregion

    #region 常量
    [Header("常量")]
    public float MaxRun;
    public float RunReduce;
    public float RunAccel;
    #endregion

}
