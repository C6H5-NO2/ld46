using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapObj = GridManager.MapObj;


public class EnemyMove : Movable {
    private Transform catTrans, humanTrans;

    private enum Target { None, Cat, Human, };
    private Target target;

    // todo
    public void DoMove() {
        Vector3 dst;
        switch(target) {
            case Target.Cat:
                dst = catTrans.position;
                break;

            case Target.Human:
                dst = humanTrans.position;
                break;

            case Target.None:
            default:
                dst = transform.position;
                break;
        }

        var search = new PathSearch(transform.position, dst, MapObj.Air | MapObj.Cat | MapObj.Human);
        var nextGrid = search.NextGrid();
        var nextPos = new Vector3(nextGrid.x, 0, nextGrid.y);

        StartCoroutine(SmoothMove(nextPos));
    }

    /* 
     * 25  26  29  34  41  50
     * 16  17  20  25  32  41
     *  9  10  13  18  25  34
     *  4   5   8  13  20  29
     *  1   2   5  10  17  26
     *  0   1   4   9  16  25
     */

    private void SetTargetToCat() {
        if((catTrans.position - transform.position).sqrMagnitude > 2.1)
            return;
        target = Target.Cat;
    }

    private void OnEnable() { CatMeow.OnMeowAttrackEnemy += SetTargetToCat; }
    private void OnDisable() { CatMeow.OnMeowAttrackEnemy -= SetTargetToCat; }

    private new void Start() {
        base.Start();

        catTrans = GameObject.FindGameObjectWithTag("Cat").transform;
        humanTrans = GameObject.FindGameObjectWithTag("Human").transform;
        target = Target.Human;

        Map[(int)transform.position.x, (int)transform.position.z] |= MapObj.Enemy;
    }

    private void Update() {
        if((catTrans.position - transform.position).sqrMagnitude > 8.1) {
            target = Target.Human; // todo
        }
    }

    private IEnumerator SmoothMove(Vector3 target) {
        const float moveTime = .15f, inverseMoveTime = 1 / moveTime;
        BusyMoving = true;
        Map[(int)transform.position.x, (int)transform.position.z] &= ~MapObj.Enemy;

        while((target - transform.position).sqrMagnitude > float.Epsilon) {
            transform.position = Vector3.MoveTowards(transform.position, target, inverseMoveTime * Time.deltaTime);
            yield return null;
        }
        transform.position = target;

        Map[(int)transform.position.x, (int)transform.position.z] |= MapObj.Enemy;
        BusyMoving = false;
        GameState.Instance.EndHumanTurn();
    }
}
