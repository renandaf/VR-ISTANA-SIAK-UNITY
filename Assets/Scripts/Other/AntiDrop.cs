using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiDrop : MonoBehaviour
{
    private Rigidbody _attachedRigidbody;
    public LayerMask layerMask;
    private Vector3 _originalPos;
    private Vector3 _originalRot;
    private bool _isDrop = false;

    void Start()
    {
        _attachedRigidbody = GetComponent<Rigidbody>();
        _originalPos = transform.position;
        _originalRot = transform.rotation.eulerAngles;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (layerMask == (layerMask | (1 << collision.gameObject.layer)) && !_isDrop)
        {
            LeanTween.move(gameObject, _originalPos + new Vector3(0,0.1f,0), 3f).setOnComplete(() => { _isDrop = false; });
            LeanTween.rotate(gameObject, _originalRot, 3f);
            _attachedRigidbody.velocity = Vector3.zero;
            _isDrop = true;           
        }
    }
}
