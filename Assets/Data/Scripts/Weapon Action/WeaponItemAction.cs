using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/ Weapon Actions/ Test Action")]
public class WeaponItemAction : ScriptableObject
{
    public int actionID;

    public virtual void AttemptToPerformAction(PlayerManager playerManager, WeaponItem weaponItem){
        if(playerManager.IsOwner){
            playerManager.playerNetworkManager.currentWeaponBeingUsedID.Value = weaponItem.ItemID;
        }

    }
}
