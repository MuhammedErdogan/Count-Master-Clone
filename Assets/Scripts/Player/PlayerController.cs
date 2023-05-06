using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Singleton
        public static PlayerController Instance { get; private set; }
        #endregion

        #region Variables
        [Range(0f, 1f)][SerializeField] private float _distanceFactor, _radius;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private const float GRAVITY = 10f;
        #endregion

        #region lists
        [SerializeField] private List<PlayerUnit> _units = new List<PlayerUnit>();
        #endregion

        #region Properties
        public float Speed => _speed;
        public float JumpForce => _jumpForce;
        public float Gravity => GRAVITY;
        public List<PlayerUnit> PlayerUnits => _units;
        #endregion

        #region Action
        private Action MoveAction;
        #endregion

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            MoveAction?.Invoke();
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventKeys.OnPlayerUnitSpawned, AddUnit);
            EventManager.StartListening(EventKeys.OnPlayerUnitDestroyed, RemoveUnit);
            EventManager.StartListening(EventKeys.OnEnemyContact, EnemyContact);
            EventManager.StartListening(EventKeys.OnGateContactEnter, GateAnalyser);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventKeys.OnPlayerUnitSpawned, AddUnit);
            EventManager.StopListening(EventKeys.OnPlayerUnitDestroyed, RemoveUnit);
            EventManager.StopListening(EventKeys.OnEnemyContact, EnemyContact);
            EventManager.StopListening(EventKeys.OnGateContactEnter, GateAnalyser);
        }

        private void Init()
        {
            //TO DO: Init playerUnits with using object pooling
            MoveAction += Move;
        }

        private void GateAnalyser(object[] objects)
        {
            //TO DO: Analyse gate and add or substract given value
        }

        private void AddUnit(int count)
        {
            //TO DO: Add unit to playerUnits with using object pooling

            ReformatUnits();
        }

        private void AddUnit(object[] objects)
        {
            PlayerUnit playerUnit = objects[0] as PlayerUnit;
            if (playerUnit == null)
            {
                Debug.LogError("PlayerUnit is null");
                return;
            }
            _units.Add(playerUnit);
            ReformatUnits();
        }

        private void RemoveUnit(int count)
        {
            if (_units.Count == 0)
            {
                Debug.LogError("PlayerUnits is empty");
                return;
            }

            if (_units.Count < count)
            {
                Debug.LogError("PlayerUnits count is less than count");
                _units.Clear();
                return;
            }

            if (_units.Count > count * 2)
            {
                ReformatUnits();
            }

            _units.RemoveRange(_units.Count - count, count);
        }

        private void RemoveUnit(object[] objects)
        {
            PlayerUnit playerUnit = objects[0] as PlayerUnit;
            if (playerUnit == null || _units.Count == 0)
            {
                Debug.LogError("PlayerUnit is null or playerUnits is empty");
                return;
            }

            if (!_units.Contains(playerUnit))
            {
                Debug.LogError("PlayerUnit is not in playerUnits");
                return;
            }

            _units.Remove(playerUnit);
            ReformatUnits();
        }

        private void ReformatUnits()
        {
            for (int i = 1; i < _units.Count; i++)
            {
                var x = _distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * _radius);
                var z = _distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * _radius);

                var NewPos = new Vector3(x, 0, z);

                _units[i].transform.DOLocalMove(NewPos, 0.5f).SetEase(Ease.OutBack);
            }
        }

        private void Move()
        {

        }

        private void EnemyContact(object[] objects)
        {

        }
    }
}
