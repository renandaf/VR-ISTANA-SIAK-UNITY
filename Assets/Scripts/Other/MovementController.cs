using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.Rendering.DebugUI;

public class MovementController : MonoBehaviour,IDataPersistence
{
    public InputActionReference moveInputActionReference;
    public GameObject transition;
    public bool doTransition;
    private AudioManager _audioManager;
    private bool _isMoving;
    [HideInInspector]
    public Material transMaterial;
    public GameObject locomotionSystem;
    public string sceneName;

    private bool isSceneChange;
    private Vector3 location;
    public bool isTesting;
    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        MeshRenderer mr = transition.GetComponent<MeshRenderer>();
        transMaterial = mr.sharedMaterial;
        if (doTransition)
        {
            TransitionOutAnimation();
        }      
    }

    public void TransitionOutAnimation()
    {
        transition.SetActive(true);
        
        Color start = transMaterial.GetColor("_BaseColor");
        Color temp = start;
        LeanTween.value(gameObject, start.a, 0, 5f)
           .setOnUpdate((float alpha) =>
           {
               Color updatedColor = start;
               updatedColor.a = alpha;
               transMaterial.SetColor("_BaseColor", updatedColor);
           })
           .setDelay(1f)
           .setOnComplete(() => {
               transition.SetActive(false); transMaterial.SetColor("_BaseColor", temp); ;
           });
    }

    public void TransitionInAnimation(Action action)
    {
        LeanTween.cancel(gameObject);
        Color start = transMaterial.GetColor("_BaseColor");
        Color temp = start;
        start.a = 0;
        transMaterial.SetColor("_BaseColor", start);

        transition.SetActive(true);
        LeanTween.value(gameObject, start.a, 1, 1f)
           .setOnUpdate((float alpha) =>
           {
               Color updatedColor = start;
               updatedColor.a = alpha;
               transMaterial.SetColor("_BaseColor", updatedColor);
           }).setOnComplete(action);
    }

    void Update()
    {
        if (moveInputActionReference.action.IsPressed() && !_isMoving) {
            if (locomotionSystem.activeSelf)
            {
                _audioManager.Play("Step");
                _isMoving = true;
            }           
        }

        if(!moveInputActionReference.action.IsPressed())
        {
            _audioManager.Stop("Step");
            _isMoving = false;
        }
    }
    public void LoadData(GameData data)
    {
        if (!isTesting)
        {
            transform.position = data.playerPosition;
        }
        sceneName = data.scene;
    }

    public void SaveData(GameData data)
    {
       
        if (isSceneChange)
        {
            data.playerPosition = location;
            isSceneChange = false;
        }
        else
        {
            data.playerPosition = transform.position;
        }
        data.scene = sceneName;
    }

    public void ChangeScene(string sceneName)
    {
        TransitionInAnimation(() => {
            this.sceneName = sceneName;
            SceneManager.LoadSceneAsync(sceneName);
        });
    }

    public void ChangeLocation(string id)
    {
        if(id == "Balai to Lantai 1")
        {
            location = new Vector3(-4.46500015f, -4.45100021f, 0.365999997f);
            isSceneChange = true;
        }
        else if (id == "Istana to Lantai 1")
        {
            location = new Vector3(-8.43999958f, -4.451f, -11.3599997f);
            isSceneChange = true;
        }
        else if (id == "Lantai 1 to Istana")
        {
            location = new Vector3(88.3300018f, -0.411000013f, 98.9990005f);
            isSceneChange = true;
        }
        else if (id == "Lantai 1 to Lantai 2")
        {
            location = new Vector3(-5.35599995f, -4.54699993f, -19.7549992f);
            isSceneChange = true;
        }
        else if (id == "Lantai 2 to Lantai 1")
        {
            location = new Vector3(-10.9700003f, -4.45100021f, 16.507f);
            isSceneChange = true;
        }
        else if (id == "to Balai")
        {
            location = new Vector3(2.42000008f, -0.456f, 2.1099999f);
            isSceneChange = true;
        }
        else if (id == "to Menu")
        {
            location = new Vector3(0, 0, 0);
            isSceneChange = true;
        }
    }

    public void ChangeLocationTeleport(Vector3 loc)
    {
        location = loc;
        isSceneChange = true;
    }
}
