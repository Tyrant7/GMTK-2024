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

    [SerializeField] EnvironmentGenerator environmentGenerator;
    [SerializeField] TableGenerator tableGenerator;

    List<EnvironmentObject> currentEnvironment;

    private void Start()
    {
        StartCoroutine(StartDrawPhase(5));
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
        yield return new WaitForSeconds(2f);
        SceneLoader.Instance.LoadScene("PickPhase", TransitionHandler.TransitionType.SideSwipe);
        StartCoroutine(StartPickPhase());
    }

    private void CreateEnvironment()
    {
        currentEnvironment = environmentGenerator.GenerateEnvironment(environmentPrefab, 50);
    }

    private IEnumerator StartPickPhase()
    {
        yield return new WaitUntil(() => !SceneLoader.LoadingScene);

        gameState = GameState.PickPhase;
        List<GameObject> tableContents = tableGenerator.GenerateTable(currentEnvironment, currentEnvironment, 10);

        Table table = FindObjectOfType<Table>();
        table.Populate(tableContents);
    }

    public void Pick(GameObject chosen)
    {
        Debug.Log("picking " + chosen.name);
    }
}
