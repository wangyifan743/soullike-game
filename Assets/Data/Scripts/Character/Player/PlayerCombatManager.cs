using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager playerManager;
    public WeaponItem currentWeaponBeingUsed;

    [Header("Flags")]
    public bool canComboWithMainHandWeapon = false;

    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
    }

    public void performWeaponBasedAction(WeaponItemAction weaponItemAction, WeaponItem weaponItem){
        if(playerManager.IsOwner){
            weaponItemAction.AttemptToPerformAction(playerManager, weaponItem);

            playerManager.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId,
                                                                                        weaponItemAction.actionID,
                                                                                        weaponItem.ItemID);
        }
    }

    public void DrainStaminaBasedOnAttack(){
        if(!playerManager.IsOwner)
            return;
        
        if(currentWeaponBeingUsed == null)
            return;

        float staminaDeducted = 0;

        // 根据伤害类型扣除体力
        switch (currentAttackType)
        {
            case AttackType.LightAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;
            
            default:
                break;
        }
        playerManager.playerNetworkManager.currentStamina.Value -= staminaDeducted;
    }

    
}
