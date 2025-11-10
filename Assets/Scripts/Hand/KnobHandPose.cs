using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Content.Interaction;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class KnobHandPose : MonoBehaviour
{
    public float poseTransitionDuration;
    public HandData rightHandPose;
    public HandData leftHandPose;
    public Transform rightAttachPoint;
    public Transform leftAttachPoint;
    public Transform handle;
    public Transform grabPoint;

    private bool _isGrab = false;
    private Transform hand;
    private Vector3 _startingHandPosition;
    private Vector3 _finalHandPosition;
    private Quaternion _startingHandRotation;
    private Quaternion _finalHandRotation;

    private Quaternion[] _startingFingerRotation;
    private Quaternion[] _finalFingerRotation;

    void Start()
    {
        XRBaseInteractable grabInteractable = GetComponent<XRBaseInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnsetPose);

        rightHandPose.gameObject.SetActive(false);
        leftHandPose.gameObject.SetActive(false);

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
            var handData = hand.GetComponent<HandData>();
            var handPhysic = hand.GetComponent<HandPhysic>();
            handPhysic.isKnob = true;
            handData.animator.enabled = false;

            //check hand
            if (handData.handType == HandData.HandType.Right)
            {
                hand.parent = rightAttachPoint;
                hand.localPosition = new Vector3(0, 0, 0);
                hand.localRotation = Quaternion.identity;
                SetHandDataValues(handData, rightHandPose);           
                _isGrab = true;
            }
            else
            {
                hand.parent = leftAttachPoint;
                hand.localPosition = new Vector3(0, 0, 0);
                hand.localRotation = Quaternion.identity;
                SetHandDataValues(handData, leftHandPose);
                _isGrab = true;
            }

            //SetHandData(handData, _finalHandPosition, _finalHandRotation, _finalFingerRotation);
            //set pose secara smooth
            StartCoroutine(SetHandDataRoutine(handData, _finalHandPosition, _finalHandRotation, _finalFingerRotation, _startingHandPosition, _startingHandRotation, _startingFingerRotation));
        }
    }

    public void SetHandDataValues(HandData h1, HandData h2)
    {
        _startingHandPosition = h1.root.localPosition;
        _finalHandPosition = h2.root.localPosition;

        //_startingHandPosition = new Vector3(h1.root.localPosition.x / h1.root.localScale.x,
        //                     h1.root.localPosition.y / h1.root.localScale.y, h1.root.localPosition.z / h1.root.localScale.z);

        //_finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x,
        //                             h2.root.localPosition.y / h2.root.localScale.y, h2.root.localPosition.z / h2.root.localScale.z);

        _startingHandRotation = h1.root.localRotation;
        _finalHandRotation = h2.root.localRotation;

        _startingFingerRotation = new Quaternion[h1.fingerBones.Length];
        _finalFingerRotation = new Quaternion[h2.fingerBones.Length];

        for (int i = 0; i < h1.fingerBones.Length; i++)
        {
            _startingFingerRotation[i] = h1.fingerBones[i].localRotation;
            _finalFingerRotation[i] = h2.fingerBones[i].localRotation;
        }
    }

    public void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
    {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;

        for (int i = 0; i < newBonesRotation.Length; i++)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }

    //set pose secara smooth
    public IEnumerator SetHandDataRoutine(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation, Vector3 startingPosition, Quaternion startingRotation, Quaternion[] startingBonesRotation)
    {
        float timer = 0f;

        while (timer < poseTransitionDuration)
        {
            Vector3 p = Vector3.Lerp(startingPosition, newPosition, timer / poseTransitionDuration);
            Quaternion r = Quaternion.Lerp(startingRotation, newRotation, timer / poseTransitionDuration);

            h.root.localPosition = p;
            h.root.localRotation = r;

            for (int i = 0; i < newBonesRotation.Length; i++)
            {
                h.fingerBones[i].localRotation = Quaternion.Lerp(startingBonesRotation[i], newBonesRotation[i], timer / poseTransitionDuration);
            }

            timer += Time.deltaTime;
            yield return null;
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
            var handData = hand.GetComponent<HandData>();
            var handPhysic = hand.GetComponent<HandPhysic>();
            handPhysic.isKnob = false;
            handData.animator.enabled = true;
            //SetHandData(handData, _startingHandPosition, _startingHandRotation, _startingFingerRotation);
            //set pose secara smooth
            StartCoroutine(SetHandDataRoutine(handData, _startingHandPosition, _startingHandRotation, _startingFingerRotation, _finalHandPosition, _finalHandRotation, _finalFingerRotation));
        }
    }
}
