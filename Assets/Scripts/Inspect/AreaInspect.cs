using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class AreaInspect : MonoBehaviour
{
    private GameObject _xrOrigin;
    private Transform _leftHand;
    private Transform _rightHand;
    private AnimatedHandController _leftController;
    private AnimatedHandController _rightController;
    private Rigidbody _leftHandRigid;
    private Rigidbody _rightHandRigid;
    public LayerMask excludeMask;
    public GameObject informationUI;
    private Camera _mainCamera;
    public float distance;
    private Transform _inspectArea;
    public Transform inspectObject;
    public Vector3 targetScale;

    private bool _isVisible = true;
    private Vector3 _velocity1 = Vector3.zero;
    private Vector3 _velocity2 = Vector3.zero;
    private Vector3 _velocity3 = Vector3.zero;
    private UniversalAdditionalCameraData _camData;
    private Vector3 _originalPosition;
    private Vector3 _originalObjectRotation;
    private Vector3 _originalRotation;
    private Vector3 _originalSize;
    private InspectItem _inspectItem;
    private LayerMask _originalLayer;
    private Material _mat;
    private CanvasGroup _informationUICanvas;
    private Vector3 _canvasSize;
    private AudioManager _audioManager;

    private ActionBasedContinuousMoveProvider _move;
    public XRRayInteractor _teleport;
    private InputActionProperty _inputActionProperty;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        _xrOrigin = FindObjectOfType<XROrigin>().gameObject;
        _mainCamera = Camera.main;
        _inspectArea = _mainCamera.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.gameObject.name == "InspectArea");
        _leftHand = _xrOrigin.transform.Find("Camera Offset/Left Controller/LeftHand");
        _rightHand = _xrOrigin.transform.Find("Camera Offset/Right Controller/RightHand");
        _leftController = _leftHand.GetComponent<AnimatedHandController>();
        _leftHandRigid = _leftHand.GetComponent<Rigidbody>();
        _rightController = _rightHand.GetComponent<AnimatedHandController>();
        _rightHandRigid = _rightHand.GetComponent<Rigidbody>();
        _informationUICanvas = informationUI.GetComponent<CanvasGroup>();
        _inspectItem = inspectObject.GetComponent<InspectItem>();
        _originalPosition = inspectObject.transform.position;
        _originalRotation = inspectObject.transform.rotation.eulerAngles;
        _originalSize = inspectObject.transform.localScale;
        _originalLayer = inspectObject.gameObject.layer;
        _camData = _mainCamera.GetUniversalAdditionalCameraData();
        _canvasSize = informationUI.transform.localScale;

        _move = FindObjectOfType<ActionBasedContinuousMoveProvider>();
        _inputActionProperty = _move.leftHandMoveAction;
    }

    private void Update()
    {
        if (!_isVisible)
        {
            Vector3 targetDir = _mainCamera.transform.position - inspectObject.transform.position;
            targetDir.y = 0;
            Vector3 newDir = Vector3.RotateTowards(inspectObject.transform.forward, targetDir, 1 * Time.deltaTime, 0.0F);
            inspectObject.transform.rotation = Quaternion.LookRotation(newDir);
            informationUI.transform.rotation = Quaternion.LookRotation(newDir);

            Vector3 targetPosition = FindTargetPosition();
            MoveToward(targetPosition);
            if (ReachedPosition(targetPosition))
            {
                _isVisible = true;
                ToggleInspect();
            }
        }
    }

    private void ToggleInspect()
    {
        informationUI.transform.localScale = _canvasSize;
        if (informationUI.activeSelf)
        {
            informationUI.SetActive(false);
            _informationUICanvas.alpha = 0;
        }
        else
        {
            informationUI.SetActive(true);
            LeanTween.alphaCanvas(_informationUICanvas, 1, 1);
        }
        
        _inspectItem.enabled = !_inspectItem.enabled;
    }

    private Vector3 FindTargetPosition()
    {
        return _mainCamera.transform.position + (new Vector3(_mainCamera.transform.forward.x, 0, _mainCamera.transform.forward.z) * distance);
    }
    private void MoveToward(Vector3 targetPosition)
    {
        inspectObject.transform.position = Vector3.SmoothDamp(inspectObject.transform.position, targetPosition, ref _velocity1, 1f);
        informationUI.transform.position = Vector3.SmoothDamp(informationUI.transform.position, targetPosition, ref _velocity3, 1f);
        if (inspectObject.transform.localScale != targetScale)
        {
            inspectObject.transform.localScale = Vector3.SmoothDamp(inspectObject.transform.localScale, targetScale, ref _velocity2, 1f);
        }    
    }

    private bool ReachedPosition(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, inspectObject.transform.position) < 0.05f;
    }

    public void EnterView()
    {
        _audioManager.Stop("Voice1");
        inspectObject.gameObject.layer = LayerMask.NameToLayer("Inspect");
        _isVisible = false;
        _teleport.enabled = false;
        _move.leftHandMoveAction = new InputActionProperty();

        _leftController.isInspect = true;
        _rightController.isInspect = true;

        _camData.SetRenderer(1);

        _mat = _inspectArea.gameObject.GetComponent<Renderer>().material;
        LeanTween.value(_inspectArea.gameObject, 0, 0.99f, 2f).setOnUpdate((float val) =>
        {
            Color c = _mat.color;
            c.a = val;
            _mat.color = c;
        });

        _leftHandRigid.excludeLayers = _leftHandRigid.excludeLayers + excludeMask;
        _rightHandRigid.excludeLayers = _rightHandRigid.excludeLayers + excludeMask;
    }

    public void ExitView()
    {
        LeanTween.value(_inspectArea.gameObject, 0.99f, 0, 2).setOnUpdate((float val) =>
        {
            Color c = _mat.color;
            c.a = val;
            _mat.color = c;
        });
        LeanTween.move(inspectObject.gameObject, _originalPosition, 2);
        LeanTween.scale(inspectObject.gameObject, _originalSize, 2);
        LeanTween.rotate(inspectObject.gameObject, _originalObjectRotation, 2);
        LeanTween.rotate(inspectObject.gameObject, _originalRotation, 2);
        ToggleInspect();
        _camData.SetRenderer(0);
        _leftHandRigid.excludeLayers = _leftHandRigid.excludeLayers - excludeMask;
        _rightHandRigid.excludeLayers = _rightHandRigid.excludeLayers - excludeMask;
        _teleport.enabled = true;
        _move.leftHandMoveAction = _inputActionProperty;
        _leftController.isInspect = false;
        _rightController.isInspect = false;
        inspectObject.gameObject.layer = _originalLayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player"))){
            Invoke("EnterView", 3);         
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player"))){
            CancelInvoke("EnterView");
        }
    }
}
