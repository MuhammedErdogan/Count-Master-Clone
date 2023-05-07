using DG.Tweening;
using Interface;
using Manager;
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

        private void OnEnable()
        {
            EventManager.StartListening(EventKeys.OnGameStarted, Init);
            EventManager.StartListening(EventKeys.PlayerOnEnemyContact, OnAttack);
            EventManager.StartListening(EventKeys.OnPlayerUnitHit, RemoveUnit);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventKeys.OnGameStarted, Init);
            EventManager.StopListening(EventKeys.PlayerOnEnemyContact, OnAttack);
            EventManager.StopListening(EventKeys.OnPlayerUnitHit, RemoveUnit);
        }

        private void Init(object[] obj)
        {
            for (int i = 0; i < _enemyCount; i++)
            {
                EnemyUnit unit = Instantiate(_enemyUnitPrefab, transform);
                _units.Add(unit);
            }

            _enemyZoneIndicator.transform.localScale = (_enemyCount / 20) * Vector3.one;

            ReformatUnits();
        }

        private void OnAttack(object[] obj)
        {
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
            var unit = (EnemyUnit)obj[0];
            if (!_units.Contains(unit))
            {
                return;
            }

            _units.Remove(unit);

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
            }
        }

        public void OnContactEnter(GameObject other, Vector3 point)
        {
            EventManager.TriggerEvent(EventKeys.PlayerOnEnemyContact, new object[] { other, this });
        }

        public void OnContactExit(GameObject other, Vector3 point)
        {
            EnemyZoneCleared();
        }
    }
}