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
        private Vector3 _startPosition;
        private float _screenWidth;
        private float _currentVelocity;

        private float _currentSpeed;
        private bool _isEnemyContact = false;

        Action Action;

        void Start()
        {
            _screenWidth = Screen.width;
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventKeys.LevelLoaded, Init);
            EventManager.StartListening(EventKeys.PlayerOnEnemyContact, EnemyMovementStarted);
            EventManager.StartListening(EventKeys.EnemyContactEnded, EnemyMovementended);
            EventManager.StartListening(EventKeys.FinishTriggered, FinishMovementStarted );
            //EventManager.StartListening(EventKeys.TowerCompleted, FinishMovementEnded);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventKeys.LevelLoaded, Init);
            EventManager.StopListening(EventKeys.PlayerOnEnemyContact, EnemyMovementStarted);
            EventManager.StopListening(EventKeys.EnemyContactEnded, EnemyMovementended);
            EventManager.StopListening(EventKeys.FinishTriggered, FinishMovementStarted);
            //EventManager.StopListening(EventKeys.TowerCompleted, FinishMovementEnded);
        }

        private void Init(object[] obj)
        {
            _currentSpeed = _speed;
            _isEnemyContact = false;

            Action = ForwardMovement;
        }

        void Update()
        {
            Action?.Invoke();
        }

        private void ForwardMovement()
        {
            Vector3 forwardMovement = new Vector3(0, 0, _currentSpeed) * Time.deltaTime;
            transform.position += forwardMovement;

            if (_isEnemyContact)
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
                transform.position = newPosition;
            }
        }

        private void EnemyMovementStarted(object[] obj)
        {
            _currentSpeed = .25f;
            _isEnemyContact = true;
        }

        private void EnemyMovementended(object[] obj)
        {
            _currentSpeed = _speed;
            _isEnemyContact = false;
        }

        private void FinishMovementStarted(object[] obj)
        {
            _currentSpeed = 7;

            Action = () =>
            {
                Vector3 forwardMovement = new Vector3(0, 0, _currentSpeed) * Time.deltaTime;
                transform.position += forwardMovement;

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, transform.position.y, transform.position.z), 10 * Time.deltaTime);
            };
        }

        private void FinishMovementEnded(object[] obj)
        {
            Action = null;
        }
    }
}
