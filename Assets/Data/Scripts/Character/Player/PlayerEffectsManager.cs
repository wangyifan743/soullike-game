using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    [Header("For Debug")]
    [SerializeField]InstantCharacterEffect effectToTest;
    [SerializeField]bool processEffect;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update() {
        if(processEffect){
            processEffect = false;
            TakeStaminaDamageEffect effect = Instantiate(effectToTest) as TakeStaminaDamageEffect;
            ProcessInstantEffects(effectToTest);
        }
    }


}
