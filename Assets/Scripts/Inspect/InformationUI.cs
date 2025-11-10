using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class InformationUI : MonoBehaviour
{
    private Transform _player;
    public GameObject informationUI;
    private AnimatedHandController _leftRayInteractor;
    private AnimatedHandController _rightRayInteractor;
    public float disableDistance;
    public bool isCallout;
    public Callout[] calloutList;

    private CanvasGroup _UIcanvas;
    private bool _stop;
    private Vector3 _originalPosition;
    public Outline[] outline;

    private void Awake()
    {
        _player = Camera.main.transform;
        _leftRayInteractor = FindObjectsOfType<AnimatedHandController>().FirstOrDefault(hand => hand.gameObject.name == "LeftHand");
        _rightRayInteractor = FindObjectsOfType<AnimatedHandController>().FirstOrDefault(hand => hand.gameObject.name == "RightHand");
    }

    void OnEnable()
    {
        _originalPosition = informationUI.transform.position;
        _stop = false;
        _UIcanvas = informationUI.GetComponent<CanvasGroup>();
        ToggleActive();
        LeanTween.alphaCanvas(_UIcanvas, 1, 0.5f);        
    }

    private void OnDisable()
    {
        if (!_stop)
        {
            LeanTween.alphaCanvas(_UIcanvas, 0, 0.5f).setOnComplete(ToggleActive); 
        }       
    }

    void ToggleActive()
    {
        informationUI.SetActive(!informationUI.activeSelf);
        _leftRayInteractor.isInspect = informationUI.activeSelf;
        _rightRayInteractor.isInspect = informationUI.activeSelf;
        foreach (Outline a in outline)
        {
            a.enabled = !informationUI.activeSelf;
        }       
        if (!informationUI.activeSelf)
        {        
            informationUI.transform.rotation = Quaternion.identity;
            informationUI.transform.position = _originalPosition;
            if (isCallout)
            {
                foreach (Callout callout in calloutList)
                {
                    callout.GazeHoverEnd();
                }              
            }
        }
        if (_stop)
        {
            enabled = false;
        }
    }

    public void EnterCallout()
    {
        if (isCallout)
        {
            foreach (Callout callout in calloutList)
            {
                callout.GazeHoverStart();
            }
        }
    }

    private void Update()
    {
        if (Vector3.Distance(_player.position, transform.position) > disableDistance && !_stop)
        {
            _stop = true;
            LeanTween.alphaCanvas(_UIcanvas, 0, 0.5f).setOnComplete(ToggleActive);
        }
    }
}
