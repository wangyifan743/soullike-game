using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkManager : CharacterNetworkManager
{
    PlayerManager playerManager;
    public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character",
                                                                                                    NetworkVariableReadPermission.Everyone,
                                                                                                    NetworkVariableWritePermission.Owner);
    
    public NetworkVariable<int> currentWeaponBeingUsedID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentLeftHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<bool> isUsingRightHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isUsingLeftHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
    }

    public void SetCharacterActionHand(bool rightHandedAction){
        if(rightHandedAction){
            isUsingRightHand.Value = true;
            isUsingLeftHand.Value = false;
        }
        else{
            isUsingRightHand.Value = false;
            isUsingLeftHand.Value = true;
        }
    }

    public void SetMaxHealthValue(int oldVitality, int newVitality){
        maxHealth.Value = playerManager.playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
        PlayerUIManager.instance.playerUIHUDManager.SetMaxHealthValue(maxHealth.Value);
        currentHealth.Value = maxHealth.Value;
    }

    public void SetMaxStaminaValue(int oldEndurance, int newEndurance){
        maxStamina.Value = playerManager.playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
        PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(maxStamina.Value);
        currentStamina.Value = maxStamina.Value;
    }

    public void OnCurrentRightHandWeaponIDChange(int oldID, int newID){
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        playerManager.playerInventoryManager.currentRightHandWeapon = newWeapon;
        playerManager.playerEquipmentManager.LoadWeaponOnRightHand();

    }

    public void OnCurrentleftHandWeaponIDChange(int oldID, int newID){
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        playerManager.playerInventoryManager.currentLeftHandWeapon = newWeapon;
        playerManager.playerEquipmentManager.LoadWeaponOnLeftHand();
    }

    public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID){
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        playerManager.playerCombatManager.currentWeaponBeingUsed= newWeapon;
        
    }

    [ServerRpc]
    public void NotifyTheServerOfWeaponActionServerRpc(ulong clientID, int actionID, int weaponID){
        if(IsServer){
            NotifyTheClientsOfWeaponActionClientRpc(clientID, actionID, weaponID);
        }
    }

    [ClientRpc]
    public void NotifyTheClientsOfWeaponActionClientRpc(ulong clientID, int actionID, int weaponID){
        if(NetworkManager.Singleton.LocalClientId != clientID){
            PlayWeaponAction(actionID, weaponID);
        }
    }

    private void PlayWeaponAction(int actionID, int weaponID){
        WeaponItemAction weaponItemAction = WorldActionManager.instance.GetWeaponItemActionByID(actionID);
        WeaponItem weaponItem = WorldItemDatabase.instance.GetWeaponByID(weaponID);
        if(weaponItemAction != null){
            weaponItemAction.AttemptToPerformAction(playerManager, weaponItem);
        }
        else{
            Debug.Log("没有可执行的工作");
        }

    }

}
