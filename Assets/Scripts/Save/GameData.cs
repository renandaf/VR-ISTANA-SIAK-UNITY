using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int progress;
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> puzzleCollected;
    public SerializableDictionary<string, bool> puzzleSolved;
    public SerializableDictionary<string, bool> locationVisited;
    public SerializableDictionary<string, bool> medalEarned;
    public float moveSpeed;
    public float turnSpeed;
    public float voiceVolume;
    public string scene;
    public GameData()
    {
        this.progress = 0;
        this.playerPosition = new Vector3(92.5899963f, -0.411000013f, 25.4099998f);
        this.puzzleCollected = new SerializableDictionary<string, bool>();
        this.puzzleSolved = new SerializableDictionary<string, bool>();
        this.locationVisited = new SerializableDictionary<string, bool>();
        this.medalEarned = new SerializableDictionary<string, bool>();
        this.moveSpeed = 2;
        this.turnSpeed = 25;
        this.voiceVolume = 0.8f;
        this.scene = "Istana Siak";
    }
}
