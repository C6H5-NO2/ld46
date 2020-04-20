using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapObj = GridManager.MapObj;

public class HumanMove : Movable {
    private Transform catTrans;
    private CatMeow catMeow;

    private new void Start() {
        base.Start();

        catTrans = GameObject.FindGameObjectWithTag("Cat").transform;
        catMeow = catTrans.GetComponent<CatMeow>();

        Map[(int)transform.position.x, (int)transform.position.z] |= MapObj.Human;
    }

    private void Update() {
        if(GameState.Instance.Turn == GameState.TurnOf.Human) {
            Vector3 dst;
            switch(catMeow.State) {
                case CatMeow.MeowState.MeowAttrackHuman:
                    dst = catTrans.position;
                    break;

                case CatMeow.MeowState.PassOut:
                // todo: kill all enemy

                case CatMeow.MeowState.NoMeow:
                default:
                    // todo: find nearest enemy
                    dst = new Vector3(GridManager.gridSizeX - 1, 0, GridManager.gridSizeY - 1);
                    break;
            }

            var search = new PathSearch(transform.position, dst, MapObj.Air | MapObj.Enemy);
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
