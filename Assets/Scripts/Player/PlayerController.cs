using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    #region Player move variable
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
    #endregion

    [Header("PlayerAttack Variable")]
    public bool isAttackState;
    public bool isAiming;
    public bool isShootArrow;
    public bool isShooting;
    public int isAttackStateHash = Animator.StringToHash("IsAttackState");
    public int isShootArrowHash = Animator.StringToHash("IsShootArrow");


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        HandleInput();
        if (isAttackState)
        {
            PlayerAttacking();
            return;
        }
        else
        {
            PlayerSetAttackAnimation();
        }
        if (isMoving)
        {
            PlayerMove();
        }
        else
        {
            PlayerSetMoveAnimation();
        }
    }


    void HandleInput()
    {
        isAttackState = IsAttackStateInput() || isShooting;
        isAiming = IsAimingInput();
        isShootArrow = IsShootArrowInput();

        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Mouse X");
        isMoving = verticalInput != 0;
        isRunning = Input.GetButton("LeftShift") && verticalInput > 0;
    }
    bool IsAttackStateInput()
    {
        if (StartAttackInput())
        {
            isAttackState = true;
        }
        else if (EndAttackInput())
        {
            isAttackState = false;
        }
        return isAttackState;
    }
    bool StartAttackInput()
    {
        return Input.GetMouseButton(1);
    }
    bool EndAttackInput()
    {
        return Input.GetMouseButtonUp(1);
    }
    bool IsAimingInput()
    {
        return Input.GetMouseButton(0);
    }
    bool IsShootArrowInput()
    {
        return Input.GetMouseButtonUp(0);
    }

    void PlayerAttacking()
    {
        PlayerAttack();
        PlayerSetAttackAnimation();
    }
    void PlayerAttack()
    {
        if (isAiming)
        {
            PlayerAiming();
        }
        else if (isShootArrow)
        {
            PlayerShootArrow();
        }
    }
    void PlayerAiming()
    {
        Debug.Log("A");
    }
    void PlayerShootArrow()
    {
        Debug.Log("Shoot");
        PlayerSetShootArrowAnimation();
    }
    void IsShooting()
    {
        isShooting = !isShooting;
    }
    void ForceEndAttackState()
    {
        if (isAttackState = true && !IsAttackStateInput())
        {
            isAttackState = false;
        }
    }
    void PlayerSetShootArrowAnimation()
    {
        animator.SetTrigger(isShootArrowHash);
    }
    void PlayerSetAttackAnimation()
    {
        animator.SetBool(isAttackStateHash, isAttackState);
    }

    #region PlayerMove
    void PlayerMove()
    {
        PlayerMoveTransform();
        PlayerSetMoveAnimation();
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
    void PlayerSetMoveAnimation()
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
