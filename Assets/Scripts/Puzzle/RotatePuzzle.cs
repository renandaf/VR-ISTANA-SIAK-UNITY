using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RotatePuzzle : MonoBehaviour
{
    private XRSocketInteractor _socket;
    private bool _enable = false;
    private Transform _puzzle;
    private float _puzzleRotation;
    public Transform attachPoint;

    private void Start()
    {
        _socket = GetComponent<XRSocketInteractor>();
    }

    public void SetRotation()
    {
        IXRHoverInteractable puzzle = _socket.GetOldestInteractableHovered();
        _puzzle = puzzle.transform;
    }

    private void CheckRotation()
    {
        _puzzleRotation = _puzzle.rotation.eulerAngles.z;
    }

    public void SetEnable(bool enable)
    {
        _enable = enable;
    }

    public void RotateAttachPoint()
    {
        attachPoint.localRotation = Quaternion.identity;
        if (_puzzleRotation > 45f && _puzzleRotation < 135f)
        {
            attachPoint.localRotation = Quaternion.Euler(new Vector3(0, 180, 90));
        }
        else if (_puzzleRotation > 135f && _puzzleRotation < 225f)
        {
            attachPoint.localRotation = Quaternion.Euler(new Vector3(0, 180, 180));
        }
        else if (_puzzleRotation > 225f && _puzzleRotation < 315f)
        {
            attachPoint.localRotation = Quaternion.Euler(new Vector3(0, 180, 270));
        }
        else
        {
            attachPoint.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }

    private void Update()
    {
        if (_enable)
        {
            CheckRotation();
        }
    }
}
