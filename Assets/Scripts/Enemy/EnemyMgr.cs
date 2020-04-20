using UnityEngine;
using System.Collections.Generic;

public class EnemyMgr : MonoBehaviour {
    //public static EnemyMgr Instance; // only set in GameManager

    public GameObject enemyPrefab;

    [HideInInspector] public List<EnemyMove> enemyMoves;

    //[HideInInspector] public GameObject cat, human; // only set in GameManager

    private Transform catTrans;
    private CatMeow catMeow;

    private HumanHealth humanHealth;

    private void InitEnemies() {
        if(enemyMoves == null) {
            enemyMoves = new List<EnemyMove>();
        }
        else {
            foreach(var enemy in enemyMoves)
                Destroy(enemy.gameObject);
            enemyMoves.Clear();
        }

        // todo: upd alg
        // load rand enemies
        int enemyNum = Random.Range(1, 4);
        while(enemyNum-- != 0) {
            // shit code
            var randPos = new Vector3(Random.Range(1, GridManager.gridSizeX), 0, Random.Range(1, GridManager.gridSizeY));
            var enemy = Instantiate(enemyPrefab, randPos, Quaternion.identity);

            var enemyMove = enemy.AddComponent<EnemyMove>();

            enemyMoves.Add(enemyMove);
        }
    }

    private void Start() {
        catTrans = GameObject.FindGameObjectWithTag("Cat").transform;
        catMeow = catTrans.GetComponent<CatMeow>();

        humanHealth = GameObject.FindGameObjectWithTag("Human").GetComponent<HumanHealth>();

        InitEnemies();
    }

    private void Update() {
        if(GameState.Instance.Turn == GameState.TurnOf.Enemy) {
            foreach(var enemyMov in enemyMoves)
                enemyMov.DoMove(); // todo
            GameState.Instance.EndEnemyTurn();
        }
    }
}
