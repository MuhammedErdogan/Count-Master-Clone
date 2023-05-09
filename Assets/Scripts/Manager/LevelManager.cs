using Manager;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private List<GameObject> levels = new List<GameObject>();
    [SerializeField] private Transform levelParents;
    private int selectedIndex, currentLevel;
    #endregion

    #region Components
    #endregion

    private void Awake()
    {
        currentLevel = PlayerPrefs.GetInt("LevelIndex", 0);
        selectedIndex = PlayerPrefs.GetInt("LastPlayedIndex", 0);
    }

    private void OnEnable()
    {
        //EventManager.StartListening(EventKeys.OnPlayButtonClicked, GenerateLevel);
        //EventManager.StartListening(EventKeys.OnHomeReturned, DestroyLevels);
    }

    private void OnDisable()
    {
        //EventManager.StopListening(EventKeys.OnPlayButtonClicked, GenerateLevel);
        //EventManager.StopListening(EventKeys.OnHomeReturned, DestroyLevels);
    }

    private void GenerateLevel(object[] obj = null)
    {
        DestroyLevels();
        //var levelScript = Instantiate(levels[selectedIndex], levelParents).GetComponent<LevelModel>();
        //EventManager.TriggerEvent(EventKeys.OnLevelCreated, new object[] { levelScript.GetStartSkateBoard, levelScript.GetStartPos, currentLevel });
        PlayerPrefs.SetInt("LevelIndex", currentLevel);
        PlayerPrefs.SetInt("LastPlayedIndex", selectedIndex);
    }

    private void DestroyLevels(object[] obj = null)
    {
        int childCount = levelParents.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(levelParents.GetChild(0).gameObject);
        }
    }

    public void PassToNextLevel()
    {
        currentLevel++;
        SetLevelIndex();
        GenerateLevel();
    }

    public void ReplayThisLevel()
    {
        GenerateLevel();
    }

    private void SetLevelIndex()
    {
        if (currentLevel < levels.Count)
        {
            selectedIndex = currentLevel;
            return;
        }
        var oldIndex = selectedIndex;
        selectedIndex = Random.Range(0, levels.Count);
        if (oldIndex == selectedIndex)
        {
            selectedIndex = (selectedIndex + 1) % levels.Count;
        }
    }
}