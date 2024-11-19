using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{

    public static PlayerInputManager instance;
    public PlayerManager playerManager;
    PlayerControls playerControls;
    [Header("Movement Input")]
    [SerializeField] Vector2 moveInput;
    [SerializeField] public float horizontalInput;
    [SerializeField] public float verticalInput;
    [SerializeField] public float moveAmount;

    [Header("Camera Movement Input")]
    [SerializeField] Vector2 cameraInput;
    [SerializeField] public float horizontalCameraInput;
    [SerializeField] public float vertiCalcameralInput;

    [Header("Player Action Input")]
    [SerializeField] public bool dodgeInput; 
    [SerializeField] public bool jumpInput;
    [SerializeField] public bool isSprinting;
    [SerializeField] bool switchRightWeaponInput = false;
    [SerializeField] bool switchLeftWeaponInput = false;

    [Header("Bumper Input")]
    [SerializeField] public bool leftMouseInput;

    [Header("Lock On")]
    [SerializeField] bool lockOnInput;

    [Header("Trigger Input")]
    [SerializeField] bool rightMouseInput;
    [SerializeField] bool holdShiftInput;

    
    [SerializeField] public bool isRunning;
    [SerializeField]float clampedHorizontalInput;
    [SerializeField]float clampedVerticalInput;

    

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnSceneChange;
        instance.enabled = false;
    }

    private void Update() {
        HandleAllInputs();
    }

    private void HandleAllInputs(){
        HandleMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleSprintInput();
        HandleJumpInput();
        HandleLeftMouseInput();
        HandleLockOn();
        HandleRightMouseInput();
        HandleChargeRightMouseInput();
        HandleSwitchRightWeaponInput();
        HandleSwitchLeftWeaponInput();
    }

    private void OnSceneChange(Scene oldScene, Scene newScene){
        if(newScene.buildIndex == WorldSaveManager.instance.GetWorldSceneIndex()){
            
            instance.enabled = true;
        }
    }

    private void OnEnable() {
        if(playerControls == null){
            playerControls = new PlayerControls();

            playerControls.PlayerMovements.Movement.performed += i => moveInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovements.Movement.canceled += i => moveInput = Vector2.zero;
            playerControls.PlayerMovements.Run.performed += i => isRunning = true;
            playerControls.PlayerMovements.Run.canceled += i => isRunning = false;
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.canceled += i => cameraInput = Vector2.zero;
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
            playerControls.PlayerActions.Sprint.performed += i => isSprinting = true;
            playerControls.PlayerActions.Sprint.canceled += i => isSprinting = false;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.LeftMouse.performed += i=> leftMouseInput = true;
            playerControls.PlayerActions.Lockon.performed += i => lockOnInput = true;

            // triggers
            playerControls.PlayerActions.RightMouse.performed += i => rightMouseInput = true;
            playerControls.PlayerActions.HoldShift.performed += i => holdShiftInput = true;
            playerControls.PlayerActions.HoldShift.canceled += i => holdShiftInput = false;

            // switch weapon
            playerControls.PlayerActions.SwitchRightWeapon.performed += i => switchRightWeaponInput = true;
            playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switchLeftWeaponInput = true;

        }

        playerControls.Enable();
    }

    private void OnDestroy() {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void HandleMovementInput(){
        // moveAmount: 1 walk        2 run        3 sprint  
        horizontalInput = moveInput.x;
        verticalInput = moveInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput)+Mathf.Abs(verticalInput));
        if(moveAmount!=0)
            moveAmount = isRunning?2:1;
        
        playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);

    }

    private void HandleSwitchRightWeaponInput(){
        if(switchRightWeaponInput){
            switchRightWeaponInput = false;
            playerManager.playerEquipmentManager.SwitchRightWeapon();
        }
    }

    private void HandleSwitchLeftWeaponInput(){
        if(switchLeftWeaponInput){
            switchLeftWeaponInput = false;
            playerManager.playerEquipmentManager.SwitchLeftWeapon();
        }
    }


    private void HandleLockOn(){
        
        if(lockOnInput){
            if(playerManager.playerNetworkManager.isLockedOn.Value){
                if(playerManager.playerCombatManager.currentAttackTarget == null) return;
                if(playerManager.playerCombatManager.currentAttackTarget.isDead.Value == true){
                    playerManager.playerNetworkManager.isLockedOn.Value = false;
                }



            }
        }
        // 解锁
        if(lockOnInput && playerManager.playerNetworkManager.isLockedOn.Value){
            lockOnInput = false;
            playerManager.playerNetworkManager.isLockedOn.Value = false;
            PlayerCamera.instance.ClearLockOnTargets();
            
            return;
        }
        // 锁定
        if(lockOnInput && !playerManager.playerNetworkManager.isLockedOn.Value){
            lockOnInput = false;
            PlayerCamera.instance.HandleLocatingLockOnTarget();
            if(PlayerCamera.instance.nearestLockOnTarget != null){
                playerManager.playerNetworkManager.isLockedOn.Value = true;
                playerManager.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);

            }
            
        }
    }

    private void HandleCameraMovementInput(){
        horizontalCameraInput = cameraInput.x;
        vertiCalcameralInput = cameraInput.y;
    }

    private void HandleDodgeInput(){
        if(dodgeInput){
            if(playerManager.isPerformingAction)
                return;
            // 准备翻滚或后退步
            dodgeInput = false;
            playerManager.playerLocomotionManager.AttemptToPerformDodge();
        }
    }

    private void HandleSprintInput(){
        // 同时按下shift键和空格键才是冲刺
        if(isRunning && isSprinting){
            moveAmount = 3;
            playerManager.playerLocomotionManager.HandleSprint(moveAmount);
        }      
    }

    private void HandleJumpInput(){
        // 如果按下键盘上的F键，就是准备去跳跃
        if(jumpInput){
            jumpInput = false;
            playerManager.playerLocomotionManager.AttemptToJump();
        }
        
    }
    // 轻击
    private void HandleLeftMouseInput(){
        if(leftMouseInput){
            leftMouseInput = false;

            playerManager.playerNetworkManager.SetCharacterActionHand(true);

            playerManager.playerCombatManager.performWeaponBasedAction(playerManager.playerInventoryManager.currentRightHandWeapon.ohLeftMouseAction,
                                                                        playerManager.playerInventoryManager.currentRightHandWeapon);
        }
    }
    // 重击
    private void HandleRightMouseInput(){
        if(rightMouseInput){
            rightMouseInput = false;

            playerManager.playerNetworkManager.SetCharacterActionHand(true);

            playerManager.playerCombatManager.performWeaponBasedAction(playerManager.playerInventoryManager.currentRightHandWeapon.ohRightMouseAction,
                                                                        playerManager.playerInventoryManager.currentRightHandWeapon);
        }
    }

    // 蓄力重击
    private void HandleChargeRightMouseInput(){
        if(playerManager.isPerformingAction){
            if(playerManager.playerNetworkManager.isUsingRightHand.Value){
                playerManager.playerNetworkManager.isChargingAttack.Value = holdShiftInput;
            }
        }
    }

    private void OnApplicationFocus(bool focusStatus) {
        if(enabled){
            if(focusStatus){
                playerControls.Enable();
            }else{
                playerControls.Disable();
            }
        }
    }
}
