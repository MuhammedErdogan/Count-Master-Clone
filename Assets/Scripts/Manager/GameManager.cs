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

        private void OnEnable()
        {
            EventManager.StartListening(EventKeys.Buttonclicked, OnButtonClicked);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventKeys.Buttonclicked, OnButtonClicked);
        }

        private void OnButtonClicked(object[] buttonType)
        {
            Debug.Log("Button clicked: " + buttonType);

            switch ((ButtonType)buttonType[0])
            {
                case ButtonType.Play:
                    StartGame();
                    break;
                case ButtonType.Resume:
                    ResumeGame();
                    break;
                case ButtonType.Restart:
                    RestartGame();
                    break;
                case ButtonType.Pause:
                    PauseGame();
                    break;
                default:
                    break;
            }
        }

        private void StartGame()
        {
            EventManager.TriggerEvent(EventKeys.OnGameStarted);
        }

        private void RestartGame()
        {
            EventManager.TriggerEvent(EventKeys.OnGameStarted);
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