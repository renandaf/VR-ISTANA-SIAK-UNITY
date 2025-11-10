using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGate : MonoBehaviour
{
    public ScanTicketController scan;
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player") && scan._isOpen)
        {
            scan.ChangeLight();
            scan.block.SetActive(true);
        }     
    }
}
