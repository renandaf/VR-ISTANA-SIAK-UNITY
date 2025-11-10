using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class AnimatedHandController : MonoBehaviour
{
    public InputActionReference gripInputActionReference;
    public InputActionReference triggerInputActionReference;
    public GameObject rayInteraction;
    public InteractionLayerMask layerMask;
    public LayerMask UIMask;
    [HideInInspector]
    public bool isInspect;

    private Animator _handAnimator;
    private float _gripValue;
    private float _triggerValue;
    private XRRayInteractor _ray;
    private LayerMask _originalMask;

    void Start()
    {
        _handAnimator = GetComponent<Animator>();
        _ray = rayInteraction.GetComponent<XRRayInteractor>();
        _originalMask = _ray.raycastMask;
    }

    void Update()
    {
        AnimateGrip();
        AnimateTrigger();
    }

    private void AnimateGrip()
    {
        _gripValue = gripInputActionReference.action.ReadValue<float>();
        _handAnimator.SetFloat("Grip", _gripValue);
    }

    private void AnimateTrigger()
    {
        _triggerValue = triggerInputActionReference.action.ReadValue<float>();
        _handAnimator.SetFloat("Trigger", _triggerValue);
        rayInteraction.SetActive(_triggerValue > 0.5f);
        if (rayInteraction.activeSelf)
        {
            _ray.raycastMask = _originalMask + UIMask;
        }
        else
        {
            _ray.raycastMask = _originalMask;
        }
        if (rayInteraction.activeSelf && !isInspect)
        {
            _ray.interactionLayers = layerMask;
        }
        else
        {
            _ray.interactionLayers = InteractionLayerMask.GetMask("Huruf");
        }
    }
}
