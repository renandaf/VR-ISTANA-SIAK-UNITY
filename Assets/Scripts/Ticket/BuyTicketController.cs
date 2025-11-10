using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuyTicketController : MonoBehaviour
{
    public XRSocketInteractor socket;
    private AudioManager _audioManager;
    public GameObject ticket;
    public Transform target;
    private GameObject placedObject;
    private MoneyData moneyData;
    private bool isPlaced = false;
    void Start()
    {
        _audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Money"))
        {
            moneyData = other.GetComponent<MoneyData>();
            if (!moneyData.GetIsGrab() && !isPlaced)
            {
                isPlaced = true;
                placedObject = other.gameObject;
                Invoke("TakeMoney", 1f);
            }

            if(moneyData.GetIsGrab())
            {
                CancelInvoke();
                isPlaced = false;
            }
        }
    }

    private void TakeMoney()
    {
        placedObject.GetComponent<Rigidbody>().isKinematic = true;
        Invoke("AudioPlay", 3.5f);
        _audioManager.Play("Print");
        var seq = LeanTween.sequence();
        seq.append(LeanTween.move(placedObject, target, 1.5f).setOnComplete(() => { _audioManager.Play("Proses Tiket"); }));
        seq.append(LeanTween.move(placedObject, target.position - new Vector3(0, 1f, 0), 1f).setOnComplete(() => { Destroy(placedObject); GetTicket(); }).setDelay(2f));
    }

    public void AudioPlay()
    {
        _audioManager.Play("Print");
    }

    private void GetTicket()
    {
        GameObject temp = Instantiate(ticket, target.position - new Vector3(0, 1f, 0), Quaternion.identity);
        temp.GetComponent<Ticket>().socket = socket;
        temp.GetComponent<Rigidbody>().isKinematic = true;
        var seq = LeanTween.sequence();
        seq.append(LeanTween.move(temp, target, 1.5f));
        seq.append(LeanTween.move(temp,  new Vector3(transform.position.x,target.position.y,transform.position.z), 2f).setOnComplete(() => { temp.GetComponent<Rigidbody>().isKinematic = false; _audioManager.Play("Scan Tiket"); }));
    }
}
