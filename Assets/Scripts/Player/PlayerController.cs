using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Singleton
        public static PlayerController Instance { get; private set; }
        #endregion

        #region Components
        private Rigidbody _rb;
        #endregion

        #region Variables
        [Range(0f, 1f)][SerializeField] private float _distanceFactor, _radius;
        [SerializeField] private float _jumpForce = 5f;
        private Vector3 _startPosition;
        #endregion

        #region lists
        [SerializeField] private List<PlayerUnit> _units = new List<PlayerUnit>();
        #endregion

        #region Properties
        public float JumpForce => _jumpForce;
        public List<PlayerUnit> PlayerUnits => _units;
        #endregion

        #region Action
        #endregion

        private void Awake()
        {
            Instance = this;
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventKeys.OnGameStarted, Init);
            EventManager.StartListening(EventKeys.OnPlayerUnitSpawned, AddUnit);
            EventManager.StartListening(EventKeys.OnPlayerUnitDestroyed, RemoveUnit);
            EventManager.StartListening(EventKeys.OnEnemyContact, EnemyContact);
            EventManager.StartListening(EventKeys.OnGateContactEnter, GateAnalyser);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventKeys.OnGameStarted, Init);
            EventManager.StopListening(EventKeys.OnPlayerUnitSpawned, AddUnit);
            EventManager.StopListening(EventKeys.OnPlayerUnitDestroyed, RemoveUnit);
            EventManager.StopListening(EventKeys.OnEnemyContact, EnemyContact);
            EventManager.StopListening(EventKeys.OnGateContactEnter, GateAnalyser);
        }

        private void Init(object[] objects)
        {
            //TO DO: Init playerUnits with using object pooling
            //MoveAction = Move;
            Debug.Log("PlayerController Init");
        }

        private void GateAnalyser(object[] objects)
        {
            var value = (int)objects[2];
            var operation = (Operations)objects[3];

            Debug.Log($"GateAnalyser value: {value} operation: {operation}");

            switch (operation)
            {
                case Operations.Add:
                    AddUnit(value);
                    break;
                case Operations.Subtract:
                    RemoveUnit(value);
                    break;
                case Operations.Multiply:
                    AddUnit(_units.Count * (value - 1));
                    break;
                case Operations.Divide:
                    RemoveUnit(Mathf.FloorToInt(_units.Count / value));
                    break;
                default:
                    break;
            }
        }

        private void AddUnit(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var unit = PoolManager.Instance.SpawnFromPool(PoolType.PlayerUnit, transform.position, Quaternion.identity);
                unit.transform.SetParent(transform);
                _units.Add(unit.GetComponent<PlayerUnit>());
            }

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

            Debug.Log($"RemoveUnit count: {count}");

            var unitsWillBeDestroyed = _units.GetRange(_units.Count - count, count);
            _units.RemoveRange(_units.Count - count, count);

            for (var i = unitsWillBeDestroyed.Count - 1; i >= 0; i--)
            {
                PlayerUnit unit = unitsWillBeDestroyed[i];
                unit.DestroyUnit();
            }

            ReformatUnits();
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
            Debug.Log($"ReformatUnits count: {_units.Count}");
            if (_units.Count == 1)
            {
                _units[0].transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutBack);
                return;
            }

            for (int i = 1; i < _units.Count; i++)
            {
                var x = _distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * _radius);
                var z = _distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * _radius);

                var NewPos = new Vector3(x, 0, z);

                _units[i].transform.DOLocalMove(NewPos, 0.75f).SetEase(Ease.OutBack);
            }
        }

        private void EnemyContact(object[] objects)
        {

        }
    }
}
