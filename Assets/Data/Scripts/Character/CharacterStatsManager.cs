using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager characterManager;

    [Header("Stamina Regeneration")]
    [SerializeField] float staminaRegenerationAmount = 2;
    private float staminaGenerationTimer;
    private float staminaTickTimer;
    [SerializeField] float staminaRegenerationDelay = 2;
    protected virtual void Awake(){
        characterManager = GetComponent<CharacterManager>();
    }

    protected virtual void Start(){
        
    }


    public virtual float CalculateStaminaBasedOnEnduranceLevel(int endurance){
        float stamina = 0;

        stamina = endurance * 10.0f;

        return stamina;
    }

    public virtual float CalculateHealthBasedOnVitalityLevel(int vatality){
        float health = 0;

        health = vatality * 15.0f;

        return health;
    }

    public virtual void RegenerateStamina(){
        if(!characterManager.IsOwner)
            return;
        if(characterManager.isPerformingAction)
            return;
        
        staminaGenerationTimer += Time.deltaTime;
        if(staminaGenerationTimer >= staminaRegenerationDelay){
            if(characterManager.characterNetworkManager.currentStamina.Value <= characterManager.characterNetworkManager.maxStamina.Value){
                staminaTickTimer += Time.deltaTime;
                if(staminaTickTimer > 0.2){
                    staminaTickTimer = 0;
                    characterManager.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                } 
            }
        }
    }

    public virtual void ResetStaminaGenerationTimerAndMaxStamina(float oldValue, float newValue){
        if(newValue < oldValue)
            staminaGenerationTimer = 0;

        if(newValue>=characterManager.characterNetworkManager.maxStamina.Value)
            characterManager.characterNetworkManager.currentStamina.Value = characterManager.characterNetworkManager.maxStamina.Value;
    }


}
