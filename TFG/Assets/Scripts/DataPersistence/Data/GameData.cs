using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int life, sceneIndex, isSelected;

    public string[] objectsArray;

    public int[] objectsQuantity;

    public Vector3 playerPosition;

    public SerializableDictionary<string, bool> enemiesDefeated;

    public SerializableDictionary<string, bool> objectCollected;

    public SerializableDictionary<string, bool> chestCollected;


    public GameData() {
        this.life = 6;
        objectsQuantity = new int[] { 0, 0, 0, 0, 0 ,0};
        objectsArray = new string[6] { "Sword Button", "Decoy Button", "WaterBall Button", "Health Button", "SuperHealth Button", "MegaHealth Button" };
        playerPosition = Vector3.zero;
        objectCollected = new SerializableDictionary<string, bool>();
        chestCollected = new SerializableDictionary<string, bool>();
        sceneIndex = 1;
        isSelected = 0;
    }
}
