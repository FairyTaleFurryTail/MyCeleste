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
    [Header("����")]
    public BoxCollider2D footBox;
    public BoxCollider2D bodyBox;
    #region vars

    private ActionStateMechine stateMachine;
    private IntroTypes introType;

    //״̬��������
    public bool onGround;

    #endregion

    #region �����ź�
    [Header("����")]
    public Vector2 input_move;
    #endregion

    #region ��ʼ������
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
        //��������
        input_move = input.GamePlay.Move.ReadValue<Vector2>();

        //���ּ�ʱ��

        //��������
        onGround=CheckGroud();

        stateMachine.Update();

        AnimUpdate();

    }

    #region ���Jump
    public void Jump()
    {
        
    }
    #endregion

    #region ����
    [Header("����")]
    public float MaxRun;
    public float RunReduce;
    public float RunAccel;
    #endregion

}
