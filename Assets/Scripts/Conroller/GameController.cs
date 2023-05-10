using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.StartListening(EventKeys.OnGameStarted, Init);
        EventManager.StartListening(EventKeys.OnGamePaused, OnGamePaused);
        EventManager.StartListening(EventKeys.OnGameResumed, OnGameResumed);
        EventManager.StartListening(EventKeys.LevelLoaded, OnLevelLoaded);
        EventManager.StartListening(EventKeys.TowerCompleted, OnTowerCompleted);
        EventManager.StartListening(EventKeys.OnPlayerUnitCountChange, OnPlayerUnitChange);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventKeys.OnGameStarted, Init);
        EventManager.StopListening(EventKeys.OnGamePaused, OnGamePaused);
        EventManager.StopListening(EventKeys.OnGameResumed, OnGameResumed);
        EventManager.StopListening(EventKeys.LevelLoaded, OnLevelLoaded);
        EventManager.StopListening(EventKeys.TowerCompleted, OnTowerCompleted);
        EventManager.StopListening(EventKeys.OnPlayerUnitCountChange, OnPlayerUnitChange);
    }

    private void Init(object[] __)
    {
        Time.timeScale = 1.0f;
    }

    private void OnGamePaused(object[] __)
    {
        Time.timeScale = 0f;
    }

    private void OnGameResumed(object[] __)
    {
        Time.timeScale = 1.0f;
    }

    private void OnLevelLoaded(object[] __)
    {
        Time.timeScale = 1.0f;
    }

    private void OnPlayerUnitChange(object[] obj)
    {
        var count = (int)obj[0];
        if (count <= 0)
        {
            EventManager.TriggerEvent(EventKeys.LevelFailed, null);
            Time.timeScale = 0;
        }
    }

    private void OnTowerCompleted(object[] __)
    {
        this.DelayedAction(4f, () =>
        {
            EventManager.TriggerEvent(EventKeys.LevelCompleted, null);
            Time.timeScale = 0f;
        }, out _);
    }
}
