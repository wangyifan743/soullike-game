using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("Collider")]
    [SerializeField]protected Collider damageCollider;

    [Header("Damage")]
    public float physicalDamage = 0;
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;
    public float holyDamage = 0;

    public float poiseDamage = 0;

    public float angleHitFrom;

    [Header("Contact Point")]
    public Vector3 contactPoint;

    protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();


    protected virtual void Awake(){

    }

    public virtual void OnTriggerEnter(Collider other) {
        
    }

    protected virtual void DamageTarget(CharacterManager damageTarget){

        
        

    }


    public virtual void EnableDamageCollider(){
        damageCollider.enabled = true;
    }


    public virtual void DisableDamageCollider(){
        damageCollider.enabled = false;
        charactersDamaged.Clear();
    }
}
