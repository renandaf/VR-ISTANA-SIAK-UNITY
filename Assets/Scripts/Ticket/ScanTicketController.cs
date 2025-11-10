using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanTicketController : MonoBehaviour
{
    private bool isProcessing = false;
    public GameObject gate;
    public GameObject indicator;
    public GameObject block;
    public List<Material> indicatorNeutral;
    public List<Material> indicatorGreen;
    private AudioManager _audioManager;
    [HideInInspector]
    public bool _isOpen;

    private void Start()
    {
        _audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Ticket") && !_isOpen && !isProcessing)
        {
            isProcessing = true;
            Invoke("OpenGate", 1.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ticket"))
        {
            isProcessing = false;
            CancelInvoke("OpenGate");
        }
    }

    private void OpenGate()
    {
        _audioManager.Play("Beep");
        ChangeLight();
        LeanTween.rotateY(gate, 88, 2f);
        isProcessing = false;  
        block.SetActive(false);
    }

    public void ChangeLight()
    {   
        MeshRenderer indicatorRenderer = indicator.GetComponent<MeshRenderer>();
        if (indicatorRenderer.sharedMaterials[0] == indicatorNeutral[0] && !_isOpen)
        {
            indicatorRenderer.SetMaterials(indicatorGreen);
            _isOpen = true;
        }
        else if (indicatorRenderer.sharedMaterials[0] == indicatorGreen[0])
        {
            indicatorRenderer.SetMaterials(indicatorNeutral);
            _isOpen = false;
            LeanTween.rotateY(gate, 0, 2f);
        }
    }
}
