using TMPro;
using UnityEngine;

public class UICharacterSaveSlot : MonoBehaviour
{
    SaveFileDataWrite saveFileWriter;

    [Header("Game Slot")]
    public CharacterSlot characterSlot;

    [Header("Character Info")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI timePlayed;

    private void OnEnable()
    {
        LoadSaveSlots();
    }

    private void LoadSaveSlots()
    {
        saveFileWriter = new SaveFileDataWrite();
        saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

        //SAVE SLOT 01
        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot_01:
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.CharacterFileNameDescriptor(characterSlot);

                if (saveFileWriter.CheckFileExistence())
                {
                    characterName .text = WorldSaveGameManager.instance.characterSlot01.characterName;
                }
                else
                {
                    gameObject.SetActive(false);    
                }

                break;
            case CharacterSlot.CharacterSlot_02:
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.CharacterFileNameDescriptor(characterSlot);

                if (saveFileWriter.CheckFileExistence())
                {
                    characterName .text = WorldSaveGameManager.instance.characterSlot02.characterName;
                }
                else
                {
                    gameObject.SetActive(false);    
                }

                break;
            case CharacterSlot.CharacterSlot_03:
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.CharacterFileNameDescriptor(characterSlot);

                if (saveFileWriter.CheckFileExistence())
                {
                    characterName .text = WorldSaveGameManager.instance.characterSlot03.characterName;
                }
                else
                {
                    gameObject.SetActive(false);    
                }

                break;
            case CharacterSlot.CharacterSlot_04:
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.CharacterFileNameDescriptor(characterSlot);

                if (saveFileWriter.CheckFileExistence())
                {
                    characterName .text = WorldSaveGameManager.instance.characterSlot04.characterName;
                }
                else
                {
                    gameObject.SetActive(false);    
                }

                break;
            case CharacterSlot.CharacterSlot_05:
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.CharacterFileNameDescriptor(characterSlot);

                if (saveFileWriter.CheckFileExistence())
                {
                    characterName .text = WorldSaveGameManager.instance.characterSlot05.characterName;
                }
                else
                {
                    gameObject.SetActive(false);    
                }

                break;
            default:
                break;
        }
    }
}
