using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; } = null;

    public GameObject catPrefab, humanPrefab;

    private GameObject cat, human;
    public GameObject Cat { get; private set; }
    public GameObject Human { get; private set; }

    private GameState gameState;
    private GenGrid genGrid;

    private int level = 0;

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode) {
        ++level;
        Debug.Log("load level: " + level + "\nGameManager: " + GetInstanceID());
    }

    private void OnEnable() { SceneManager.sceneLoaded += OnLevelLoaded; }

    private void OnDisable() { SceneManager.sceneLoaded -= OnLevelLoaded; }

    private void Awake() {
        if(Instance == null)
            Instance = this;
        else if(Instance != this) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        gameState = GetComponent<GameState>();
        gameState.GameRestart();
        GameState.Instance = gameState;

        genGrid = GetComponent<GenGrid>();
        genGrid.InitFloor();

        Cat = Instantiate(catPrefab, Vector3.zero, Quaternion.identity);
        Human = Instantiate(humanPrefab, Vector3.zero, Quaternion.identity);
    }

    private void Start() {

    }

    private void Update() {
        // todo: testcode
        if(Input.GetKeyDown(KeyCode.Space))
            gameState.EndCatTurn();
    }
}
