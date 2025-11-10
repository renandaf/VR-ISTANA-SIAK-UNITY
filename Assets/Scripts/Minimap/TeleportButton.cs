using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportButton : MonoBehaviour
{
    private DenahController denah;
    public GameObject player;
    public Vector3 location;
    public float rotation;
    public Button _button;
    public string id;
    [HideInInspector]
    public bool visited = false;
    public string scene;

    private MovementController _controller;

    private void Start()
    {
        denah = FindAnyObjectByType<DenahController>();
        _controller = player.GetComponent<MovementController>();
    }

    private void Update()
    {
        if (visited)
        {
            ColorBlock Farbpalette = _button.colors;
            Farbpalette.normalColor = new Color(1, 0.9529412f, 0.7450981f);
            Farbpalette.highlightedColor = new Color(0.945098f, 0.8784314f, 0.5843138f);
            Farbpalette.pressedColor = new Color(1, 0.9529412f, 0.7450981f);
            Farbpalette.selectedColor = new Color(0.945098f, 0.8784314f, 0.5843138f);
            _button.colors = Farbpalette;
        }
        else
        {
            ColorBlock Farbpalette = _button.colors;
            Farbpalette.selectedColor = new Color(0.9150943f, 0.9150943f, 0.9150943f);
            Farbpalette.highlightedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f);
            Farbpalette.normalColor = new Color(0.9150943f, 0.9150943f, 0.9150943f);
            Farbpalette.pressedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f);
            _button.colors = Farbpalette;
        }
    }

    public void Teleport()
    {
        denah.ToggleMenu();
        if(_controller.sceneName == scene)
        {
            _controller.TransitionInAnimation(TeleportEnd);
        }
        else
        {
            _controller.ChangeScene(scene);
            _controller.ChangeLocationTeleport(location);
        }
    }

    public void TeleportEnd()
    {
        player.transform.position = location;
        player.transform.eulerAngles = new Vector3(0, rotation, 0);
        _controller.TransitionOutAnimation();
    }
}
