using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapObj = GridManager.MapObj;

public class HumanMove : Movable {
    private Transform catTrans;
    private CatMeow catMeow;

    private EnemyManager enemyManager;

    private new void Start() {
        base.Start();

        catTrans = GameManager.Instance.Cat.transform;
        catMeow = catTrans.GetComponent<CatMeow>();

        enemyManager = GameManager.Instance.GetComponent<EnemyManager>();

        Map[(int)transform.position.x, (int)transform.position.z] |= MapObj.Human;
    }

    private void Update() {
        if(GameState.Instance.Turn == GameState.TurnOf.Human) {
            Vector3 dst;

            // todo: testcode
            var exitPos = new Vector3(GridManager.gridSizeX - 2, 0, GridManager.gridSizeY - 2);

            switch(catMeow.State) {
                case CatMeow.MeowState.MeowAttrackHuman:
                    dst = catTrans.position;
                    break;

                case CatMeow.MeowState.PassOut:
                    // todo: kill all enemies
                    if(enemyManager.enemyMoves.Count == 0)
                        dst = exitPos;
                    else {
                        var minSqrDis = float.MaxValue;
                        Transform nearestEnemy = null;
                        foreach(var enemy in enemyManager.enemyMoves) {
                            var sqrDis = (enemy.transform.position - transform.position).sqrMagnitude;
                            if(sqrDis < minSqrDis) {
                                minSqrDis = sqrDis;
                                nearestEnemy = enemy.transform;
                            }
                        }
                        dst = nearestEnemy.position;
                    }
                    break;

                case CatMeow.MeowState.NoMeow:
                default:
                    // todo: find path to key
                    // attack ~2 block enemies
                    dst = exitPos;
                    break;
            }

            var search = new PathSearch(transform.position, dst); //, MapObj.Air | MapObj.Enemy);
            var nextGrid = search.NextGrid();
            var nextPos = new Vector3(nextGrid.x, 0, nextGrid.y);

            StartCoroutine(SmoothMove(nextPos));
        }
    }

    private IEnumerator SmoothMove(Vector3 target) {
        const float moveTime = .15f, inverseMoveTime = 1 / moveTime;
        BusyMoving = true;
        Map[(int)transform.position.x, (int)transform.position.z] &= ~MapObj.Human;

        while((target - transform.position).sqrMagnitude > float.Epsilon) {
            transform.position = Vector3.MoveTowards(transform.position, target, inverseMoveTime * Time.deltaTime);
            yield return null;
        }
        transform.position = target;

        Map[(int)transform.position.x, (int)transform.position.z] |= MapObj.Human;
        BusyMoving = false;

        GameState.Instance.EndHumanTurn();
    }
}
