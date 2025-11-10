using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleGroup : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;
    private AudioManager _audioManager;
    public PuzzleSocket[] listPuzzle;
    public GameObject errorText;
    private bool _isComplete = false;
    private float _temp;
    public GameObject tombol;
    public Medal medal;
    public GameObject medalDuplicate;
    private bool getMedal;
    public GameObject puzzleBox;

    public List<GameObject> puzzleList;

    private Tablet _tablet;

    private void Start()
    {
        _tablet = FindAnyObjectByType<Tablet>();
        _audioManager = FindAnyObjectByType<AudioManager>();
    }

    public void Menyerah()
    {
        foreach (GameObject puzzle in puzzleList) { 
            Destroy(puzzle);
        }
        getMedal = false;
        _isComplete = true;
        _audioManager.Play("Complete");
        Invoke("Complete", 1);
    }

    public void Check()
    {
        _temp = 0;
        foreach (PuzzleSocket piece in listPuzzle)
        {
            if (piece.isCorrect)
            {
                _temp++;
            }
            else
            {
               
            }
        }
        _isComplete = _temp == listPuzzle.Length;
        if (_isComplete)
        {
            getMedal = true;
            _audioManager.Play("Complete");
            Invoke("Complete", 3);

        }
        else
        {
            _audioManager.Play("Error");
            errorText.SetActive(true);
            CancelInvoke("ErrorMessage");
            Invoke("ErrorMessage",2f);
        }
    }

    public void ErrorMessage()
    {
        errorText.SetActive(false);
    }

    public void Complete()
    {
        _tablet.puzzleSolved++;
        LeanTween.scale(gameObject, gameObject.transform.localScale * 0.01f, 0.4f).setOnComplete(() => 
        {
            foreach (GameObject puzzle in puzzleList)
            {
                Destroy(puzzle);
            }
            puzzleBox.SetActive(false);
            if (getMedal)
            {
                _audioManager.Play("Medal");
                medal.Earned();
                medalDuplicate.SetActive(true);
                LeanTween.scale(medalDuplicate, medalDuplicate.transform.localScale * 50, 0.5f);
                LeanTween.scale(medalDuplicate, medalDuplicate.transform.localScale / 50, 0.5f).setDelay(1.5f).setOnComplete(() => { medalDuplicate.SetActive(false); });
            }
        });     
        tombol.SetActive(true);
        LeanTween.alphaCanvas(tombol.GetComponent<CanvasGroup>(), 1, 0.5f).setDelay(3f);        
    }

    public void LoadData(GameData data)
    {
        data.puzzleSolved.TryGetValue(id, out _isComplete);
        if (_isComplete)
        {
            puzzleBox.SetActive(false);
            tombol.SetActive(true);
            tombol.GetComponent<CanvasGroup>().alpha = 1;
        }
    }

    public void SaveData(GameData data)
    {
        if (data.puzzleSolved.ContainsKey(id))
        {
            data.puzzleSolved.Remove(id);
        }
        data.puzzleSolved.Add(id, _isComplete);
    }
}
