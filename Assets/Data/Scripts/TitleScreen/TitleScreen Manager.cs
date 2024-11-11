using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance;

    [Header("Menus")]
    [SerializeField] GameObject titleScreenMainMenu;
    [SerializeField] GameObject titleScreenLoadMenu;

    [Header("Buttons")]
    [SerializeField] Button mainMenuReturnButton;
    [SerializeField] Button loadMenuButton;
    [SerializeField] Button deleteCharacterSlotConfirmButton;

    [Header("Pop Ups")]
    [SerializeField] GameObject noCharacterSlotsPopUp;
    [SerializeField] Button noCharacterSlotsOkayButton;
    [SerializeField] Button mainMenuNewGameButton;
    [SerializeField] GameObject deleteCharacterSlotPopUp;

    [Header("Character Slots")]
    public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

    [Header("Title Screen Inputs")]
    [SerializeField] bool deleteCharacterSlot = false;

    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame(){
        WorldSaveManager.instance.AttemptToCreateNewGame();

    }

    public void OpenLoadMenu(){
        titleScreenMainMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);
        mainMenuReturnButton.Select();
    }

    public void CloseLoadMenu(){
        
        titleScreenMainMenu.SetActive(true);
        titleScreenLoadMenu.SetActive(false);
        loadMenuButton.Select();
    }

    public void DisplayNoFreeCharacterSlotsPopUp(){
        noCharacterSlotsPopUp.SetActive(true);
        noCharacterSlotsOkayButton.Select();
    }

    public void CloseNoFreeCharacterSlotsPopUp(){
        noCharacterSlotsPopUp.SetActive(false);
        mainMenuNewGameButton.Select();
    }

    public void SelectCharacterSlot(CharacterSlot characterSlot){
        currentSelectedSlot = characterSlot;
    }

    public void SelectNoSlot(){
        currentSelectedSlot = CharacterSlot.NO_SLOT;
    }

    public void AttemptToDeleteCharacterSlot(){
        if(currentSelectedSlot != CharacterSlot.NO_SLOT){
            deleteCharacterSlotPopUp.SetActive(true);
            deleteCharacterSlotConfirmButton.Select();
        }
    }

    public void DeletecharacterSlot(){
        deleteCharacterSlotPopUp.SetActive(false);

        WorldSaveManager.instance.DeleteGame(currentSelectedSlot);

        // 刷新
        titleScreenLoadMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);

        mainMenuReturnButton.Select();
    }

    public void CloseDeleteCharacterPopUp(){
        deleteCharacterSlotPopUp.SetActive(false);
        mainMenuReturnButton.Select();
    }
}
