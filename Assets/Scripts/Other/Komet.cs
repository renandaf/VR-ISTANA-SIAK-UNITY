using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class Komet : MonoBehaviour
{
    public XRKnob knob;
    public AudioSource audioSourceClip;
    public AudioSource audioSourceKomet;
    public AudioClip soundClip;
    public AudioClip kometSound;
    public float delay;
    public GameObject bulat;
    private float _prevValue;

    public GameObject stopButton;


    private float _tempValue;
    private bool _isPlaying;

    private void Start()
    {
        _prevValue = delay;
    }

    void Update()
    {
        if (knob.value >= _prevValue)
        {
            audioSourceClip.PlayOneShot(soundClip, 1);
            _prevValue = _prevValue + delay;
            if (!_isPlaying) {
                _tempValue = _tempValue + 0.1f;
            }           
        }

        if (knob.value <= _prevValue - delay && knob.value != 0)
        {
            audioSourceClip.PlayOneShot(soundClip, 1);
            _prevValue = _prevValue - delay;
            if (!_isPlaying)
            {
                _tempValue = _tempValue - 0.1f;
            }           
        }

        if (_tempValue > 10 || _tempValue < -10 && !_isPlaying)
        {
            stopButton.SetActive(true);
            LeanTween.rotateZ(bulat, -1080, 181);
            _tempValue = 0;
            audioSourceKomet.PlayOneShot(kometSound, 1);
            _isPlaying = true;
            Invoke("Change", 185);
        }
    }

    public void Stop()
    {
        audioSourceKomet.Stop();
        CancelInvoke("Change");
        Change();
        LeanTween.cancel(bulat);
    }

    public void Change()
    {
       _isPlaying = false;
    }
}
