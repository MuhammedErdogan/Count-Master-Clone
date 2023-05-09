using DG.Tweening;
using Interface;
using Manager;
using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour, IContactable
    {
        [Range(0f, 1f)][SerializeField] private float _distanceFactor, _radius;
        [SerializeField] private GameObject _enemyZoneIndicator;
        [SerializeField] private int _enemyCount = 10;
        [SerializeField] private EnemyUnit _enemyUnitPrefab;
        [SerializeField] private List<EnemyUnit> _units = new List<EnemyUnit>();

        private Action AttackAction;

        private void OnEnable()
        {
            EventManager.StartListening(EventKeys.OnGameStarted, Init);
            EventManager.StartListening(EventKeys.OnPlayerUnitHit, RemoveUnit);
            EventManager.StartListening(EventKeys.PlayerOnEnemyContact, OnAttack);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventKeys.OnGameStarted, Init);
            EventManager.StopListening(EventKeys.PlayerOnEnemyContact, OnAttack);
            EventManager.StopListening(EventKeys.OnPlayerUnitHit, RemoveUnit);
        }

        private void Update()
        {
            AttackAction?.Invoke();
        }

        private void Init(object[] obj)
        {
            for (int i = 0; i < _enemyCount; i++)
            {
                EnemyUnit unit = Instantiate(_enemyUnitPrefab, transform);
                _units.Add(unit);
            }
            ReformatUnits();
        }

        private void OnAttack(object[] obj)
        {
            GameObject go = (GameObject)obj[0];
            var pos = go.transform.position;
            AttackAction = () =>
            {
                for (int i = 0; i < _units.Count; i++)
                {
                    EnemyUnit unit = _units[i];
                    var playerUnits = go.GetComponent<PlayerController>().PlayerUnits;
                    unit.MoveToPlayer(pos, playerUnits[playerUnits.Count - 1].transform.position);
                }
            };
            for (int i = 0; i < _units.Count; i++)
            {
                EnemyUnit unit = _units[i];
                unit.Attack();
            }
        }

        private void StopAttack()
        {
            for (int i = 0; i < _units.Count; i++)
            {
                EnemyUnit unit = _units[i];
                unit.StopAttack();
            }
        }

        private void RemoveUnit(object[] obj)
        {
            var col = obj[1] as Collider;
            var unit = col.GetComponent<EnemyUnit>();

            if (!_units.Contains(unit))
            {
                return;
            }

            _units.Remove(unit);
            Destroy(unit.gameObject);

            if (_units.Count == 0)
            {
                EnemyZoneCleared();
            }
        }

        private void EnemyZoneCleared()
        {
            EventManager.TriggerEvent(EventKeys.EnemyContactEnded, new object[] { this });

            StopAttack();
            GetComponent<Collider>().enabled = false;
            _enemyZoneIndicator.transform.DOScale(Vector3.zero, 0.5f).
                OnComplete(() => gameObject.SetActive(false));
        }

        private void ReformatUnits()
        {
            for (int i = 1; i <= _units.Count; i++)
            {
                var x = _distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * _radius);
                var z = _distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * _radius);

                var NewPos = new Vector3(x, 0, z);

                _units[i - 1].transform.localPosition = NewPos;
                _units[i - 1].transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }

        public void OnContactEnter(GameObject other, Vector3 point)
        {
            EventManager.TriggerEvent(EventKeys.PlayerOnEnemyContact, new object[] { other, this, _units });
        }

        public void OnContactExit(GameObject other, Vector3 point)
        {
            EnemyZoneCleared();
        }
    }
}