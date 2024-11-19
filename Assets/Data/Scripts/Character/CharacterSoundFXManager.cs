using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterSoundFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    protected virtual void Awake(){
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFX(AudioClip audioClip, float volume = 1){
        audioSource.PlayOneShot(audioClip, volume);

    }
    public void PlayRollSoundFX(){
        audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
    }

    public void PlayBackstepSoundFX(){
        audioSource.PlayOneShot(WorldSoundFXManager.instance.backstepSFX);
    }
}
