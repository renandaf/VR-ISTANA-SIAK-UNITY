using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class PuzzleKnob : MonoBehaviour
{
    private XRKnob _knob;
    public PuzzleSocket puzzleSocket;
    void Start()
    {
        _knob = GetComponent<XRKnob>();
    }
    public void RotateKnob()
    {
        if (_knob.isSelected)
        {
            if (_knob.value >= 0.45f && _knob.value <= 0.55f)
            {
                puzzleSocket.isCorrect = true;
            }
            else
            {
                puzzleSocket.isCorrect = false;
            }
        }
    }

    private void Update()
    {
        RotateKnob();
    }

    public void SetKnobValue()
    {
        _knob.value = 0.5f;
    }
}
