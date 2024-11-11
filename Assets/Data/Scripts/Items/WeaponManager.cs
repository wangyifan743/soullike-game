using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public MeleeWeaponDamageCollider meleeDamageCollider;


    private void Awake() {
        meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();

    }

    public void SetWeaponDamage(CharacterManager characterWieldingWeapon , WeaponItem weapon){
        meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
        meleeDamageCollider.physicalDamage = weapon.physicalDamage;
        meleeDamageCollider.magicDamage = weapon.magicDamage;
        meleeDamageCollider.fireDamage = weapon.fireDamage;
        meleeDamageCollider.holyDamage = weapon.holyDamage;
        meleeDamageCollider.lightningDamage = weapon.lightningDamage;
        meleeDamageCollider.light_Attack_01_Modifier = weapon.light_Attack_01_Modifier;
    }
}