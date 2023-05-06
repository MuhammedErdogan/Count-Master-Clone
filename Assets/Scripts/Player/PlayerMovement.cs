using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float _speed = 10.0f;
    public float _swerveAmount = 0.5f;
    public float _swerveSpeed = 5.0f;
    public float _smoothTime = 0.1f;
    private Rigidbody _rb;
    private Vector3 _startPosition;
    private float _screenWidth;
    private bool _isGameStarted = false;
    private float _currentVelocity;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _screenWidth = Screen.width;
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventKeys.OnGameStarted, Init);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventKeys.OnGameStarted, Init);
    }

    private void Init(object[] obj)
    {
        _isGameStarted = true;
    }

    void Update()
    {
        if (!_isGameStarted)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _startPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            float movementDirection = (Input.mousePosition.x - _startPosition.x) / _screenWidth * 2.0f;
            float targetPositionX = Mathf.Clamp(transform.position.x + movementDirection, -_swerveAmount, _swerveAmount);
            float smoothPositionX = Mathf.SmoothDamp(transform.position.x, targetPositionX, ref _currentVelocity, _smoothTime, _swerveSpeed);
            Vector3 newPosition = new Vector3(smoothPositionX, transform.position.y, transform.position.z);
            _rb.MovePosition(newPosition);
        }
    }

    void FixedUpdate()
    {
        if (!_isGameStarted)
            return;

        _rb.velocity = new Vector3(0, 0, _speed);
    }
}
