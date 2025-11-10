using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class InformationUIArea : MonoBehaviour
{
    public GameObject[] listInformationUI;
    public GameObject audioMaster;
    public float uiDelay;
    private AnimatedHandController _leftRayInteractor;
    private AnimatedHandController _rightRayInteractor;
    public bool destroyOnFinish;
    public float timeDestroyOnFinish;

    private CanvasGroup[] _UIcanvas;
    private Vector3[] _originalPosition;
    private bool _active = false;
    

    private void Start()
    {
        _UIcanvas = new CanvasGroup[listInformationUI.Length];
        _originalPosition = new Vector3[listInformationUI.Length];
        _leftRayInteractor = FindObjectsOfType<AnimatedHandController>().FirstOrDefault(hand => hand.gameObject.name == "LeftHand");
        _rightRayInteractor = FindObjectsOfType<AnimatedHandController>().FirstOrDefault(hand => hand.gameObject.name == "RightHand");
        for (int i = 0; i < listInformationUI.Length; i++)
        {
            _originalPosition[i] = listInformationUI[i].transform.position;
            _UIcanvas[i] = listInformationUI[i].GetComponent<CanvasGroup>();
        }

    }

    void ToggleActive()
    {
        for (int i = 0; i < listInformationUI.Length; i++)
        {
            listInformationUI[i].SetActive(!listInformationUI[i].activeSelf);
            _leftRayInteractor.isInspect = listInformationUI[i].activeSelf;
            _rightRayInteractor.isInspect = listInformationUI[i].activeSelf;
            if (!listInformationUI[i].activeSelf)
            {
                listInformationUI[i].transform.rotation = Quaternion.identity;
                listInformationUI[i].transform.position = _originalPosition[i];
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")) && !_active)
        {
            _active = true;
            Invoke("DisplayInfo", uiDelay);
            for (int i = 0; i < listInformationUI.Length; i++)
            {
                
                LeanTween.alphaCanvas(_UIcanvas[i], 1, 0.5f);
            }
        }
    }

    private void DisplayInfo()
    {
        audioMaster.SetActive(true);
        if (destroyOnFinish)
        {
            Invoke("Destroy",timeDestroyOnFinish);
        }
        ToggleActive();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    public void HideInfo()
    {
        _active = false;
        audioMaster.SetActive(false);
        for (int i = 0; i < listInformationUI.Length; i++)
        {
            LeanTween.alphaCanvas(_UIcanvas[i], 0, 0.5f);
        }
        Invoke("ToggleActive", 0.5f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")) && _active)
        {
            HideInfo();
            CancelInvoke("Destroy");
        }
    }
}
