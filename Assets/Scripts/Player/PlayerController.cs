using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using TMPro;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Singleton
        public static PlayerController Instance { get; private set; }
        #endregion

        #region Components
        [SerializeField] private TextMeshPro _scoreText;
        #endregion

        #region Variables
        [Range(0f, 1f)][SerializeField] private float _distanceFactor, _radius;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] PlayerState _state;
        #endregion

        #region lists
        [SerializeField] private List<PlayerUnit> _units = new List<PlayerUnit>();
        #endregion

        #region Properties
        public float JumpForce => _jumpForce;
        public List<PlayerUnit> PlayerUnits => _units;
        private void UpdateCountText(int count) => _scoreText.text = count.ToString();

        #endregion

        #region Action
        public Action EnemyAction;
        #endregion

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            EnemyAction?.Invoke();
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventKeys.LevelLoaded, Init);
            EventManager.StartListening(EventKeys.PlayerOnEnemyContact, EnemyContact);
            EventManager.StartListening(EventKeys.EnemyContactEnded, EnemyContactEnded);
            EventManager.StartListening(EventKeys.OnGateContactEnter, GateAnalyser);
            EventManager.StartListening(EventKeys.OnPlayerUnitHit, RemoveUnit);
            EventManager.StartListening(EventKeys.FinishTriggered, MakeHumanTower);
            EventManager.StartListening(EventKeys.OnPlayerUnitFall, RemoveUnit);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventKeys.LevelLoaded, Init);
            EventManager.StopListening(EventKeys.PlayerOnEnemyContact, EnemyContact);
            EventManager.StopListening(EventKeys.OnGateContactEnter, GateAnalyser);
            EventManager.StopListening(EventKeys.EnemyContactEnded, EnemyContactEnded);
            EventManager.StopListening(EventKeys.OnPlayerUnitHit, RemoveUnit);
            EventManager.StopListening(EventKeys.FinishTriggered, MakeHumanTower);
            EventManager.StopListening(EventKeys.OnPlayerUnitFall, RemoveUnit);
        }

        private void Init(object[] objects)
        {
            var startPos = objects[1] as Transform;
            transform.position = startPos.position;
            _state = PlayerState.Run;

            for (int i = 0; i < _units.Count; i++)
            {
                _units[i].DestroyUnit();
            }

            _units.Clear();
            AddUnit(1);

            UpdateCountText(_units.Count);

            for (int i = 0; i < _units.Count; i++)
            {
                _units[i].Init(_state);
            }
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
                unit.name = $"PlayerUnit_{_units.Count}";
                PlayerUnit unitComponent = unit.GetComponent<PlayerUnit>();
                unitComponent.Init(_state);
                _units.Add(unitComponent);
            }

            ReformatUnits();

            UpdateCountText(_units.Count);
        }

        private void RemoveUnit(int count)
        {
            if (_units.Count == 0 || _units == null)
            {
                EventManager.TriggerEvent(EventKeys.OnPlayerUnitCountChange, new object[] { 0 });
                return;
            }

            if (_units.Count < count)
            {
                EventManager.TriggerEvent(EventKeys.OnPlayerUnitCountChange, new object[] { 0 });
                _units.Clear();
                return;
            }

            Debug.Log($"RemoveUnit count: {count}");

            var unitsWillBeDestroyed = _units.GetRange(_units.Count - count, count);
            _units.RemoveRange(_units.Count - count, count);

            for (var i = unitsWillBeDestroyed.Count - 1; i >= 0; i--)
            {
                var unit = unitsWillBeDestroyed[i];
                unit.DestroyUnit();
            }

            EventManager.TriggerEvent(EventKeys.OnPlayerUnitCountChange, new object[] { _units.Count });

            ReformatUnits();

            UpdateCountText(_units.Count);
        }

        private void RemoveUnit(object[] objects)
        {
            var unit = objects[0] as PlayerUnit;
            if (unit == null || _units.Count == 0)
            {
                EventManager.TriggerEvent(EventKeys.OnPlayerUnitCountChange, new object[] { 0 });
                return;
            }

            if (!_units.Contains(unit))
            {
                Debug.LogError("Unit is not in playerUnits");
                return;
            }

            EventManager.TriggerEvent(EventKeys.OnPlayerUnitCountChange, new object[] { _units.Count });

            _units.Remove(unit);

            UpdateCountText(_units.Count);
        }

        private void ReformatUnits()
        {
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
                _units[i].transform.DORotate(new Vector3(0, 0, 0), 0.75f).SetEase(Ease.OutBack);
            }

        }

        private void EnemyContact(object[] obj)
        {
            _state = PlayerState.Attack;

            for (int i = 0; i < _units.Count; i++)
            {
                _units[i].ChangeState(_state);
            }

            var enemyUnitList = obj[2] as List<EnemyUnit>;
            EnemyAction = () =>
            {
                if (_units.Count == 0)
                {
                    EnemyAction = null;
                    return;
                }

                for (int i = 0; i < _units.Count; i++)
                {
                    _units[i].MoveToEnemy(enemyUnitList[0].transform.position);
                }
            };
        }

        private void EnemyContactEnded(object[] obj)
        {
            EnemyAction = null;
            _state = PlayerState.Run;

            ReformatUnits();
        }

        private void MakeHumanTower(object[] obj)
        {
            var tower = gameObject.GetComponent<HumanTower>();
            tower.MakeTower(_units.Count);
        }
    }
}
