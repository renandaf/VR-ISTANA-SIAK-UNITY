using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private TextMeshProUGUI slotText;
    [SerializeField] private TextMeshProUGUI checkpoint;
    [SerializeField] private TextMeshProUGUI huruf;
    [SerializeField] private TextMeshProUGUI kuis;
    [SerializeField] private string scene;
    [SerializeField] private GameObject star1;
    [SerializeField] private GameObject star2;
    [SerializeField] private GameObject star3;
    [SerializeField] private GameObject star4;


    private MovementController _controller;

    private void Start()
    {
        _controller = FindObjectOfType<MovementController>();
    }

    public void SetData(GameData data, int number)
    {
        if(data == null)
        {
            slotText.text = "null";
            checkpoint.text = "null";
            huruf.text = "null";
            kuis.text = "null";
        }
        else
        {
            scene = data.scene;
            slotText.text = "Slot " + number;
            checkpoint.text = data.locationVisited.CountTrueValues() + "/20";
            huruf.text = data.puzzleCollected.CountTrueValues() + "/5";
            kuis.text = data.puzzleSolved.CountTrueValues() + "/4";
            data.medalEarned.TryGetValue("Medal 1", out bool isTrue1);
            data.medalEarned.TryGetValue("Medal 2", out bool isTrue2);
            data.medalEarned.TryGetValue("Medal 3", out bool isTrue3);
            data.medalEarned.TryGetValue("Medal 4", out bool isTrue4);
            if (isTrue1)
            {
                star1.SetActive(true);
            }
            if (isTrue2)
            {
                star2.SetActive(true);
            }
            if (isTrue3)
            {
                star3.SetActive(true);
            }
            if (isTrue4)
            {
                star4.SetActive(true);
            }
        }
    }

    public string GetProfileId()
    {
        return profileId;
    }

    public void SetProfileId(string id)
    {
        profileId = id;
    }

    public void OnSaveSlotClicked()
    {
        DataPersistenceManager.instance.ChangeSelectedProfileId(GetProfileId());
        _controller.ChangeScene(scene);
    }

    public void OnDeleteSlotClicked()
    {
        DataPersistenceManager.instance.DeleteSave(GetProfileId());
    }
}
