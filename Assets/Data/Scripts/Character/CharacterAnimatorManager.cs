using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager characterManager;
    [SerializeField]float clappedHorizontalInput;
    [SerializeField]float clappedVerticalInput;

    protected virtual void Awake(){
        characterManager = GetComponent<CharacterManager>();
    }

    public void UpdateAnimatorMovementParameters(float horizontalInput, float verticalInput){
        
        if(characterManager.animator != null){
            characterManager.animator.SetFloat("Horizontal", horizontalInput, 0.1f, Time.deltaTime);
            characterManager.animator.SetFloat("Vertical", verticalInput, 0.1f, Time.deltaTime);
        }
    }

    public void PlayTargetActionAnimation(string targetAnimation,
                                            bool isPerformingAction,
                                            bool apllyRootmotion = true,
                                            bool canMove = false,
                                            bool canRotate = false){
        characterManager.animator.applyRootMotion = apllyRootmotion;
        characterManager.animator.CrossFade(targetAnimation, 0.2f);
        characterManager.isPerformingAction = isPerformingAction;
        characterManager.canMove = canMove;
        characterManager.canRotate = canRotate;

        characterManager.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId,
                                                                                    targetAnimation, 
                                                                                    apllyRootmotion);
    }


    public void PlayTargetAttackActionAnimation(AttackType attackType,
                                            string targetAnimation,
                                            bool isPerformingAction,
                                            bool apllyRootmotion = true,
                                            bool canMove = false,
                                            bool canRotate = false){
        characterManager.characterCombatManager.currentAttackType = attackType;
        characterManager.animator.applyRootMotion = apllyRootmotion;
        characterManager.animator.CrossFade(targetAnimation, 0.2f);
        characterManager.isPerformingAction = isPerformingAction;
        characterManager.canMove = canMove;
        characterManager.canRotate = canRotate;

        characterManager.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId,
                                                                                    targetAnimation, 
                                                                                    apllyRootmotion);
    }

}
