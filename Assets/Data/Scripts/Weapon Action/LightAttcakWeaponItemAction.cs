using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/ Weapon Actions/ Light Attack Action")]
public class LightAttcakWeaponItemAction : WeaponItemAction
{
    [SerializeField] string light_Attack_01 = "Main_Light_Attack_01";
    public override void AttemptToPerformAction(PlayerManager playerManager, WeaponItem weaponItem)
    {
        base.AttemptToPerformAction(playerManager, weaponItem);

        if(!playerManager.IsOwner)
            return;

        if(playerManager.playerNetworkManager.currentStamina.Value <= 0)
            return;
        
        if(!playerManager.isGrounded)
            return;

        PerformLightAttack(playerManager, weaponItem);
    }

    private void PerformLightAttack(PlayerManager playerManager, WeaponItem weaponItem){
        if(playerManager.playerNetworkManager.isUsingRightHand.Value){
            playerManager.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);
        }
        if(playerManager.playerNetworkManager.isUsingLeftHand.Value){

        }
    }
}
