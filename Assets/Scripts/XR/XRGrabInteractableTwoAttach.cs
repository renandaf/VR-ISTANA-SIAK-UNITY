using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    public Transform leftAttachTransform;
    public Transform rightAttachTransform;
    public Transform socketAttachTransform;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null)
        {
            if (controllerInteractor.xrController.transform.CompareTag("Left Hand"))
            {
                attachTransform = leftAttachTransform;
            }
            else if (controllerInteractor.xrController.transform.CompareTag("Right Hand"))
            {
                attachTransform = rightAttachTransform;
            }
        }
        if (args.interactorObject is XRSocketInteractor)
        {
            attachTransform = socketAttachTransform;
        }
        base.OnSelectEntering(args);
    }
}
