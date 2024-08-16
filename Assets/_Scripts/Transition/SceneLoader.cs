using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

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
    public static bool LoadingScene { get; private set; } = false;

    public static Action OnSceneTransition;
    public static Action OnSceneFinishTransition;

    public string SceneName { get => SceneManager.GetActiveScene().name; }

    public void LoadMenuScene(string sceneName)
    {
        LoadScene(sceneName, TransitionHandler.TransitionType.SideSwipe);
    }
    public void LoadMenuScene()
    {
        LoadMenuScene("MainMenu");
    }

    // Loads a new scene with a transition
    public void LoadScene(string sceneName, TransitionHandler.TransitionType type = TransitionHandler.TransitionType.Regular)
    {
        if (LoadingScene)
        {
            return;
        }
        LoadingScene = true;
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
            // TimeManager.PauseGame(false);
            TransitionHandler.Instance.FinishSceneTransition();

            OnSceneFinishTransition?.Invoke();
        };
    }
}
