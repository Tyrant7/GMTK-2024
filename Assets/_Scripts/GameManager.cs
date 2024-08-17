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
        NotStarted, DrawPhase, PickPhase, BuildPhase, ScorePhase, CombatPhase,
    }

    public GameState gameState { get; private set; }


    [SerializeField] GameObject environmentPrefab;
    [SerializeField] GameObject playerCutoutPrefab;

    [SerializeField] EnvironmentGenerator environmentGenerator;
    [SerializeField] TableGenerator tableGenerator;
    [SerializeField] Scorer scorer;
    [SerializeField] CombatHandler combatHandler;

    List<EnvironmentObject> currentEnvironment;
    List<GameObject> chosenElements;
    List<CutoutObject> placedElements;

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
            case "BuildPhase":
                gameState = GameState.BuildPhase;
                break;
            case "CombatPhase":
                gameState = GameState.CombatPhase;
                break;
            default:
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
        currentEnvironment[0].transform.root.gameObject.SetActive(false);
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
        table.Populate(tableContents, playerCutoutPrefab);
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
        gameState = GameState.PickPhase;

        Debug.Log("welcome to the build phase");
        WorkshopTable workshopTable = FindObjectOfType<WorkshopTable>();
        workshopTable.AddChosenObjects(chosenElements);
    }

    public void EndBuildPhase()
    {
        WorkshopTable workshopTable = FindObjectOfType<WorkshopTable>();
        placedElements = workshopTable.GetPlacedElements();
        if (placedElements.Count < chosenElements.Count)
        {
            // TODO
            // Gray out build button or give warning to player
            Debug.Log("Didn't place all elements");
            return;
        }

        StartScorePhase();
    }

    private void StartScorePhase()
	{
        gameState = GameState.ScorePhase;
        StartCoroutine(AnimateScorePhase());
	}

    private IEnumerator AnimateScorePhase()
    {
        ScoreDisplayInfo info = scorer.ScoreEnvironment(currentEnvironment, placedElements);
        StartCoroutine(scorer.AnimateScores(info));
        yield return new WaitUntil(() => !scorer.Animating);

        yield return new WaitForSeconds(1f);

        Environment environment = FindObjectOfType<Environment>(true);
        environment.gameObject.SetActive(true);
        foreach (EnvironmentObject envObj in currentEnvironment)
        {
            envObj.gameObject.SetActive(true);
        }

        Reveal tableReveal = FindObjectOfType<WorkshopTable>().gameObject.GetComponentInChildren<Reveal>();
        StartCoroutine(tableReveal.AnimateReveal());
        yield return new WaitUntil(() => !tableReveal.IsRevealing);

        yield return new WaitForSeconds(1f);

        StartCoroutine(tableReveal.AnimateConceal());
        yield return new WaitUntil(() => !tableReveal.IsRevealing);

        // Let's create the spawn requests for the combat phase so all of these objects don't have to stick around
        List<SpawnRequest> spawnRequests = new List<SpawnRequest>();
        foreach (CutoutObject cutout in placedElements)
        {
            spawnRequests.Add(new SpawnRequest(cutout.transform.position, cutout.prefab, cutout.isPlayer));
        }

        SceneLoader.Instance.LoadScene("CombatPhase", TransitionHandler.TransitionType.SideSwipe);
        yield return new WaitUntil(() => !SceneLoader.LoadingScene);
        StartCombatPhase(spawnRequests, info.totalScore);
    }

    private void StartCombatPhase(List<SpawnRequest> spawnRequests, int score)
    {
        gameState = GameState.CombatPhase;

        // Cleanup scene, let's destroy the environment and notepad
        GameObject environment = FindObjectOfType<Environment>().gameObject;
        GameObject notepad = FindObjectOfType<Notepad>().gameObject;
        if (environment) Destroy(environment);
        if (notepad) Destroy(notepad);

        combatHandler.StartCombat(spawnRequests, score);
    }

    public void EndCombatPhase()
    {
        Debug.Log("ending combat phase");
    }
}

public struct SpawnRequest
{
    public Vector2 position;
    public GameObject prefab;
    public bool isPlayer;

    public SpawnRequest(Vector2 position, GameObject prefab, bool isPlayer)
    {
        this.position = position;
        this.prefab = prefab;
        this.isPlayer = isPlayer;
    }
}
