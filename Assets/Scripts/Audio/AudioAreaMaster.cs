using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAreaMaster : MonoBehaviour
{
    private AudioManager _audioManager;
    private AudioSource _audioSource;
    public float fromSeconds;
    public float toSeconds;
    public bool destroyOnComplete;

    [Header("Audio")]
    [SerializeField] private AudioClip audioClip;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void OnEnable()
    {
        Invoke("PlayAudioClip", 0.5f);
    }

    public void PlayAudioClip()
    {
        _audioSource.clip = audioClip;
        _audioSource.Stop();
        _audioSource.Play();
        _audioSource.time = fromSeconds;
    }

    // Update is called once per frame
    private void Update()
    {
        _audioSource.volume = _audioManager.voiceOverVolume;
        if (_audioSource.time >= toSeconds && toSeconds > fromSeconds)
        {
            _audioSource.Pause();
            if (destroyOnComplete)
            {
                enabled = false;
            }
        }
    }
}
