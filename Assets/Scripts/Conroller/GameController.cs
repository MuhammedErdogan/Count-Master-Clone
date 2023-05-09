using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.StartListening(EventKeys.OnGameStarted, Init);
        EventManager.StartListening(EventKeys.OnPlayerUnitCountChange, OnPlayerUnitDestroyed);
        EventManager.StartListening(EventKeys.FinishTriggered, OnLevelCompleted);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventKeys.OnGameStarted, Init);
        EventManager.StopListening(EventKeys.OnPlayerUnitCountChange, OnPlayerUnitDestroyed);
        EventManager.StopListening(EventKeys.FinishTriggered, OnLevelCompleted);
    }

    private void Init(object[] obj)
    {
        Time.timeScale = 1.0f;
    }

    private void OnPlayerUnitDestroyed(object[] obj)
    {
        var count = (int)obj[0];
        if (count == 0)
        {
            EventManager.TriggerEvent(EventKeys.LevelFailed, null);
        }
    }

    private void OnLevelCompleted(object[] obj)
    {
        this.DelayedAction(5f, () =>
        {
            EventManager.TriggerEvent(EventKeys.LevelCompleted, null);
            Time.timeScale = 0f;
        }, out _);
    }
}
