using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BalaiRungsriController : MonoBehaviour
{
    public GameObject pembatasTiang;
    public GameObject pembatasTali;
    private MovementController _controller;

    private void Start()
    {
        _controller = FindObjectOfType<MovementController>();
    }
    public void RemovePembatas()
    {
        LeanTween.moveLocalX(pembatasTiang, pembatasTiang.transform.localPosition.x + 2f, 3f);
        LeanTween.scaleZ(pembatasTali, 0.3597459f, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
            _controller.ChangeScene("Balai Rungsri");
            _controller.ChangeLocation("to Balai");
        }
    }
}
