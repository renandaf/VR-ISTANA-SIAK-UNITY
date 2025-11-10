using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class HandMenuController : MonoBehaviour, IDataPersistence
{
    [Header("Objek")]
    public AudioManager audioManager;
    public ActionBasedContinuousMoveProvider locomotionMove;
    public ActionBasedContinuousTurnProvider locomotionTurn;
    public InputActionReference inputActionReference;
    public GameObject handMenuCanvas;
    public GameObject success;

    [Header("Menu")]
    public Slider sMoveSpeed;
    public Slider sTurnSpeed;
    public Slider sVoiceVolume;

    private bool _isReady = true;
    private MovementController _movementController;

    private void Start()
    {
        _movementController = FindAnyObjectByType<MovementController>();
    }

    void Update()
    {
        if (_isReady)
        {
            ToggleHandMenu();
        }

        locomotionMove.moveSpeed = sMoveSpeed.value;
        locomotionTurn.turnSpeed = sTurnSpeed.value;
        audioManager.voiceOverVolume = sVoiceVolume.value;
    }

    private void ToggleHandMenu()
    {
        if (inputActionReference.action.IsPressed())
        {
            handMenuCanvas.SetActive(!handMenuCanvas.activeSelf);
            _isReady = false;
            Invoke("Delay", 0.5f);
        }
    }

    private void Delay()
    {
        _isReady = true;
    }

    public void ClickSave()
    {
        DataPersistenceManager.instance.SaveGame();
        success.SetActive(true);
        Invoke("Success", 2f);
    }

    public void Success()
    {
        success.SetActive(false);
    }

    public void ClickExit()
    {
        DataPersistenceManager.instance.ChangeSelectedProfileId("test");
        _movementController.ChangeScene("Menu Scene");

    }

    public void LoadData(GameData data)
    {
        sMoveSpeed.value = data.moveSpeed;
        sTurnSpeed.value = data.turnSpeed;
        sVoiceVolume.value = data.voiceVolume;
    }

    public void SaveData(GameData data)
    {
        data.moveSpeed = sMoveSpeed.value;
        data.turnSpeed = sTurnSpeed.value;
        data.voiceVolume = sVoiceVolume.value;
    }
}
