using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerManager : CharacterManager
{
    [Header("Debug Menu")]
    [SerializeField] bool SwitchLeftWeapon = false;
    [SerializeField] bool SwitchRightWeapon = false;

    [HideInInspector]public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector]public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector]public PlayerNetworkManager playerNetworkManager;

    [HideInInspector]public PlayerStatsManager playerStatsManager;
    [HideInInspector]public PlayerInventoryManager playerInventoryManager;
    [HideInInspector]public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector]public PlayerCombatManager playerCombatManager;
    
    protected override void Awake()
    {
        base.Awake();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
    }

    protected override void Start()
    {
        base.Start();
        if(IsOwner){
            transform.position = new Vector3(0, 0.42f , 0);
            
        }
    }

    protected override void Update()
    {
        base.Update();
        if(!IsOwner)
            return;
        playerLocomotionManager.HandleAllMovements();

        

        playerStatsManager.RegenerateStamina();

        DebugMenu();
    }

    protected override void LateUpdate()
    {
        if(!IsOwner)
            return;
        base.LateUpdate();
        PlayerCamera.instance.HandleAllCameraActions();
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if(IsOwner){
            PlayerCamera.instance.playerManager = this;
            PlayerInputManager.instance.playerManager = this;
            WorldSaveManager.instance.playerManager = this;

            // 更新最大状态
            playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetMaxHealthValue;
            playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetMaxStaminaValue;
            

            // 更新当前状态
            playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaGenerationTimerAndMaxStamina;

            playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewHealthValue;

            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentleftHandWeaponIDChange;
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsedID.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
        }
        
        playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        if(IsOwner){
            PlayerUIManager.instance.playerUIPopUpManager.SendYoudiedPopUp();
        }
        return base.ProcessDeathEvent(manuallySelectDeathAnimation);

        
    }


    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData){
        currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        currentCharacterData.vitality = playerNetworkManager.vitality.Value;
        currentCharacterData.endurance = playerNetworkManager.endurance.Value;

        
        currentCharacterData.currentHealth = playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vitality.Value);
        currentCharacterData.currentStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);

    }


    public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData){
        playerNetworkManager.characterName.Value = currentCharacterData.characterName;
        Vector3 myPosition = new Vector3(currentCharacterData.xPosition,
                                            currentCharacterData.yPosition,
                                            currentCharacterData.zPosition);


        transform.position = myPosition;

        playerNetworkManager.vitality.Value = currentCharacterData.vitality;
        playerNetworkManager.endurance.Value = currentCharacterData.endurance;

        // stamina
        playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
        playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
        PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
        PlayerUIManager.instance.playerUIHUDManager.SetCurrentStaminaValue(playerNetworkManager.currentStamina.Value);
        //health
        playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vitality.Value);
        playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
        PlayerUIManager.instance.playerUIHUDManager.SetMaxHealthValue(playerNetworkManager.maxHealth.Value);
        PlayerUIManager.instance.playerUIHUDManager.SetCurrentHealthValue(playerNetworkManager.currentHealth.Value);
    }


    private void DebugMenu(){

        if(SwitchLeftWeapon){
            SwitchLeftWeapon = false;
            playerEquipmentManager.SwitchLeftWeapon();
        }

        if(SwitchRightWeapon){
            SwitchRightWeapon = false;
            playerEquipmentManager.SwitchRightWeapon();
        }
    }

}
