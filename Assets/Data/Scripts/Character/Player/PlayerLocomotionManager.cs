using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    CharacterManager characterManager;
    PlayerManager playerManager;
    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;

    [Header("Movement Settings")]
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float sprintingSpeed = 8;
    [SerializeField] float rotationSpeed = 7;

    [SerializeField] float sprintingStaminaCost = 2;

    [Header("Dodge")]
    private Vector3 rollDirection;
    [SerializeField] float dodgeStaminaCost = 25;

    [Header("Jump")]
    [SerializeField] float jumpStaminaCost = 25;
    [SerializeField] float gravityAcceleration = -9.8f;
    [SerializeField] float inAirTimer = 0;

    [SerializeField] LayerMask groundLayerMask;

    [SerializeField] float jumpHeight = 2;

    [SerializeField] float verticalVelocity = 0;

    [SerializeField] float sphereRadius = 0.2f;
    protected Vector3 jumpDirection;

  

    

    

    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
        characterManager = GetComponent<CharacterManager>();
    }

    protected override void Update()
    {
        base.Update();
        if(playerManager.IsOwner){
            playerManager.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            playerManager.characterNetworkManager.verticalMovement.Value = verticalMovement;
            playerManager.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        else{
            horizontalMovement = playerManager.characterNetworkManager.horizontalMovement.Value;
            verticalMovement = playerManager.characterNetworkManager.verticalMovement.Value;
            moveAmount = playerManager.characterNetworkManager.moveAmount.Value;

            playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
        }
        // 随时通知animator的isGrounded的状态以实现完美的状态转换
        playerManager.animator.SetBool("isGrounded",playerManager.isGrounded);
       
       
        HandleJumpMovement();

        
    }

    public void HandleAllMovements(){
        handleGroundedMovement();
        handleRotation();
    }

    private void GetMovementInputs(){
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }
    private void handleGroundedMovement(){
        if(!playerManager.canMove)
            return;
        GetMovementInputs();
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement +
                        PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.y = 0;
        moveDirection.Normalize();

        // 区分步行和奔跑
        float currentSpeed;
        if(PlayerInputManager.instance.isRunning && PlayerInputManager.instance.isSprinting){
            currentSpeed = sprintingSpeed;
        }
        else if(PlayerInputManager.instance.isRunning){
            currentSpeed = runningSpeed;
        }
        else{
            currentSpeed = walkingSpeed;
        }
        Vector3 movement = moveDirection * currentSpeed * Time.deltaTime;
        playerManager.characterController.Move(movement);
    }

    private void handleRotation(){
        if(!playerManager.canRotate)
            return;
        targetRotationDirection += PlayerCamera.instance.transform.forward * verticalMovement;
        targetRotationDirection += PlayerCamera.instance.transform.right * horizontalMovement;
        targetRotationDirection.y = 0;
        targetRotationDirection.Normalize();

        if(targetRotationDirection == Vector3.zero)
            targetRotationDirection = transform.forward;

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, 
                                                    newRotation, 
                                                    rotationSpeed * Time.deltaTime);
        
        transform.rotation = targetRotation;

    }

    public void AttemptToPerformDodge(){
        if(playerManager.playerNetworkManager.currentStamina.Value <= 0)
            return;
        if(playerManager.isPerformingAction)
            return;
        // 按下方向键就准备翻滚
        if(PlayerInputManager.instance.moveAmount > 0){
            playerManager.playerAnimatorManager.PlayTargetActionAnimation("roll_forward", true, true, false, false);
        }
        else
            playerManager.playerAnimatorManager.PlayTargetActionAnimation("backstep", true, true, false, false);
        
        playerManager.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;

        
    }

    public void HandleSprint(float moveAmount){
        playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
        playerManager.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
    }

    public void AttemptToJump(){
        if(playerManager.playerNetworkManager.currentStamina.Value <= 0)
            return;
        if(playerManager.isPerformingAction)
            return;
        if(playerManager.isJumping)
            return;
        if(!playerManager.isGrounded)
            return;
        // 播放跳跃动画
        playerManager.playerAnimatorManager.PlayTargetActionAnimation("main_jump_start",false,false,false,false);
        // 减少体力
        playerManager.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;
        // 设置状态为正在跳跃
        playerManager.isJumping = true;

        jumpDirection = moveDirection;
        if(moveAmount == 1){
            jumpDirection *= 2;
        }else if(moveAmount ==  2){
            jumpDirection *= 4;
        }else if(moveAmount ==  3){
            jumpDirection *= 6;
        }

    }

    public void HandleJumpMovement(){
        


        // 检查是否着地
        CheckIfIsGrounded();
        
        if(playerManager.isGrounded){
            // 从空中着地，速度一定向下，重置垂直方向的速度以及定时器
            if(verticalVelocity < 0){
                inAirTimer = 0;
                verticalVelocity = 0;
            }
        }
        else{
            // 在空中，定时开始，并且根据定时更新动画
            inAirTimer += Time.deltaTime;
            playerManager.animator.SetFloat("inAirTimer", inAirTimer);
                
        }
        // 根据垂直速度一直移动（垂直速度为0不移动）
        verticalVelocity += gravityAcceleration * Time.deltaTime;
        playerManager.characterController.Move(Vector3.up*verticalVelocity*Time.deltaTime);

        if(playerManager.isJumping){
            playerManager.characterController.Move(jumpDirection*Time.deltaTime);
        }
    }

    // 检查是否着地
    public void CheckIfIsGrounded(){
        playerManager.isGrounded = Physics.CheckSphere(transform.position, sphereRadius, groundLayerMask);
    }

    // 在起跳动画的某一帧加上一个向上的垂直速度
    public void ApplyJumpVelocity(){
        // v方=2*g*h，根据跳跃高度和重力加速度决定初始速度
        verticalVelocity = Mathf.Sqrt(-2 * gravityAcceleration * jumpHeight);
    }

    // 画球可视化是否与地面接触
    private void OnDrawGizmosSelected(){
        //Gizmos.DrawSphere(transform.position, sphereRadius);
    }

    
}
