using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/ Instant Effects / Take Damage")]
public class TakeDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage; 

    [Header("Damage")]
    public float physicalDamage; // 物理伤害
    public float magicDamage;
    public float fireDamage;
    public float lightningDamage;
    public float holyDamage;

    [Header("Final Damage")]
    private float finalDamageDealt = 0;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageSFX = true;
    public AudioClip elementDamageSoundFX;

    [Header("Poise")]
    public float poiseDamage;
    public bool poiseIsBroken = false;

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;
    public Vector3 contactPoint;

    public override void ProcessEffect(CharacterManager characterManager)
    {
        base.ProcessEffect(characterManager);

        Debug.Log("处理伤害");

        if(characterManager.isDead.Value)
            return;

        // 计算伤害
        CalculateDamage(characterManager);
        // 检查伤害来自哪个方向
        PlayDirectionalBasedDamageAnimation(characterManager);
        // 播放伤害动画
        PlayDamageVFX(characterManager);
        // 检查持续伤害
        // 播放伤害声效
        PlayDamageSFX(characterManager);

    }

    private void CalculateDamage(CharacterManager characterManager){
        if(!characterManager.IsOwner)
            return;

        if(characterCausingDamage != null){

        }

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

        if(finalDamageDealt < 0){
            finalDamageDealt = -1;
        }

        characterManager.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
    }

    private void PlayDamageVFX(CharacterManager character){
        character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
    }

    private void PlayDamageSFX(CharacterManager character){
        AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);
        character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
    }

    private void PlayDirectionalBasedDamageAnimation(CharacterManager character){
        if(!character.IsOwner) return;
        if(character.isDead.Value) return;
        poiseIsBroken = true;
        if(angleHitFrom >= 145 && angleHitFrom <=180){
            damageAnimation = character.characterAnimatorManager.hit_Forward_Medium_01;
        }
        else if(angleHitFrom <= -145 && angleHitFrom >= -180){
            damageAnimation = character.characterAnimatorManager.hit_Forward_Medium_01;
        }
        else if(angleHitFrom >= -45 && angleHitFrom <= 45){
            damageAnimation = character.characterAnimatorManager.hit_Backward_Medium_01;
        }
        else if(angleHitFrom >= -144 && angleHitFrom <= -45){
            damageAnimation = character.characterAnimatorManager.hit_Left_Medium_01;
        }
        else if(angleHitFrom >= 45 && angleHitFrom <= 144){
            damageAnimation = character.characterAnimatorManager.hit_Right_Medium_01;
        }

        if(poiseIsBroken){
            character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
        }
    }

}
