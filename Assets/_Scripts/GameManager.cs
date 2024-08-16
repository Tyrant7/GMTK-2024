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

    private void Start()
    {
        StartCoroutine(StartDrawPhase(10));
    }

    private IEnumerator StartDrawPhase(int countDownLength)
    {
        gameState = GameState.DrawPhase;
        CreateEnvironment();
        PhaseTimer phaseTimer = FindObjectOfType<PhaseTimer>();
        for (int i = countDownLength; i > 0; i--)
        {
            if (phaseTimer) phaseTimer.UpdateText(i);
            yield return new WaitForSeconds(1f);
        }
        if (phaseTimer) phaseTimer.UpdateText(0);

        // Sounds!

        gameState = GameState.PickPhase;
        yield return new WaitForSeconds(3f);
        SceneLoader.Instance.LoadScene("SecondScene");
    }

    private void CreateEnvironment()
    {
        generator.GenerateEnvironment(environmentPrefab, 50);
    }
}
