using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    PlayerManager playerManager;
    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
    }
    public override void EnableCanDoCombo(){
        if(playerManager.playerNetworkManager.isUsingRightHand.Value){
            playerManager.playerCombatManager.canComboWithMainHandWeapon = true;
        }
        else{

        }
    }

    public override void DisableCanDoCombo(){
        playerManager.playerCombatManager.canComboWithMainHandWeapon = false;
    }
}
