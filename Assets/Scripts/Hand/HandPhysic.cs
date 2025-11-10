using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPhysic : MonoBehaviour
{
    public Transform target;
    private Rigidbody _rb;
    private Collider[] _handColliders;
    public Vector3 rotationOffset;
    [HideInInspector]
    public bool isKnob = false;

    //membuat sebuah renderer ketika tangan dihalangi oleh collider lain
    public Renderer nonPhysicalHand;
    public float nonPhysicalHandDistance = 0.05f;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        //mengambil seluruh collider pada hand
        _handColliders = GetComponentsInChildren<Collider>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        //menampilkan non physic hand
        if (distance > nonPhysicalHandDistance && !isKnob) { 
            nonPhysicalHand.enabled = true;
        }
        else
        {
            nonPhysicalHand.enabled = false;
        }
    }

    void FixedUpdate()
    {
        //membuat physic dan model tangan mengikuti gerak posisi dan rotasi controller
        //hand position
        _rb.velocity = (target.position - transform.position) / Time.fixedDeltaTime;
        //hand rotation
        var rotationWithOffset = target.rotation * Quaternion.Euler(rotationOffset);
        Quaternion rotationDifference = rotationWithOffset * Quaternion.Inverse(transform.rotation);
        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

        Vector3 rotationDifferenceInDegree = angleInDegree * rotationAxis;

        _rb.angularVelocity = (rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);   
    }

    public void EnableHandColliders() {
        foreach (var item in _handColliders) {
            item.enabled = true;
        }
    }

    public void EnableHandCollidersDelay(float delay)
    {
        Invoke("EnableHandColliders", delay);
    }

    public void DisableHandColliders()
    {
        foreach (var item in _handColliders)
        {
            item.enabled = false;
        }
    }
}
