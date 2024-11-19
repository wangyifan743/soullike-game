using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/ Weapon Actions/ Light Attack Action")]
public class LightAttcakWeaponItemAction : WeaponItemAction
{
    [SerializeField] string light_Attack_01 = "Main_Light_Attack_01";
    [SerializeField] string light_Attack_02 = "Main_Light_Attack_02";
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

    private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction){

        if(playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction){
            playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

            if(playerPerformingAction.playerCombatManager.lastAttackAnimationPerformed == light_Attack_01){
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack02, light_Attack_02, true);
            }
            else{
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);
            }
        }
        else if(!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);
        }
        
    }
}
