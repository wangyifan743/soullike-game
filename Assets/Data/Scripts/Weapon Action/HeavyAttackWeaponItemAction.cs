using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/ Weapon Actions/ Heavy Attack Action")]
public class HeavyAttackWeaponItemAction : WeaponItemAction
{
    [SerializeField] string heavy_Attack_01 = "Main_Heavy_Attack_01";
    [SerializeField] string heavy_Attack_02 = "Main_Heavy_Attack_02";
    public override void AttemptToPerformAction(PlayerManager playerManager, WeaponItem weaponItem)
    {
        base.AttemptToPerformAction(playerManager, weaponItem);

        if(!playerManager.IsOwner)
            return;

        if(playerManager.playerNetworkManager.currentStamina.Value <= 0)
            return;
        
        if(!playerManager.isGrounded)
            return;

        PerformHeavyAttack(playerManager, weaponItem);
    }

    private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction){
        if(playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction){
            playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

            if(playerPerformingAction.playerCombatManager.lastAttackAnimationPerformed == heavy_Attack_01){
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack02, heavy_Attack_02, true);
            }
            else{
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack01, heavy_Attack_01, true);
            }
        }
        else if(!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack01, heavy_Attack_01, true);
        }
    }
}
