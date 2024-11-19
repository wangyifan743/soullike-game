using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectsManager : MonoBehaviour
{
    public static WorldCharacterEffectsManager instance;
    [Header("Damage")]
    public TakeDamageEffect takeDamageEffect;

    [Header("VFX")]
    public GameObject bloodSplatterVFX;

    [SerializeField] List<InstantCharacterEffect> instantEffects;
    private void Awake() {
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    private void GenerateEffectsID(){
        for (int i = 0; i < instantEffects.Count; i++)
        {
            instantEffects[i].instantEffectID = i;
        }
    }
}
