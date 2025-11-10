using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class PuzzleSocket : MonoBehaviour
{
    public int correctIndex;
    public int correctRotation;
    public Material trueMaterial;
    public Material falseMaterial;
    [HideInInspector]
    public bool isEmpty;

    private XRSocketInteractor _interactor;
    private MeshRenderer _meshRenderer;

    [HideInInspector]
    public bool isCorrect;

    private void Start()
    {
        isEmpty = true;
        _interactor = GetComponent<XRSocketInteractor>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    public void PieceEnterCheck()
    {
        Invoke("PieceEnter", 0.5f);
    }

    public void PieceEnter()
    {
        IXRSelectInteractable obj = _interactor.GetOldestInteractableSelected();
        int objIndex = obj.transform.gameObject.GetComponent<PuzzlePieceData>().index;
        if (objIndex == correctIndex && obj.transform.rotation.eulerAngles.z == correctRotation)
        {
            isCorrect = true;
        }
        else
        {
            isCorrect = false;
        }
    }

    public void SetIsEmpty(bool value)
    {
        isEmpty = value;
    }

    public void HoverCheck(bool value)
    {
        if (value)
        {
            if (isEmpty)
            {
                _meshRenderer.sharedMaterial = trueMaterial;
            }
            else
            {
                _meshRenderer.sharedMaterial = falseMaterial;
            }
           
        }
        else
        {
            _meshRenderer.SetSharedMaterials(new List<Material>());
        }
    }

    public void PieceExit()
    {
        isCorrect = false;
    }
}
