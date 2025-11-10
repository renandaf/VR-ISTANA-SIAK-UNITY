using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PuzzlePieceData : MonoBehaviour
{
    private XRGrabInteractableTwoAttach _interactible;
    public int index;
    public MeshRenderer meshRenderer;
    public GameObject image;
    public InteractionLayerMask mask;

    private void Start()
    {
        _interactible = GetComponent<XRGrabInteractableTwoAttach>();
    }

    public void PuzzlePlaced()
    {
        IXRSelectInteractor interactor = _interactible.GetOldestInteractorSelecting();
        if(interactor.interactionLayers.Equals(mask))
        {
            meshRenderer.enabled = false;
            image.SetActive(false);
        }
    }

    public void PuzzleRemoved()
    {
        meshRenderer.enabled = true;
        image.SetActive(true);
    }   
}
