using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//REFERENCE THIS DATA FOR EVERY SAVE FILE
public class CharacterSaveData
{
    [Header("Character Name")]
    public string characterName;

    [Header("Time Played")]
    public float secondsPlayed;

    //CAN ONLY SAVE BASIC/PRIMITIVE VARIABLE TYPES FOR SERIALIZATION
    [Header("WorldCoordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;
}
