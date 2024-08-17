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
    List<GameObject> chosenElements;

    private void Start()
    {
        switch (SceneLoader.Instance.SceneName)
        {
            case "DrawPhase":
                StartCoroutine(StartDrawPhase(5));
                break;
            case "PickPhase":
                CreateEnvironment(true);
                StartPickPhase();
                break;
            /*
        case "BuildPhase":
            CreateEnvironment(true);
            break;
            */
            default:
                StartCoroutine(StartDrawPhase(5));
                break;
        }
    }

    private IEnumerator StartDrawPhase(int countDownLength)
    {
        Debug.Log("welcome to the draw phase");

        gameState = GameState.DrawPhase;
        CreateEnvironment(false);
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
        yield return new WaitUntil(() => !SceneLoader.LoadingScene);
        StartPickPhase();
    }

    private void CreateEnvironment(bool debug)
    {
        currentEnvironment = environmentGenerator.GenerateEnvironment(environmentPrefab, 50, debug);
    }

    private void StartPickPhase()
    {
        Debug.Log("welcome to the pick phase");

        gameState = GameState.PickPhase;
        List<GameObject> tableContents = tableGenerator.GenerateTable(currentEnvironment, currentEnvironment, 10);

        Table table = FindObjectOfType<Table>();
        table.Populate(tableContents);
    }

    public void EndPickPhase()
    {
        Table table = FindObjectOfType<Table>();
        chosenElements = table.GetChosenElements();

        StartCoroutine(TransitionOutOfPickPhase());
    }

    private IEnumerator TransitionOutOfPickPhase()
    {
        SceneLoader.Instance.LoadScene("BuildPhase", TransitionHandler.TransitionType.SideSwipe);
        yield return new WaitUntil(() => !SceneLoader.LoadingScene);
        StartBuildPhase();
    }

    private void StartBuildPhase()
    {
        Debug.Log("welcome to the build phase");
        gameState = GameState.PickPhase;
    }
}
