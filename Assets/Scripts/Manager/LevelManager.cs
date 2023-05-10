using UnityEngine;
using System.Collections.Generic;
using Manager;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<LevelModel> levels;
    [SerializeField] private Transform levelParents;

    private LevelModel _activeLevel;
    private int _currentLevel;

    private void Awake()
    {
        _currentLevel = SaveManager.LoadInt(SaveManager.KEY_LEVEL_INDEX);

        for (int i = levels.Count - 1; i >= 0; i--)
        {
            levels[i].CloseLevel();
        }

        _activeLevel = levels[GetLevelIndex()];
        _activeLevel.SetupLevel();
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventKeys.OnGameStarted, LoadLevel);
        EventManager.StartListening(EventKeys.OnNextLevelRequest, NextLevel);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventKeys.OnGameStarted, LoadLevel);
        EventManager.StopListening(EventKeys.OnNextLevelRequest, NextLevel);
    }

    private void LoadLevel(object[] obj = null)
    {
        int index = GetLevelIndex();
        if(_activeLevel != null)
        {
            _activeLevel.gameObject.SetActive(false);
        }

        _activeLevel = levels[index];
        _activeLevel.SetupLevel();

        EventManager.TriggerEvent(EventKeys.LevelLoaded, new object[] { _currentLevel, _activeLevel.PlayerStartPos });
        SaveManager.SaveInt(SaveManager.KEY_LEVEL_INDEX, _currentLevel);
    }

    public void NextLevel(object[] obj = null)
    {
        _currentLevel++;
        GetLevelIndex();
        LoadLevel();
    }

    public void ReplayThisLevel()
    {
        LoadLevel();
    }

    private int GetLevelIndex()
    {
        if (_currentLevel < levels.Count)
        {
            return _currentLevel;
        }

        return Random.Range(0, levels.Count);
    }
}
