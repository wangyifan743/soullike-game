using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveManager : MonoBehaviour
{
    public static WorldSaveManager instance;

    public PlayerManager playerManager;

    [Header("Save/Load")]
    [SerializeField] bool saveGame;
    [SerializeField] bool loadGame;

    [Header("World Scene Index")]
    [SerializeField] private int WorldSceneIndex = 2;

    [Header("Save Data Writer")]
    private SaveFileDataWritter saveFileDataWritter;

    [Header("Current Character Data")]
    public CharacterSlot currentCharacterSlotBeingUsed;
    public CharacterSaveData currentCharacterData;
    private string saveFileName;

    [Header("Character Slots")]
    public CharacterSaveData characterSlot01;
    public CharacterSaveData characterSlot02;
    public CharacterSaveData characterSlot03;
    public CharacterSaveData characterSlot04;
    public CharacterSaveData characterSlot05;
    public CharacterSaveData characterSlot06;
    public CharacterSaveData characterSlot07;
    public CharacterSaveData characterSlot08;
    public CharacterSaveData characterSlot09;
    public CharacterSaveData characterSlot10;


    private void Awake() {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
        LoadAllCharacterProfiles();
    }

    private void Update() {
        if(saveGame){
            saveGame = false;
            SaveGame();
        }
        else if(loadGame){
            loadGame = false;
            LoadGame();
        }
    }

    public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot){
        string fileName = "";
        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot_01:
                fileName = "characterSlot01";
                break;
            case CharacterSlot.CharacterSlot_02:
                fileName = "characterSlot02";
                break;
            case CharacterSlot.CharacterSlot_03:
                fileName = "characterSlot03";
                break;
            case CharacterSlot.CharacterSlot_04:
                fileName = "characterSlot04";
                break;
            case CharacterSlot.CharacterSlot_05:
                fileName = "characterSlot05";
                break;
            case CharacterSlot.CharacterSlot_06:
                fileName = "characterSlot06";
                break;
            case CharacterSlot.CharacterSlot_07:
                fileName = "characterSlot07";
                break;
            case CharacterSlot.CharacterSlot_08:
                fileName = "characterSlot08";
                break;
            case CharacterSlot.CharacterSlot_09:
                fileName = "characterSlot09";
                break;
            case CharacterSlot.CharacterSlot_10:
                fileName = "characterSlot10";
                break;
            default:
                break;
        }

        return fileName;
    }

    // 游戏开始时，加载所有角色基础信息
    private void LoadAllCharacterProfiles(){
        saveFileDataWritter = new SaveFileDataWritter();
        saveFileDataWritter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        characterSlot01 = saveFileDataWritter.LoadSaveFile();

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
        characterSlot02 = saveFileDataWritter.LoadSaveFile();

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
        characterSlot03 = saveFileDataWritter.LoadSaveFile();

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
        characterSlot04 = saveFileDataWritter.LoadSaveFile();

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
        characterSlot05 = saveFileDataWritter.LoadSaveFile();

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
        characterSlot06 = saveFileDataWritter.LoadSaveFile();

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
        characterSlot07 = saveFileDataWritter.LoadSaveFile();

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
        characterSlot08 = saveFileDataWritter.LoadSaveFile();

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
        characterSlot09 = saveFileDataWritter.LoadSaveFile();

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
        characterSlot10 = saveFileDataWritter.LoadSaveFile();
    }

    public void AttemptToCreateNewGame(){
        saveFileDataWritter = new SaveFileDataWritter();
        saveFileDataWritter.saveDataDirectoryPath = Application.persistentDataPath;
        

        // 检查是否能在第一个插槽创建新游戏
        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        if(!saveFileDataWritter.CheckToSeeIfFileExists()){
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        // 检查是否能在第二个插槽创建新游戏
        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
        if(!saveFileDataWritter.CheckToSeeIfFileExists()){
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }
        
        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
        if(!saveFileDataWritter.CheckToSeeIfFileExists()){
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
        if(!saveFileDataWritter.CheckToSeeIfFileExists()){
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_04;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
        if(!saveFileDataWritter.CheckToSeeIfFileExists()){
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_05;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
        if(!saveFileDataWritter.CheckToSeeIfFileExists()){
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_06;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
        if(!saveFileDataWritter.CheckToSeeIfFileExists()){
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_07;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
        if(!saveFileDataWritter.CheckToSeeIfFileExists()){
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_08;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
        if(!saveFileDataWritter.CheckToSeeIfFileExists()){
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_09;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        saveFileDataWritter.saveDataFilePath = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
        if(!saveFileDataWritter.CheckToSeeIfFileExists()){
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_10;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }
        // 如果没有多余的格子可以使用，弹窗提示
        TitleScreenManager.instance.DisplayNoFreeCharacterSlotsPopUp();

    
        
    }

    public void NewGame(){
        SaveGame();
        StartCoroutine(LoadNewScene());
    }

    public void LoadGame(){
        // 根据正在使用的插槽加载文件
        saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

        saveFileDataWritter = new SaveFileDataWritter();
        saveFileDataWritter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWritter.saveDataFilePath = saveFileName;
        currentCharacterData = saveFileDataWritter.LoadSaveFile();

        StartCoroutine(LoadNewScene());
    }

    public void SaveGame(){
        saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

        saveFileDataWritter = new SaveFileDataWritter();
        saveFileDataWritter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWritter.saveDataFilePath = saveFileName;

        playerManager.SaveGameDataToCurrentCharacterData(ref currentCharacterData); 
        saveFileDataWritter.CreateNewCharacterSaveFile(currentCharacterData);


    }

    public void DeleteGame(CharacterSlot characterSlot){
        saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

        saveFileDataWritter = new SaveFileDataWritter();
        saveFileDataWritter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWritter.saveDataFilePath = saveFileName;
        saveFileDataWritter.DeleteSaveFile();
    }


    // 异步加载新场景，使得过渡更加平滑
    public IEnumerator LoadNewScene(){
    
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(WorldSceneIndex);

        playerManager.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);

        yield return null;
    }

    public int GetWorldSceneIndex(){
        return WorldSceneIndex;
    }
}
