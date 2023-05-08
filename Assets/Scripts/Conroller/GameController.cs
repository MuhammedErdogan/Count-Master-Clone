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
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventKeys.OnGameStarted, Init);
        EventManager.StopListening(EventKeys.OnPlayerUnitCountChange, OnPlayerUnitDestroyed);
    }

    private void Init(object[] obj)
    {
        
    }

    private void OnPlayerUnitDestroyed(object[] obj)
    {
        var count = (int)obj[0];
        if(count == 0)
        {
            EventManager.TriggerEvent(EventKeys.LevelFailed, null);
        } 
    }

    private void OnLevelCompleted(object[] obj)
    {
        EventManager.TriggerEvent(EventKeys.LevelCompleted, null);
    }
}
