using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;

    public PlayerManager player;

    [Header("SAVE/LOAD")]
    [SerializeField] bool saveGame;
    [SerializeField] bool loadGame;

    [Header("World Scene Index")]
    [SerializeField] int worldSceneIndex = 1;

    [Header("Save Data Writer")]
    private SaveFileDataWrite saveFileDataWriter;


    [Header("Current Character Data")]
    public CharacterSlot currentCharacterSlotInUse;
    public CharacterSaveData currentCharacterData;
    private string saveFileName; 

    [Header("CharacterSlots")]
    public  CharacterSaveData characterSlot01;
    public  CharacterSaveData characterSlot02;
    public  CharacterSaveData characterSlot03;
    public  CharacterSaveData characterSlot04;
    public  CharacterSaveData characterSlot05;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        LoadAllCharacterProfiles();
    }
    
    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();
        }

        if (loadGame)
        {
            loadGame = false;
            LoadGame();
        }
    }

    public string CharacterFileNameDescriptor(CharacterSlot characterSlot)
    {
        string fileName = "";
        
        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot_01:
                fileName = "CharacterSlot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                fileName = "CharacterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                fileName = "CharacterSlot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                fileName = "CharacterSlot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                fileName = "CharacterSlot_05";
                break;
            default:
                break;
        }

        return fileName;
    }

    public void AttemptToCreateNewGame()
    {
        saveFileDataWriter = new SaveFileDataWrite();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        //CHECK IF CAN CREATE NEW SAVE FILE(CHECK EXISTING SAVE FILES FIRST)
        saveFileDataWriter.saveFileName = CharacterFileNameDescriptor(CharacterSlot.CharacterSlot_01);

        //IF PROFILE SLOT NOT TAKEN< THEN USE
        if (!saveFileDataWriter.CheckFileExistence())
        {
            currentCharacterSlotInUse = CharacterSlot.CharacterSlot_01;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        saveFileDataWriter.saveFileName = CharacterFileNameDescriptor(CharacterSlot.CharacterSlot_02);

        if (!saveFileDataWriter.CheckFileExistence())
        {
            currentCharacterSlotInUse = CharacterSlot.CharacterSlot_02;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        // saveFileDataWriter.saveFileName = CharacterFileNameDescriptor(CharacterSlot.CharacterSlot_03);

        // if (!saveFileDataWriter.CheckFileExistence())
        // {
        //     currentCharacterSlotInUse = CharacterSlot.CharacterSlot_03;
        //     currentCharacterData = new CharacterSaveData();
        //     StartCoroutine(LoadWorldScene());
        //     return;
        // }

        // saveFileDataWriter.saveFileName = CharacterFileNameDescriptor(CharacterSlot.CharacterSlot_04);

        // if (!saveFileDataWriter.CheckFileExistence())
        // {
        //     currentCharacterSlotInUse = CharacterSlot.CharacterSlot_04;
        //     currentCharacterData = new CharacterSaveData();
        //     StartCoroutine(LoadWorldScene());
        //     return;
        // }

        // saveFileDataWriter.saveFileName = CharacterFileNameDescriptor(CharacterSlot.CharacterSlot_05);

        // if (!saveFileDataWriter.CheckFileExistence())
        // {
        //     currentCharacterSlotInUse = CharacterSlot.CharacterSlot_05;
        //     currentCharacterData = new CharacterSaveData();
        //     StartCoroutine(LoadWorldScene());
        //     return;
        // }

        //IF NO FREE SLOTS, NOTIFY PLAYER
        TitleScreenManager.instance.DisplayNoFreeSlotsPopUp();
    }

     public void LoadGame()
    {
        //LOAD PREVIOUS FILE< WITH FILE NAME DEPENDING ON SLOT
        saveFileName = CharacterFileNameDescriptor(currentCharacterSlotInUse);

        saveFileDataWriter = new SaveFileDataWrite();
        //MACHINE AGNOSTIC FILE PATH
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;
        currentCharacterData = saveFileDataWriter.LoadSaveFile();

        StartCoroutine(LoadWorldScene());
    }

    public void SaveGame()
    {
        //SAVE CURRENT FILE UNDER FILE NAME DEPENDING ON CHARACTER SLOT
        saveFileName = CharacterFileNameDescriptor(currentCharacterSlotInUse);

        saveFileDataWriter = new SaveFileDataWrite();
        //MACHINE AGNOSTIC FILE PATH
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;

        //PASS PLAYER INFO FROM GAME AT CURRENT TIME TO SAVE FILE
        player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

        //WRITE THAT INFO ONTO JSON FILE, SAVE TO MACHINE
        saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);

    }

    // LOAD ALL CHARACTER PROFILES ON DEVICE WHEN STARTING GAME
    private void LoadAllCharacterProfiles()
    {
        saveFileDataWriter = new SaveFileDataWrite();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = CharacterFileNameDescriptor(CharacterSlot.CharacterSlot_01);
        characterSlot01 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = CharacterFileNameDescriptor(CharacterSlot.CharacterSlot_02);
        characterSlot02 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = CharacterFileNameDescriptor(CharacterSlot.CharacterSlot_03);
        characterSlot03 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = CharacterFileNameDescriptor(CharacterSlot.CharacterSlot_04);
        characterSlot04 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = CharacterFileNameDescriptor(CharacterSlot.CharacterSlot_05);
        characterSlot05 = saveFileDataWriter.LoadSaveFile();
    }

    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

        if (loadOperation == null)
        {
            Debug.LogError($"Could not start loading scene with build index {worldSceneIndex}.");
            yield break;
        }

        yield return new WaitUntil(() => loadOperation.isDone);

        if (player == null)
        {
            Debug.LogError("Could not load character data because no PlayerManager was found in the world scene.");
            yield break;
        }

        if (currentCharacterData == null)
        {
            Debug.LogError("Could not load character data because no save data is loaded.");
            yield break;
        }

        player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);

        yield return null;
    }
}
