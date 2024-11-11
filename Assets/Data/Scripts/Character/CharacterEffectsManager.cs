using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterManager characterManager;

    protected virtual void Awake(){
        characterManager = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffects(InstantCharacterEffect effect){
        effect.ProcessEffect(characterManager);
    }
}
