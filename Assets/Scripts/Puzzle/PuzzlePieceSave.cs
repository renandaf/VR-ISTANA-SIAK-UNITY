using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceSave : MonoBehaviour, IDataPersistence
{
    public string id;
    private bool _isActive;
    public GameObject puzzle;

    public void LoadData(GameData data)
    {
        data.puzzleCollected.TryGetValue(id, out _isActive);
        if (_isActive)
        {
            puzzle.SetActive(true);
        }
    }

    public void SaveData(GameData data)
    {

    }
}
