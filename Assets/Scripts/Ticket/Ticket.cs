using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.GraphicsBuffer;

public class Ticket : MonoBehaviour
{
    public XRSocketInteractor socket;
    private XRGrabInteractableTwoAttach _interactable;
    public LayerMask layerMask;


    private void Start()
    {
        _interactable = GetComponent<XRGrabInteractableTwoAttach>();
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
        if (socket.GetOldestInteractableSelected() != null)
        {
            IXRSelectInteractable interactable = socket.GetOldestInteractableSelected();
            socket.interactionManager.SelectExit(socket, interactable);
        }
        socket.interactionManager.SelectEnter(socket, _interactable);
    }
}
