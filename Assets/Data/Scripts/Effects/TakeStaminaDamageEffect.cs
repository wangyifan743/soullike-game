using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/ Instant Effects / Take Stamina Damage")]
public class TakeStaminaDamageEffect : InstantCharacterEffect
{
    
    public float staminaDamage;

    public override void ProcessEffect(CharacterManager characterManager)
    {
        base.ProcessEffect(characterManager);
        CalculateStaminaDamage(characterManager);
    }

    public void CalculateStaminaDamage(CharacterManager characterManager){
        if(characterManager.IsOwner){
            characterManager.characterNetworkManager.currentStamina.Value -= staminaDamage;
        }
    }
}
