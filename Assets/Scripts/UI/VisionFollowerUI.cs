using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VisionFollowerUI : MonoBehaviour
{
    public Transform target;
    public float distance;
    public Vector3 offsetPosition;

    private Vector3 _velocity = Vector3.zero;
    private CanvasGroup _canvasGroup;
    private bool _enabled = false;
    private bool _isVisible = false;

    private void OnBecameInvisible()
    {
        _isVisible = false;
    }

    private void OnEnable()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Invoke("ChangeEnable", 1);
    }
 
    private void Update()
    { 
        if (_enabled) {
            if (!_isVisible)
            {
                transform.LookAt(transform.position - (target.position - transform.position));
                Vector3 targetPosition = FindTargetPosition();
                MoveToward(targetPosition);
                if (ReachedPosition(targetPosition))
                {
                    _isVisible = true;
                }
            }
        }
    }

    private Vector3 FindTargetPosition()
    {
        return target.position + (new Vector3(target.forward.x,0, target.forward.z) * distance) + offsetPosition;
    }

    private void ChangeEnable()
    {
        _enabled = true;
        LeanTween.alphaCanvas(_canvasGroup, 1, 1f);
    }

    private void MoveToward(Vector3 targetPosition)
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, 1f);
    }

    private bool ReachedPosition(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, transform.position) < 0.3f ;
    }

    private void OnDisable()
    {
        _enabled = false;
        LeanTween.alphaCanvas(_canvasGroup, 0, 0.5f).setOnComplete(() => {
            gameObject.SetActive(false);
        });
    }
}
