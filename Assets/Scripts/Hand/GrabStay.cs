using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Content.Interaction;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class GrabStay : MonoBehaviour
{
    public Transform rightAttachPoint;
    public Transform leftAttachPoint;
    public Transform grabPoint;

    private bool _isGrab = false;
    private Transform hand;

    void Start()
    {
        XRBaseInteractable grabInteractable = GetComponent<XRBaseInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnsetPose);
    }
    private void Update()
    {
        if (_isGrab)
        {
            grabPoint.rotation = Quaternion.identity;
        }

    }

    public void SetupPose(BaseInteractionEventArgs arg)
    {
        //mengambil nilai dari handdata
        if (arg.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null)
        {

            hand = controllerInteractor.xrController.transform.GetChild(0);
            hand.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            var handPhysic = hand.GetComponent<HandPhysic>();
            var handData = hand.GetComponent<HandData>();
            handPhysic.isKnob = true;

            //check hand
            if (handData.handType == HandData.HandType.Right)
            {
                hand.parent = rightAttachPoint;
            }
            else
            {
                hand.parent = leftAttachPoint;
            }
                
                hand.localPosition = new Vector3(0, 0, 0);
                hand.localRotation = Quaternion.identity;
                _isGrab = true;
        }
    }

    public void UnsetPose(BaseInteractionEventArgs arg)
    {
        //mengambil nilai dari handdata
        if (arg.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null)
        {
            _isGrab = false;
            hand.parent = controllerInteractor.xrController.transform;
            hand.SetAsFirstSibling();
            hand.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            var handPhysic = hand.GetComponent<HandPhysic>();
            handPhysic.isKnob = false;
        }
    }
}
