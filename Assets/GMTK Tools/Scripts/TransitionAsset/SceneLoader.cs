using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class SceneLoader : MonoBehaviour
{
    #region Singleton

    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject, false);
        }
    }

    #endregion
    public static int Level { get; private set; } = 0;
    public static int Area { get; private set; } = 0;

    public static bool LoadingScene { get; private set; } = false;

    public static Action OnSceneTransition;
    public static Action OnSceneFinishTransition;

    public string SceneName { get => SceneManager.GetActiveScene().name; }

    private void Start()
    {
        // Determine the area we're in
        Area = LevelManager.Instance.GetAreaIndex(SceneName);
        if (Area != -1)
        {
            // Then the exact level
            Level = LevelManager.Instance.GetLevelIndex(SceneName, Area);
        }
    }

    public void LoadMenuScene(string sceneName)
    {
        Level = 0;
        Area = 0;
        LoadScene(sceneName, TransitionHandler.TransitionType.SideSwipe);
    }
    public void LoadMenuScene()
    {
        Level = 0;
        Area = 0;
        LoadMenuScene("MainMenu");
    }

    public void ReloadLevel()
    {
        LoadScene(SceneName);
    }

    public void LoadLevel(int index, int area)
    {
        Level = index;
        Area = area;

        LoadScene(LevelManager.Instance.GetLevelSceneName(index, area));
    }

    public void LoadBonusLevel(int index, int area)
    {
        Level = index;
        Area = area;

        LoadScene(LevelManager.Instance.GetBonusLevelSceneName(index, area));
    }

    public void NextLevel()
    {
        Level++;
        if (LevelManager.Instance.GetSceneType() == LevelManager.SceneType.Bonus)
        {
            if (Level < LevelManager.Instance.BonusLevelCount(Area))
            {
                LoadBonusLevel(Level, Area);
            }
            else
            {
                LoadMenuScene();
            }
        }
        else
        {
            if (Level < LevelManager.Instance.LevelCount(Area))
            {
                LoadLevel(Level, Area);
            }
            else
            {
                LoadMenuScene("Level_Select");
            }
        }
    }

    // Loads a new scene with a transition
    public void LoadScene(string sceneName, TransitionHandler.TransitionType type = TransitionHandler.TransitionType.Regular)
    {
        if (LoadingScene)
        {
            return;
        }
        LoadingScene = true;

        // Make sure everything static is in order on the next scene
        if (TimeManager.IsSlomo)
            TimeManager.ToggleSlomo();

        Selectable.DeselectAll();
        OnSceneTransition?.Invoke();

        TransitionHandler.Instance.StartSceneTransition(LoadSceneImmediate, sceneName, type);
    }

    // Loads a new scene without a transition
    public void LoadSceneImmediate(string sceneName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        ao.completed += ctx =>
        {
            LoadingScene = false;
            TimeManager.PauseGame(false);
            TransitionHandler.Instance.FinishSceneTransition();

            OnSceneFinishTransition?.Invoke();
        };
    }
}
