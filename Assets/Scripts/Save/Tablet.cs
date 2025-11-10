using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.GraphicsBuffer;

public class Tablet : MonoBehaviour, IDataPersistence
{
    public XRSocketInteractor socket;
    private XRGrabInteractableTwoAttach _interactable;
    public LayerMask layerMask;
    public TMP_Text checkpoint;
    public TMP_Text kuis;
    public TMP_Text huruf;

    [HideInInspector]
    public float puzzleCollected;
    public float puzzleSolved;
    public float locationVisited;

    private void Update()
    {
        checkpoint.text = locationVisited + "/20";
        huruf.text = puzzleCollected + "/5";
        kuis.text = puzzleSolved + "/4";
    }

    private void Start()
    {
        _interactable = GetComponent<XRGrabInteractableTwoAttach>();
    }

    public void LoadData(GameData data)
    {
        locationVisited = data.locationVisited.CountTrueValues();
        puzzleCollected = data.puzzleCollected.CountTrueValues();
        puzzleSolved = data.puzzleSolved.CountTrueValues();
    }

    public void SaveData(GameData data)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (layerMask == (layerMask | (1 << collision.gameObject.layer)))
        {
            EnterSocket();
        }
    }

    [System.Obsolete]
    public void EnterSocket()
    {
        socket.interactionManager.SelectEnter(socket, _interactable);
    }
}
