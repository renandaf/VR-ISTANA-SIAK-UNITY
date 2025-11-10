using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PuzzleClick : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;
    public GameObject puzzleIcon;
    private bool _collected = false;
    private AudioManager _audioManager;
    public GameObject spark;
    public GameObject puzzle;
    public bool sameScene;
    public GameObject puzzlePiece;

    private Tablet _tablet;
    private void Start()
    {
        _tablet = FindAnyObjectByType<Tablet>();
        _audioManager = FindAnyObjectByType<AudioManager>();
    }
    public void Click()
    {
        _tablet.puzzleCollected++;
        _collected = true;
        puzzleIcon.SetActive(false);
        if (sameScene) { 
            puzzlePiece.SetActive(true);
        }
        _audioManager.Play("Coin");
        var seq = LeanTween.sequence();
        seq.append(LeanTween.scale(gameObject, transform.localScale * 1.15f, 0.05f));
        seq.append(LeanTween.scale(gameObject, transform.localScale * 0.01f, 0.25f).setOnComplete(() => { puzzle.SetActive(false); Instantiate(spark, transform.position, Quaternion.identity); }));
    }

    public void LoadData(GameData data)
    {
        data.puzzleCollected.TryGetValue(id, out _collected);
        if (_collected)
        {
            puzzle.SetActive(false);
            puzzleIcon.SetActive(false);
            if (sameScene)
            {
                puzzlePiece.SetActive(true);
            }
        }
    }

    public void SaveData(GameData data)
    {
        if (data.puzzleCollected.ContainsKey(id))
        {
            data.puzzleCollected.Remove(id);
        }
        data.puzzleCollected.Add(id, _collected);
    }
}
