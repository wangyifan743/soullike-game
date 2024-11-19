using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    CharacterManager characterManager;
    [Header("Attack Target")]
    public CharacterManager currentAttackTarget;

    [Header("Attack Type")]
    public AttackType currentAttackType;

    [Header("Lock On Transform")]
    public Transform lockOnTransform;

    [Header("Last Attck Animation Performed")]
    public string lastAttackAnimationPerformed;

    

    protected virtual void Awake(){
        characterManager = GetComponent<CharacterManager>();
    }

    public virtual void SetTarget(CharacterManager newTarget){
        if(characterManager.IsOwner){
            if(newTarget!=null){
                currentAttackTarget = newTarget;
                characterManager.characterNetworkManager.currentTargetNetworkObjectID.Value = currentAttackTarget.GetComponent<NetworkObject>().NetworkObjectId;
            }
            else{
                currentAttackTarget = null;
            }
        }
    }
}
