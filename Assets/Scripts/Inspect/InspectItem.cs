using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class InspectItem : MonoBehaviour
{
    public InputActionReference rightGripInputActionPosition;
    public InputActionReference rightGripInputActionValue;
    public InputActionReference leftGripInputActionPosition;
    public InputActionReference leftGripInputActionValue;
    public float maxSize;
    public float minSize;
    public float rotationSpeed;
    public float scaleFactor;
    private Transform _mainCamera;

    private bool _isBothPress = false;
    private bool _isRightPress = false;
    private bool _isLeftPress = false;
    private Vector3 _rightPreviousMousePosition;
    private Vector3 _leftPreviousMousePosition;
    private float _previousHandDistance;
    private Vector3 _originalPosition;
    private Vector3 _previousSize;
    private Vector3 _originalRotation;
    private Vector3 _originalSize;

    private void Awake()
    {
        _mainCamera = Camera.main.transform;
    }
    private void OnEnable()
    {

        _originalPosition = transform.position;
        _originalRotation = transform.rotation.eulerAngles;
        _originalSize = transform.localScale;
    }

    void Update()
    {
        if (rightGripInputActionValue.action.IsPressed() && leftGripInputActionValue.action.IsPressed())
        {
            _isRightPress = false;
            _isLeftPress = false;

            if (!_isBothPress)
            {
                _previousHandDistance = Vector3.Distance(rightGripInputActionPosition.action.ReadValue<Vector3>(), leftGripInputActionPosition.action.ReadValue<Vector3>());
                _previousSize = transform.localScale;
                _isBothPress = true;              
            }
            if (_isBothPress)
            {
                float currentHandDistance = Vector3.Distance(rightGripInputActionPosition.action.ReadValue<Vector3>(), leftGripInputActionPosition.action.ReadValue<Vector3>());
                float deltaControllerDistance = currentHandDistance - _previousHandDistance;

                Vector3 size = transform.localScale;
                size.x = _previousSize.x + (deltaControllerDistance) * scaleFactor;
                size.y = _previousSize.y + (deltaControllerDistance) * scaleFactor;
                size.z = _previousSize.z + (deltaControllerDistance) * scaleFactor;
                CheckSize(size);
            }
        }
        else
        {
            _isBothPress = false;
            if (rightGripInputActionValue.action.IsPressed() && !_isRightPress)
            {
                _rightPreviousMousePosition = rightGripInputActionPosition.action.ReadValue<Vector3>();
                _isRightPress = true;
            }

            else if (!rightGripInputActionValue.action.IsPressed())
            {
                _isRightPress = false;
            }

            if (_isRightPress)
            {
                Vector3 deltaControllerPosition = rightGripInputActionPosition.action.ReadValue<Vector3>() - _rightPreviousMousePosition;
                float rotationX = deltaControllerPosition.y * (rotationSpeed * 1000) * Time.deltaTime;
                float rotationY = -deltaControllerPosition.x * (rotationSpeed * 1000) * Time.deltaTime;
               
                Quaternion rotation = Quaternion.Euler(-rotationX, rotationY, 0);
                transform.localRotation = rotation * transform.localRotation;

                _rightPreviousMousePosition = rightGripInputActionPosition.action.ReadValue<Vector3>();
            }

            if (leftGripInputActionValue.action.IsPressed() && !_isLeftPress)
            {
                _leftPreviousMousePosition = leftGripInputActionPosition.action.ReadValue<Vector3>();
                _isLeftPress = true;
            }

            else if (!leftGripInputActionValue.action.IsPressed())
            {
                _isLeftPress = false;
            }

            if (_isLeftPress)
            {
                Vector3 deltaControllerPosition = leftGripInputActionPosition.action.ReadValue<Vector3>() - _leftPreviousMousePosition;
                float rotationX = deltaControllerPosition.y * (rotationSpeed * 1000) * Time.deltaTime;
                float rotationY = -deltaControllerPosition.x * (rotationSpeed * 1000) * Time.deltaTime;

                Quaternion rotation = Quaternion.Euler(-rotationX, rotationY, 0);
                transform.localRotation = rotation * transform.localRotation;

                _leftPreviousMousePosition = leftGripInputActionPosition.action.ReadValue<Vector3>();
            }
        }
    }
    public void CheckSize(Vector3 addSize)
    {
        if(addSize.x > maxSize)
        {

        }else if (addSize.x < minSize)
        {

        }
        else
        {
            transform.localScale = addSize;
        }
    }

    public void ResetView()
    {
        LeanTween.move(gameObject, _originalPosition, 1);
        LeanTween.rotate(gameObject, _originalRotation, 1);
        LeanTween.scale(gameObject, _originalSize, 1);
    }
}

