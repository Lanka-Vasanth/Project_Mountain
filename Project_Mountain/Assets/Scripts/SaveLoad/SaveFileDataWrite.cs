using UnityEngine;
using System;
using System.IO;
using NUnit.Framework.Constraints;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;

public class SaveFileDataWrite
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    //CHECK IF FILE EXISTS FIRST 
    public bool CheckFileExistence()
    {
        if(File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //DELETE CHARACTER SAVE FILE
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
    }

    //USE TO CREATE SAVE FILE UPON NEW GAME START
    public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
    {
        //MAKE PATH TO SAVE FILE 
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            //CREATE DIRECTORY FOR FILE, IF IT DOESNT EXIST
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("CREATING SAVE FILE, AT PATH: "+savePath);

            //SERIALIZE C# GAME DATA OBJECT -> JSON
            string dataToStore = JsonUtility.ToJson(characterData, true);

            //WRITE FILE TO MACHINE
            using(FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using(StreamWriter fileWriter = new StreamWriter(stream)){
                    fileWriter.Write(dataToStore);
                }
            }
        }
        catch(Exception ex)
        {
            Debug.LogError("ERROR WHILE TRYING TO SAVE CHARACTER DATA, GAME COULD NOT BE SAVED: "+savePath+"\n"+ ex);
        }
    }

    //USE TO LOAD SAVE FILE UPONN LOADING PREVIOUS GAME
    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null; 
        //PATH TO LOAD FILE 
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(loadPath))
        {
            try{
                string dataToLoad = "";

                using(FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //DESERIALIZE DATA FROM JSON -> UNITY
                characterData  = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }catch(Exception ex)
            {
                Debug.LogError("ERROR WHILE TRYING TO LOAD CHARACTER DATA, GAME COULD NOT BE SAVED: "+loadPath+"\n"+ ex);
            }
        }

        return characterData;
    }

}
