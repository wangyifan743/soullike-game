using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager playerManager;
    public WeaponItem currentWeaponBeingUsed;

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
