using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class CharacterNetworkManager : NetworkBehaviour
{
    CharacterManager characterManager;

    [Header("position")]
    public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, 
                                                    NetworkVariableReadPermission.Everyone,
                                                     NetworkVariableWritePermission.Owner);

    public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity,
                                                        NetworkVariableReadPermission.Everyone,
                                                        NetworkVariableWritePermission.Owner);
    
    public Vector3 networkPositionVelocity;
    public float networkPositionSmoothTime = 0.1f;
    public float networkRotationSmoothTime = 0.1f;

    [Header("Animator")]

    public NetworkVariable<float> verticalMovement = new NetworkVariable<float>(default,
                                                        NetworkVariableReadPermission.Everyone,
                                                        NetworkVariableWritePermission.Owner);

    public NetworkVariable<float> horizontalMovement = new NetworkVariable<float>(default,
                                                        NetworkVariableReadPermission.Everyone,
                                                        NetworkVariableWritePermission.Owner);

    public NetworkVariable<float> moveAmount = new NetworkVariable<float>(default,
                                                        NetworkVariableReadPermission.Everyone,
                                                        NetworkVariableWritePermission.Owner); 


    [Header("Stats")]
    public NetworkVariable<int> endurance = new NetworkVariable<int>(10,
                                                        NetworkVariableReadPermission.Everyone,
                                                        NetworkVariableWritePermission.Owner); 

    public NetworkVariable<float> currentStamina = new NetworkVariable<float>(100,
                                                        NetworkVariableReadPermission.Everyone,
                                                        NetworkVariableWritePermission.Owner); 

    public NetworkVariable<float> maxStamina = new NetworkVariable<float>(100,
                                                        NetworkVariableReadPermission.Everyone,
                                                        NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> vitality = new NetworkVariable<int>(10,
                                                        NetworkVariableReadPermission.Everyone,
                                                        NetworkVariableWritePermission.Owner); 

    public NetworkVariable<float> currentHealth = new NetworkVariable<float>(150,
                                                        NetworkVariableReadPermission.Everyone,
                                                        NetworkVariableWritePermission.Owner); 

    public NetworkVariable<float> maxHealth = new NetworkVariable<float>(150,
                                                        NetworkVariableReadPermission.Everyone,
                                                        NetworkVariableWritePermission.Owner);  

     

    protected virtual void Awake(){
        characterManager = GetComponent<CharacterManager>();
    }

    [ServerRpc]
    public void NotifyTheServerOfPositionAndRotationServerRpc(ulong clientID){
        NotifyTheClientsOfPositionAndRotationClientRpc(clientID);
    }

    [ClientRpc]
    public void NotifyTheClientsOfPositionAndRotationClientRpc(ulong clientID){
        if(clientID != NetworkManager.Singleton.LocalClientId){
            UpdatePositionAndRotation();
        }
    }

    public void UpdatePositionAndRotation(){
        transform.position = Vector3.SmoothDamp(transform.position,
            networkPosition.Value,
            ref networkPositionVelocity,
            networkPositionSmoothTime);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                networkRotation.Value,
                                                networkRotationSmoothTime);
    }



    [ServerRpc]
    public void NotifyTheServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion){
        if(IsServer){
            PlayActionAnimationforAllClientsClientRpc(clientID, animationID, applyRootMotion);
        }
    }

    [ClientRpc]
    public void PlayActionAnimationforAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion){
        if(clientID != NetworkManager.Singleton.LocalClientId)
            PerformActionAnimationFromServer(animationID, applyRootMotion);
    }

    private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion){
        characterManager.animator.applyRootMotion = applyRootMotion;
        characterManager.animator.CrossFade(animationID, 0.2f);
    }

    [ServerRpc]
    public void NotifyTheServerOfAttackActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion){
        if(IsServer){
            PlayAttackActionAnimationforAllClientsClientRpc(clientID, animationID, applyRootMotion);
        }
    }

    [ClientRpc]
    public void PlayAttackActionAnimationforAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion){
        if(clientID != NetworkManager.Singleton.LocalClientId)
            PerformAttackActionAnimationFromServer(animationID, applyRootMotion);
    }

    private void PerformAttackActionAnimationFromServer(string animationID, bool applyRootMotion){
        characterManager.animator.applyRootMotion = applyRootMotion;
        characterManager.animator.CrossFade(animationID, 0.2f);
    }


    [ServerRpc(RequireOwnership = false)]
    public void NotifyTheServerOfCharacterDamageServerRpc(
        ulong damagedCharacterID,
        ulong charaterCausingDamageID,
        float physicalDamage,
        float fireDamage,
        float lightningDamage,
        float holyDamage,
        float magicDamage,
        float poiseDamage,
        float angleHitFrom,
        float contactPointX,
        float contactPointY,
        float contactPointZ
    ){
        if(IsServer){
            NotifyTheClientsOfCharacterDamageClientRpc(damagedCharacterID, charaterCausingDamageID, physicalDamage, 
            fireDamage, lightningDamage, holyDamage, magicDamage, poiseDamage, angleHitFrom, contactPointX, contactPointY, contactPointZ);
        }
    }

    [ClientRpc]
    public void NotifyTheClientsOfCharacterDamageClientRpc(
        ulong damagedCharacterID,
        ulong charaterCausingDamageID,
        float physicalDamage,
        float fireDamage,
        float lightningDamage,
        float holyDamage,
        float magicDamage,
        float poiseDamage,
        float angleHitFrom,
        float contactPointX,
        float contactPointY,
        float contactPointZ
    ){
        ProcessCharacterDamageFromServer(damagedCharacterID, charaterCausingDamageID, physicalDamage, 
            fireDamage, lightningDamage, holyDamage, magicDamage, poiseDamage, angleHitFrom, contactPointX, contactPointY, contactPointZ);
    }

    public void ProcessCharacterDamageFromServer(
        ulong damagedCharacterID,
        ulong charaterCausingDamageID,
        float physicalDamage,
        float fireDamage,
        float lightningDamage,
        float holyDamage,
        float magicDamage,
        float poiseDamage,
        float angleHitFrom,
        float contactPointX,
        float contactPointY,
        float contactPointZ
    ){
        CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.GetComponent<CharacterManager>();
        CharacterManager characterCausingDamage =  NetworkManager.Singleton.SpawnManager.SpawnedObjects[charaterCausingDamageID].gameObject.GetComponent<CharacterManager>();

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.lightningDamage = lightningDamage;
        damageEffect.poiseDamage = poiseDamage;
        damageEffect.angleHitFrom = angleHitFrom;
        damageEffect.contactPoint = new Vector3(contactPointX, contactPointY, contactPointZ);
        damageEffect.characterCausingDamage = characterCausingDamage;

        damagedCharacter.characterEffectsManager.ProcessInstantEffects(damageEffect);
    }

    public void CheckHP(float oldValue, float newValue){
       
        if(currentHealth.Value <=0 ){
  
            StartCoroutine(characterManager.ProcessDeathEvent());
        }

        // 过量治疗后，将生命值置为最大生命
        if(characterManager.IsOwner){
            if(currentHealth.Value > maxHealth.Value){
                currentHealth.Value = maxHealth.Value;
            }
        }
    }

}
