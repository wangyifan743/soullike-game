using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager instance;

    [Header("Action Sounds")]
    public AudioClip rollSFX;
    public AudioClip backstepSFX;

    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
}
