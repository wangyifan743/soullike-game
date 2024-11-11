using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Character_Save_Slot : MonoBehaviour
{
    SaveFileDataWritter saveFileDataWritter;

    [Header("Game Slot")]
    public CharacterSlot characterSlot;

    [Header("Character Info")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI timePlayed;


    private void OnEnable() {
        LoadSaveSlots();
    }

    private void LoadSaveSlots(){
        saveFileDataWritter = new SaveFileDataWritter();
        saveFileDataWritter.saveDataDirectoryPath = Application.persistentDataPath;

        if(characterSlot == CharacterSlot.CharacterSlot_01){
            saveFileDataWritter.saveDataFilePath = WorldSaveManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            if(saveFileDataWritter.CheckToSeeIfFileExists()){
                characterName.text = WorldSaveManager.instance.characterSlot01.characterName;
            }
            else{
                gameObject.SetActive(false);
            }
        }else if(characterSlot == CharacterSlot.CharacterSlot_02){
            saveFileDataWritter.saveDataFilePath = WorldSaveManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            if(saveFileDataWritter.CheckToSeeIfFileExists()){
                characterName.text = WorldSaveManager.instance.characterSlot02.characterName;
            }
            else{
                gameObject.SetActive(false);
            }
        }else if(characterSlot == CharacterSlot.CharacterSlot_03){
            saveFileDataWritter.saveDataFilePath = WorldSaveManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            if(saveFileDataWritter.CheckToSeeIfFileExists()){
                characterName.text = WorldSaveManager.instance.characterSlot03.characterName;
            }
            else{
                gameObject.SetActive(false);
            }
        }else if(characterSlot == CharacterSlot.CharacterSlot_04){
            saveFileDataWritter.saveDataFilePath = WorldSaveManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            if(saveFileDataWritter.CheckToSeeIfFileExists()){
                characterName.text = WorldSaveManager.instance.characterSlot04.characterName;
            }
            else{
                gameObject.SetActive(false);
            }
        }else if(characterSlot == CharacterSlot.CharacterSlot_05){
            saveFileDataWritter.saveDataFilePath = WorldSaveManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            if(saveFileDataWritter.CheckToSeeIfFileExists()){
                characterName.text = WorldSaveManager.instance.characterSlot05.characterName;
            }
            else{
                gameObject.SetActive(false);
            }
        }else if(characterSlot == CharacterSlot.CharacterSlot_06){
            saveFileDataWritter.saveDataFilePath = WorldSaveManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            if(saveFileDataWritter.CheckToSeeIfFileExists()){
                characterName.text = WorldSaveManager.instance.characterSlot06.characterName;
            }
            else{
                gameObject.SetActive(false);
            }
        }else if(characterSlot == CharacterSlot.CharacterSlot_07){
            saveFileDataWritter.saveDataFilePath = WorldSaveManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            if(saveFileDataWritter.CheckToSeeIfFileExists()){
                characterName.text = WorldSaveManager.instance.characterSlot07.characterName;
            }
            else{
                gameObject.SetActive(false);
            }
        }else if(characterSlot == CharacterSlot.CharacterSlot_08){
            saveFileDataWritter.saveDataFilePath = WorldSaveManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            if(saveFileDataWritter.CheckToSeeIfFileExists()){
                characterName.text = WorldSaveManager.instance.characterSlot08.characterName;
            }
            else{
                gameObject.SetActive(false);
            }
        }else if(characterSlot == CharacterSlot.CharacterSlot_09){
            saveFileDataWritter.saveDataFilePath = WorldSaveManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            if(saveFileDataWritter.CheckToSeeIfFileExists()){
                characterName.text = WorldSaveManager.instance.characterSlot09.characterName;
            }
            else{
                gameObject.SetActive(false);
            }
        }else if(characterSlot == CharacterSlot.CharacterSlot_10){
            saveFileDataWritter.saveDataFilePath = WorldSaveManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            if(saveFileDataWritter.CheckToSeeIfFileExists()){
                characterName.text = WorldSaveManager.instance.characterSlot10.characterName;
            }
            else{
                gameObject.SetActive(false);
            }
        }


    }


    public void LoadGameFromCharacterSlot(){
        
        WorldSaveManager.instance.currentCharacterSlotBeingUsed = characterSlot;
        WorldSaveManager.instance.LoadGame();
    }

    public void SelectCurrentSlot(){
        TitleScreenManager.instance.SelectCharacterSlot(characterSlot);
    }
}
