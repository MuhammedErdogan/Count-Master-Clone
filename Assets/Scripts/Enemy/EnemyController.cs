using DG.Tweening;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [Range(0f, 1f)][SerializeField] private float _distanceFactor, _radius;
        [SerializeField] private int enemyCount = 10;
        [SerializeField] private EnemyUnit enemyUnitPrefab;
        [SerializeField] private List<EnemyUnit> _units = new List<EnemyUnit>();

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
            for (int i = 0; i < enemyCount; i++)
            {
                EnemyUnit unit = Instantiate(enemyUnitPrefab, transform);
                _units.Add(unit);
            }
        }

        private void OnAttack()
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

                _units[i].transform.position = NewPos;
            }
        }
    }
}