using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject savePanel;
    public GameObject aboutPanel;

    private VisionFollowerUI _followerUI;
    private CanvasGroup _canvasGroup;

    void Start()
    {
        _followerUI = GetComponent<VisionFollowerUI>();
        _canvasGroup = GetComponent<CanvasGroup>();
        Invoke("UpDownAnimation", 3f);
    }

    public void UpDownAnimation()
    {
        LeanTween.moveLocalY(gameObject, transform.position.y - 0.01f, 2f).setLoopType(LeanTweenType.pingPong);
    }

    public void QuitPressed()
    {
        Application.Quit();
    }

    public void StartPressed()
    {
        _followerUI.enabled = false;
        LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, new Vector3(0, 3.1500001f, 0.379000008f), 2f);
        LeanTween.alphaCanvas(_canvasGroup, 0, 0.5f).setOnComplete(() => {
            savePanel.SetActive(true);
            savePanel.GetComponent<VisionFollowerUI>().enabled = true;
        });
    }

    public void AboutPressed()
    {
        _followerUI.enabled = false;
        LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, new Vector3(0, 3.1500001f, 0.379000008f), 2f);
        LeanTween.alphaCanvas(_canvasGroup, 0, 0.5f).setOnComplete(() => {
            aboutPanel.SetActive(true);
            aboutPanel.GetComponent<VisionFollowerUI>().enabled = true;
        });
    }

    public void BackToMain()
    {
        aboutPanel.GetComponent<VisionFollowerUI>().enabled = false;
        LeanTween.move(aboutPanel, new Vector3(0, 3.1500001f, 0.379000008f), 2f);
        savePanel.GetComponent<VisionFollowerUI>().enabled = false;
        LeanTween.move(savePanel, new Vector3(0, 3.1500001f, 0.379000008f), 2f);
        gameObject.SetActive(true);
        _followerUI.enabled = true;
        Invoke("UpDownAnimation", 3f);
    }
}
