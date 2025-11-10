using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SaveSlotMenu : MonoBehaviour
{
    public float slideValue;
    private int _numPages;
    public GameObject nextButton; 
    public GameObject prevButton;
    public GameObject kosong;

    private int _currPages;
    private bool _isScrolling = false;

    public GameObject slot;
    private List<GameObject> _temp = new List<GameObject>();

    private MovementController _controller;

    private void Start()
    {
        _controller = FindObjectOfType<MovementController>();
    }

    private void OnEnable()
    {
        _currPages = 1;
        kosong.SetActive(false);
        ActivateMenu();
    }

    public void ActivateMenu()
    {
        //load semua slot yang ada
        Dictionary<string, GameData> profileGameData = DataPersistenceManager.instance.GetAllProfilesGameData();
        int i = 1;
        foreach (string profileId in profileGameData.Keys)
        {
            GameObject temp = Instantiate(slot, transform);
            _temp.Add(temp);
            SaveSlot saveSlot = temp.GetComponent<SaveSlot>();
            GameData profileData = null;
            profileGameData.TryGetValue(profileId, out profileData);

            saveSlot.SetData(profileData, i);
            saveSlot.SetProfileId(profileId);
            i++;
        }
        _numPages = i - 1;
    }

    public void NewSlot()
    {
        _controller.TransitionInAnimation(() => {
            DataPersistenceManager.instance.NewGame();
            SceneManager.LoadSceneAsync("Istana Siak");
        });    
    }

    private void Update()
    {
        if(_currPages == 1)
        {
            prevButton.SetActive(false);
        }
        else
        {
            prevButton.SetActive(true);
        }

        if(_currPages == _numPages)
        {
            nextButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
        }

        if (_numPages == 0)
        {
            kosong.SetActive(true);
            prevButton.SetActive(false);
            nextButton.SetActive(false);
        }
    }

    public void DeleteAll()
    {
        foreach(GameObject child in _temp)
        {
            Destroy(child);
        }
    }

    public void SlideNext()
    {
        if (!_isScrolling)
        {
            _isScrolling = true;
            _currPages++;
            LeanTween.moveLocal(gameObject, new Vector3(gameObject.transform.localPosition.x - slideValue, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z), 1f).setOnComplete(() => { _isScrolling = false; });
        }
    }

    public void SlidePrev()
    {
        if (!_isScrolling)
        {
            _currPages--;
            _isScrolling = true;
            LeanTween.moveLocal(gameObject, new Vector3(gameObject.transform.localPosition.x + slideValue, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z), 1f).setOnComplete(() => { _isScrolling = false; });
        }
    }


}
