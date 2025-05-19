using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("PlayerMove Variable")]
    public CharacterController characterController;
    public Animator animator;
    public PlayerConfig playerConfig;
    public float verticalInput;
    public float moveSpeed;
    public float rotationSpeed;
    public bool isMoving;
    public bool isRunning;
    public float horizontalInput;
    public int isMovingHash = Animator.StringToHash("IsMoving");
    public int moveSpeedHash = Animator.StringToHash("MoveSpeed");


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        HandleInput();
        if (isMoving)
        {
            PlayerMove();
        }
        else
        {
            PlayerMoveAnimation();
        }
    }


    void HandleInput()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Mouse X");
        isMoving = verticalInput != 0;
        isRunning = Input.GetButton("LeftShift") && verticalInput > 0;
    }

    #region PlayerMove
    void PlayerMove()
    {
        PlayerMoveTransform();
        PlayerMoveAnimation();
        PlayerRotation(); // Cần thay đổi vị trí của hàm để khi người chơi đứng yên vẫn có thể xoay và thêm animation xoay người
    }
    void PlayerMoveTransform()
    {
        characterController.Move(transform.forward * moveSpeed * Time.deltaTime);
        CaculationMoveSpeed();
    }
    void CaculationMoveSpeed()
    {
        if (isRunning)
        {
            moveSpeed = playerConfig.moveSpeedFactor * verticalInput * 2; // Cần có giãi pháp tăng dần moveSpeed nếu người chới đang đi chuyển sang chạy
        }
        else if (Mathf.Abs(verticalInput) <= 0.01) // Trả movespeed về 0 trong trường hợp người chơi không còn di chuyển
        {
            moveSpeed = 0;
        }
        else
        {
            moveSpeed = playerConfig.moveSpeedFactor * verticalInput;
        }
    }
    void PlayerMoveAnimation()
    {
        animator.SetBool(isMovingHash, isMoving);
        animator.SetFloat(moveSpeedHash, moveSpeed);
    }
    void PlayerRotation()
    {
        CaculationRotationSpeed();
        transform.Rotate(0,horizontalInput,0);
    }
    void CaculationRotationSpeed()
    {
        rotationSpeed = horizontalInput * playerConfig.rotationSpeedFactor;
    }
    #endregion
}
