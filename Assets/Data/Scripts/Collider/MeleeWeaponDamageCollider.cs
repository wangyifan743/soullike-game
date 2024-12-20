using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("Attacking Character")]
    public CharacterManager characterCausingDamage;

    [Header("Weapon_Attack_Modifiers")]
    public float light_Attack_01_Modifier;
    public float light_Attack_02_Modifier;
    public float heavy_Attack_01_Modifier;
    public float heavy_Attack_02_Modifier;
    public float charge_Attack_01_Modifier;

    public float charge_Attack_02_Modifier;


    protected override void Awake()
    {
        base.Awake();
       
    }

    public override void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if(damageTarget!=null){
            if(damageTarget == characterCausingDamage)
                return;

            contactPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);


            DamageTarget(damageTarget);
        }


    }

    protected override void DamageTarget(CharacterManager damageTarget)
    {
        if(charactersDamaged.Contains(damageTarget))
            return;
        
        charactersDamaged.Add(damageTarget);

        TakeDamageEffect takeDamageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        takeDamageEffect.physicalDamage = physicalDamage;
        takeDamageEffect.magicDamage = magicDamage;
        takeDamageEffect.fireDamage = fireDamage;
        takeDamageEffect.lightningDamage = lightningDamage;
        takeDamageEffect.holyDamage = holyDamage;
        takeDamageEffect.poiseDamage = poiseDamage;
        takeDamageEffect.contactPoint = contactPoint;
        takeDamageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

        switch (characterCausingDamage.characterCombatManager.currentAttackType)
        {
            case AttackType.LightAttack01:
                ApplyAttackDamageModifier(light_Attack_01_Modifier, takeDamageEffect);
                break;
            case AttackType.LightAttack02:
                ApplyAttackDamageModifier(light_Attack_02_Modifier, takeDamageEffect);
                break;
            case AttackType.HeavyAttack01:
                ApplyAttackDamageModifier(heavy_Attack_01_Modifier, takeDamageEffect);
                break;
            case AttackType.HeavyAttack02:
                ApplyAttackDamageModifier(heavy_Attack_02_Modifier, takeDamageEffect);
                break;
            case AttackType.ChargeAttack01:
                ApplyAttackDamageModifier(charge_Attack_01_Modifier, takeDamageEffect);
                break;
            case AttackType.ChargeAttack02:
                ApplyAttackDamageModifier(charge_Attack_02_Modifier, takeDamageEffect);
                break;
            default:
                break;
        }

        if(characterCausingDamage.IsOwner){
            characterCausingDamage.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                damageTarget.NetworkObjectId,
                characterCausingDamage.NetworkObjectId,
                physicalDamage,
                fireDamage,
                lightningDamage,
                holyDamage,
                magicDamage,
                poiseDamage,
                angleHitFrom,
                contactPoint.x,
                contactPoint.y,
                contactPoint.z
            );
        }

        //damageTarget.characterEffectsManager.ProcessInstantEffects(takeDamageEffect);
    }

    private void ApplyAttackDamageModifier(float modifier, TakeDamageEffect damage){
        damage.physicalDamage *= modifier;
        damage.magicDamage *= modifier;
        damage.fireDamage *= modifier;
        damage.lightningDamage *= modifier;
        damage.holyDamage *= modifier;
        damage.poiseDamage *= modifier;

    }

}
