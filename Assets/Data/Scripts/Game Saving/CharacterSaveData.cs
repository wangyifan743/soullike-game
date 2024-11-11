using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// 要存储的角色相关的数据
public class CharacterSaveData 
{
    [Header("Character Name")]
    public string characterName;

    [Header("Time Played")]
    public float secondsPlayed;

    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;

    [Header("Resources")]
    public float currentHealth;
    public float currentStamina;

    [Header("Stats")]
    public int vitality;
    public int endurance;
}
