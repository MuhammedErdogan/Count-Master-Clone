using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace Player
{
    public class HumanTower : MonoBehaviour
    {
        [Header("Configuration")]
        [Range(5f, 10f)][SerializeField] private int _maxUnitPerRow;
        [Range(0f, 2f)][SerializeField] private float xGap;
        [Range(0f, 2f)][SerializeField] private float yGap;
        [Range(0f, 10f)][SerializeField] private float yOffset;

        [Header("Runtime")]
        [SerializeField] private List<int> _towerCountList;
        [SerializeField] private List<GameObject> _towerList;

        private int _unitCount;

        public void MakeTower(int unitCount)
        {
            _unitCount = unitCount;
            FillTowerList();
            StartCoroutine(BuildTowerCoroutine());
        }

        private void FillTowerList()
        {
            _towerCountList = new List<int>();
            _towerList = new List<GameObject>();

            CalculateTowerCounts();
            AdjustPlayerAmount();
        }

        private void CalculateTowerCounts()
        {
            for (int i = 1; i <= _maxUnitPerRow; i++)
            {
                if (_unitCount < i)
                {
                    break;
                }
                _unitCount -= i;
                _towerCountList.Add(i);
            }
        }

        private void AdjustPlayerAmount()
        {
            for (int i = _maxUnitPerRow; i > 0; i--)
            {
                if (_unitCount >= i)
                {
                    _unitCount -= i;
                    _towerCountList.Add(i);
                    i++;
                }
            }
        }

        private IEnumerator BuildTowerCoroutine()
        {
            var towerId = 0;
            transform.DOMoveX(0f, 0.5f).SetEase(Ease.Flash);

            yield return new WaitForSecondsRealtime(0.55f);

            foreach (int towerHumanCount in _towerCountList)
            {
                MoveTowerListChildrenUpwards();

                GameObject tower = CreateNewTower(towerId);

                _towerList.Add(tower);

                PositionTowerChildren(tower, towerHumanCount);

                towerId++;
                yield return new WaitForSecondsRealtime(0.2f);
            }
        }

        private void MoveTowerListChildrenUpwards()
        {
            for (int i = 0; i < _towerList.Count; i++)
            {
                GameObject child = _towerList[i];
                child.transform.DOLocalMove(child.transform.localPosition + new Vector3(0, yGap, 0), 0.2f).SetEase(Ease.OutQuad);
            }
        }

        private GameObject CreateNewTower(int towerId)
        {
            var tower = new GameObject("Tower" + towerId);
            tower.transform.parent = transform;
            tower.transform.localPosition = new Vector3(0, 0, 0);
            return tower;
        }

        private void PositionTowerChildren(GameObject tower, int towerHumanCount)
        {
            float tempTowerHumanCount = 0;
            var towerNewPos = Vector3.zero;

            for (int i = 1; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.transform.parent = tower.transform;
                child.transform.localPosition = new Vector3(tempTowerHumanCount * xGap, 0, 0);
                child.eulerAngles = Vector3.zero;
                towerNewPos += child.transform.position;
                tempTowerHumanCount++;
                i--;

                if (tempTowerHumanCount >= towerHumanCount)
                {
                    break;
                }
            }

            tower.transform.position = new Vector3(-towerNewPos.x / towerHumanCount, tower.transform.position.y - yOffset, tower.transform.position.z);
        }
    }
}

