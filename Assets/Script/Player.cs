using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{

    public PlayerInput input;

    #region vars

    public Vector2 moveInput;

    #endregion

    private void Awake()
    {
        input = new PlayerInput();
    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = input.GamePlay.Move.ReadValue<Vector2>();
    }
}
