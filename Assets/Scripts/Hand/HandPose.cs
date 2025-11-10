using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class HandPose : MonoBehaviour
{
    public float poseTransitionDuration;
    public HandData rightHandPose;
    public HandData leftHandPose;

    private Vector3 _startingHandPosition;
    private Vector3 _finalHandPosition;
    private Quaternion _startingHandRotation;
    private Quaternion _finalHandRotation;

    private Quaternion[] _startingFingerRotation;
    private Quaternion[] _finalFingerRotation;

    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnsetPose);

        rightHandPose.gameObject.SetActive(false);
        leftHandPose.gameObject.SetActive(false);

    }

    public void SetupPose(BaseInteractionEventArgs arg)
    {
        //mengambil nilai dari handdata
        if (arg.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null)
        {
            var handData = controllerInteractor.xrController.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = false;

            //check hand
            if (handData.handType == HandData.HandType.Right)
            {
                SetHandDataValues(handData, rightHandPose);
            }
            else
            {
                SetHandDataValues(handData, leftHandPose);
            }
            //set pose secara smooth
            StartCoroutine(SetHandDataRoutine(handData, _finalHandPosition, _finalHandRotation, _finalFingerRotation, _startingHandPosition, _startingHandRotation, _startingFingerRotation));
        }
    }

    public void SetHandDataValues(HandData h1, HandData h2)
    {
        _startingHandPosition = h1.root.localPosition;
        _finalHandPosition = h2.root.localPosition;
        _startingHandRotation = h1.root.localRotation;
        _finalHandRotation = h2.root.localRotation;

        _startingFingerRotation = new Quaternion[h1.fingerBones.Length];
        _finalFingerRotation = new Quaternion[h2.fingerBones.Length];

        for(int i = 0; i < h1.fingerBones.Length; i++)
        {
            _startingFingerRotation[i] = h1.fingerBones[i].localRotation;
            _finalFingerRotation[i] = h2.fingerBones[i].localRotation;
        }
    }

    public void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
    {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;

        for(int i = 0; i < newBonesRotation.Length; i++)
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
            var handData = controllerInteractor.xrController.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = true;
            //set pose secara smooth
            StartCoroutine(SetHandDataRoutine(handData,_startingHandPosition, _startingHandRotation, _startingFingerRotation, _finalHandPosition, _finalHandRotation, _finalFingerRotation));
        }
    }

    //mirror right hand pose to left hand pose
    #if UNITY_EDITOR

        [MenuItem("Tools/Mirror Selected Right Grab Pose")]

        public static void MirrorRightPose()
        {
            HandPose handPose = Selection.activeGameObject.GetComponent<HandPose>();
            handPose.MirrorPose(handPose.leftHandPose, handPose.rightHandPose);
        }
    #endif

    public void MirrorPose(HandData poseToMirror, HandData poseUsedToMirror)
    {
        Vector3 mirroredPosition = poseUsedToMirror.root.localPosition;
        mirroredPosition.x *= -1;

        Quaternion mirroredQuaternion = poseUsedToMirror.root.localRotation;
        mirroredQuaternion.y *= -1;
        mirroredQuaternion.z *= -1;

        poseToMirror.root.localPosition = mirroredPosition;
        poseToMirror.root.localRotation = mirroredQuaternion;

        for (int i = 0; i < poseUsedToMirror.fingerBones.Length; i++)
        {
            poseToMirror.fingerBones[i].localRotation = poseUsedToMirror.fingerBones[i].localRotation;
        }
    }
}
