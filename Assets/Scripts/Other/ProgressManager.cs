using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour, IDataPersistence
{
    private int _progress;
    public GameObject tutorialRoom;
    public RoomCheck roomCheck;
    public GameObject pintuTutorial;
    public AudioManager audioManager;
    public GameObject boxCollider;

    public void GeserPintu()
    {
        LeanTween.moveLocalZ(pintuTutorial, pintuTutorial.transform.localPosition.z + 0.02f, 0.1f);
        LeanTween.moveLocalX(pintuTutorial, pintuTutorial.transform.localPosition.x - 1.6f,2.8f);
        audioManager.Play("Pintu");
    }

    private void Update()
    {
        if(roomCheck.currentTag == "Jalan" && _progress == 0)
        {
            _progress = 1;
            Destroy(tutorialRoom);
            boxCollider.SetActive(true);
            audioManager.Play("Intro");
            LeanTween.move(gameObject, gameObject.transform.position, 4f).setOnComplete(() => { audioManager.Play("Voice1"); });                                 
        }
    }

    public void LoadData(GameData data)
    {
        _progress = data.progress;
        if (_progress == 1 )
        {
            boxCollider.SetActive(true);
            Destroy(tutorialRoom);
        }
    }

    public void SaveData(GameData data)
    {
        data.progress = _progress;
    }
}
