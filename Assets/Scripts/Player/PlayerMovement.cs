using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
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

        private float _currentSpeed;
        private bool _isEnemyContact = false;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _screenWidth = Screen.width;
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventKeys.OnGameStarted, Init);
            EventManager.StartListening(EventKeys.PlayerOnEnemyContact, EnemyMovementStarted);
            EventManager.StartListening(EventKeys.EnemyContactEnded, EnemyMovementended);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventKeys.OnGameStarted, Init);
            EventManager.StopListening(EventKeys.PlayerOnEnemyContact, EnemyMovementStarted);
            EventManager.StopListening(EventKeys.EnemyContactEnded, EnemyMovementended);
        }

        private void Init(object[] obj)
        {
            _isGameStarted = true;
            _currentSpeed = _speed;
        }

        void Update()
        {
            if (!_isGameStarted || _isEnemyContact)
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

            _rb.velocity = new Vector3(0, 0, _currentSpeed);
        }

        private void EnemyMovementStarted(object[] obj)
        {
            _currentSpeed = 1;
            _isEnemyContact = true;
        }

        private void EnemyMovementended(object[] obj)
        {
            _currentSpeed = _speed;
            _isEnemyContact = false;
        }
    }
}
