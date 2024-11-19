using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterManager characterManager;

    [Header("VFX")]
    [SerializeField] GameObject bloodSplatterVFX;

    protected virtual void Awake(){
        characterManager = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffects(InstantCharacterEffect effect){
        effect.ProcessEffect(characterManager);
    }

    public void PlayBloodSplatterVFX(Vector3 contactPoint){
        if(bloodSplatterVFX != null){
            GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
        else{
            GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
    }
}
