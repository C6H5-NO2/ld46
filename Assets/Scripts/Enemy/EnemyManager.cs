using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
    public GameObject enemyPrefab;

    [HideInInspector] public List<EnemyMove> enemyMoves;

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
        while(enemyMoves.Count < enemyNum) {
            var randGrid = new Vector2Int(Random.Range(1, GridManager.gridSizeX),
                                          Random.Range(1, GridManager.gridSizeY));
            if(GridManager.Instance.map[randGrid.x, randGrid.y] != GridManager.MapObj.Floor)
                continue;

            var randPos = new Vector3(randGrid.x, 0, randGrid.y);

            var enemy = Instantiate(enemyPrefab, randPos, Quaternion.identity);

            var enemyMove = enemy.AddComponent<EnemyMove>();
            enemy.AddComponent<EnemyHealth>();
            enemy.AddComponent<EnemyAttack>();

            enemyMoves.Add(enemyMove);
        }
    }

    private void Start() {
        catTrans = GameManager.Instance.Cat.transform;
        catMeow = catTrans.GetComponent<CatMeow>();

        humanHealth = GameManager.Instance.Human.GetComponent<HumanHealth>();

        InitEnemies();
    }

    private void Update() {
        if(GameState.Instance.Turn == GameState.TurnOf.Enemy) {
            foreach(var enemyMov in enemyMoves)
                enemyMov.DoMove(); // todo
            while(true) {
                bool quit = true;
                foreach(var enemyMov in enemyMoves)
                    if(enemyMov.BusyMoving)
                        quit = false;
                if(quit)
                    break;
            }
            GameState.Instance.EndEnemyTurn();
        }
    }
}
