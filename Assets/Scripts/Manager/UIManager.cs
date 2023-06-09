using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private GameObject startPanel, gamePanel, pausePanel, gameOverPanel, victoryPanel;
        [SerializeField] TextMeshProUGUI _tmPro;
        #endregion

        private void OnEnable()
        {
            EventManager.StartListening(EventKeys.LevelLoaded, OnGameStarted);
            EventManager.StartListening(EventKeys.OnGamePaused, OnGamePaused);
            EventManager.StartListening(EventKeys.OnGameResumed, OnGameResumed);
            EventManager.StartListening(EventKeys.LevelCompleted, OnLevelCompleted);
            EventManager.StartListening(EventKeys.LevelFailed, OnLevelFailed);
            EventManager.StartListening(EventKeys.LevelLoaded, OnGameStarted);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventKeys.LevelLoaded, OnGameStarted);
            EventManager.StopListening(EventKeys.OnGamePaused, OnGamePaused);
            EventManager.StopListening(EventKeys.OnGameResumed, OnGameResumed);
            EventManager.StopListening(EventKeys.LevelCompleted, OnLevelCompleted);
            EventManager.StopListening(EventKeys.LevelFailed, OnLevelFailed);
            EventManager.StopListening(EventKeys.LevelLoaded, OnGameStarted);
        }

        private void OnGameStarted(object[] obj)
        {
            _tmPro.text = $"LEVEL: {(int)obj[0] + 1}";
            startPanel.SetActive(false);
            gamePanel.SetActive(true);
            pausePanel.SetActive(false);
            gameOverPanel.SetActive(false);
            victoryPanel.SetActive(false);
        }

        private void OnGamePaused(object[] buttonType)
        {
            startPanel.SetActive(false);
            gamePanel.SetActive(false);
            pausePanel.SetActive(true);
            gameOverPanel.SetActive(false);
            victoryPanel.SetActive(false);
        }

        private void OnGameResumed(object[] buttonType)
        {
            startPanel.SetActive(false);
            gamePanel.SetActive(true);
            pausePanel.SetActive(false);
            gameOverPanel.SetActive(false);
            victoryPanel.SetActive(false);
        }

        private void OnLevelFailed(object[] buttonType)
        {
            startPanel.SetActive(false);
            gamePanel.SetActive(false);
            pausePanel.SetActive(false);
            gameOverPanel.SetActive(true);
            victoryPanel.SetActive(false);
        }

        private void OnLevelCompleted(object[] buttonType)
        {
            startPanel.SetActive(false);
            gamePanel.SetActive(false);
            pausePanel.SetActive(false);
            gameOverPanel.SetActive(false);
            victoryPanel.SetActive(true);
        }
    }
}
