using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        #region
        public static GameManager Instance { get; private set; }
        #endregion

        #region Variables
        private EventManager _eventManager;
        private ParticleManager _particleManager;
        private PoolManager _poolManager;
        private SaveManager _saveManager;
        #endregion

        #region Properties

        #endregion

        private void Awake()
        {
            Instance = this;
        }

        private void StartGame()
        {
            EventManager.TriggerEvent(EventKeys.OnGameStarted);
        }

        private void EndGame()
        {
            EventManager.TriggerEvent(EventKeys.OnGameEnded);
        }

        private void PauseGame()
        {
            EventManager.TriggerEvent(EventKeys.OnGamePaused);
        }

        private void ResumeGame()
        {
            EventManager.TriggerEvent(EventKeys.OnGameResumed);
        }
    }
}