using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModel : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform _playerStartPos;
    [SerializeField] private FinishType _finishType;
    #endregion

    #region Properties
    public Transform PlayerStartPos => _playerStartPos;
    public FinishType FinishType => _finishType;
    #endregion

    public void SetupLevel()
    {
        gameObject.SetActive(true);
    }

    public void CloseLevel()
    {
        gameObject.SetActive(false);
    }
}
