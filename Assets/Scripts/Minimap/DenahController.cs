using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class DenahController : MonoBehaviour
{
    public InputActionReference inputActionReference;
    public GameObject denahCanvas;

    private ActionBasedContinuousMoveProvider _move;
    public XRRayInteractor _teleport;
    private InputActionProperty _inputActionProperty;
    private bool _isReady = true;

    private void Start()
    {
        _move = FindObjectOfType<ActionBasedContinuousMoveProvider>();
        _inputActionProperty = _move.leftHandMoveAction;
    }

    void Update()
    {
        if (_isReady)
        {
            if (inputActionReference.action.IsPressed())
            {
                ToggleMenu();
            }
        }
    }

    public void ToggleMenu()
    {
        denahCanvas.SetActive(!denahCanvas.activeSelf);
        _teleport.enabled = !denahCanvas.activeSelf;
        if (denahCanvas.activeSelf)
        {
            _move.leftHandMoveAction = new InputActionProperty();
        }
        else
        {
            _move.leftHandMoveAction = _inputActionProperty;
        }
        _isReady = false;
        Invoke("Delay", 0.5f);
    }

    private void Delay()
    {
        _isReady = true;
    }

}
