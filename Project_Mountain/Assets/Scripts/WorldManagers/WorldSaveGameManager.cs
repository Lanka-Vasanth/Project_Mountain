using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;

    [SerializeField] PlayerManager playerManager;

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
    // public  CharacterSaveData characterSlot02;
    // public  CharacterSaveData characterSlot03;
    // public  CharacterSaveData characterSlot04;
    // public  CharacterSaveData characterSlot05;


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
    }

    private void CharacterFileNameDescriptor()
    {
        switch (currentCharacterSlotInUse)
        {
            case CharacterSlot.CharacterSlot_01:
                saveFileName = "CharacterSlot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                saveFileName = "CharacterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                saveFileName = "CharacterSlot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                saveFileName = "CharacterSlot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                saveFileName = "CharacterSlot_05";
                break;
            default:
                break;

        }
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

    public void CreateNewGame()
    {
        //CREATE NEW FILE< WITH FILE NAME DEPENDING ON SLOT
        CharacterFileNameDescriptor();

        currentCharacterData = new CharacterSaveData();
    }

     public void LoadGame()
    {
        //LOAD PREVIOUS FILE< WITH FILE NAME DEPENDING ON SLOT
        CharacterFileNameDescriptor();

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
        CharacterFileNameDescriptor();

        saveFileDataWriter = new SaveFileDataWrite();
        //MACHINE AGNOSTIC FILE PATH
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;

        //PASS PLAYER INFO FROM GAME AT CURRENT TIME TO SAVE FILE
        playerManager.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

        //WRITE THAT INFO ONTO JSON FILE, SAVE TO MACHINE
        saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);

    }

    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadOperator = SceneManager.LoadSceneAsync(worldSceneIndex);
        yield return null;
    }
}
