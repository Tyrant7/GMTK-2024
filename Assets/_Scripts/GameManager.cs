using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    public enum GameState
    {
        NotStarted, DrawPhase, PickPhase, BuildPhase
    }

    public GameState gameState { get; private set; }


    [SerializeField] GameObject environmentPrefab;

    [SerializeField] EnvironmentGenerator generator;

    [ContextMenu("Generate Environment")]
    public void CreateEnvironment()
    {
        generator.GenerateEnvironment(environmentPrefab, 50);
    }
}
