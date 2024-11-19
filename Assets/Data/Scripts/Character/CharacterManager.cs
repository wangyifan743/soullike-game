using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
public class CharacterManager : NetworkBehaviour
{
    [Header("Stats")]
    public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [HideInInspector]public CharacterController characterController;
    [HideInInspector]public Animator animator;
    [HideInInspector]public CharacterNetworkManager characterNetworkManager;

    [HideInInspector]public CharacterEffectsManager characterEffectsManager;

    [HideInInspector]public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector]public CharacterCombatManager characterCombatManager;
    [HideInInspector]public CharacterSoundFXManager characterSoundFXManager;

    [Header("Flags")]
    [SerializeField] public bool isPerformingAction;
    [SerializeField] public bool canMove;
    [SerializeField] public bool canRotate;

    [SerializeField] public bool isJumping = false;

    [SerializeField] public bool isGrounded = true;


    protected virtual void Awake(){
        DontDestroyOnLoad(gameObject);
        characterController = GetComponent<CharacterController>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
        animator = GetComponent<Animator>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>();
        characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
    }

    protected virtual void Start(){
        IgnoreMyOwnColliders();
    }
    

    protected virtual void  Update() {
        if(IsOwner){
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
            
            characterNetworkManager.NotifyTheServerOfPositionAndRotationServerRpc(NetworkManager.Singleton.LocalClientId);
        }
        
    }

    public void UpdateValues(){
        
    }

    protected virtual void LateUpdate() {
        
    }


    public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false){
        if(IsOwner){
            characterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;
        }

        if(!manuallySelectDeathAnimation){
            characterAnimatorManager.PlayTargetActionAnimation("Dead_01",true,true,false,false);
        }

        yield return new WaitForSeconds(5);

        // 播放音效等
    }

    // 角色自己身上的碰撞器不要互相触发
    protected virtual void IgnoreMyOwnColliders(){
        Collider characterControllerCollider = GetComponent<Collider>();
        Collider[] damageableCharacterCollider = GetComponentsInChildren<Collider>();
        List<Collider> ignoreColliders = new List<Collider>();

        foreach (var collider in damageableCharacterCollider)
        {
            ignoreColliders.Add(collider);
        }
        ignoreColliders.Add(characterControllerCollider);

        foreach (var collider in ignoreColliders)
        {
            foreach (var otherCollider in ignoreColliders)
            {
                Physics.IgnoreCollision(collider, otherCollider, true);
            }
        }

    }
}
