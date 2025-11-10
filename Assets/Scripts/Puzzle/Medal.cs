using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.GraphicsBuffer;

public class Medal : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;
    private bool _collected = false;
    public XRSocketInteractor socket;
    private XRGrabInteractableTwoAttach _interactable;
    public LayerMask layerMask;

    private void Start()
    {
        _interactable = GetComponent<XRGrabInteractableTwoAttach>();
    }

    public void Earned()
    {
        _collected = true;
        gameObject.SetActive(true);
    }

    public void LoadData(GameData data)
    {
        data.medalEarned.TryGetValue(id, out _collected);
        if (_collected)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.medalEarned.ContainsKey(id))
        {
            data.medalEarned.Remove(id);
        }
        data.medalEarned.Add(id, _collected);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (layerMask == (layerMask | (1 << collision.gameObject.layer)) && _collected)
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
