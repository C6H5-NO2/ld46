using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class EnemyMove : MonoBehaviour {
    private Transform catTrans, humanTrans;
    
    private enum Target { None, Cat, Human, };
    private Target target;

    // todo
    public void DoMove() {
        var src = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        Vector2Int dst;
        switch(target) {
            case Target.Cat:
                dst = new Vector2Int((int)catTrans.position.x, (int)catTrans.position.z);
                break;

            case Target.Human:
                dst = new Vector2Int((int)humanTrans.position.x, (int)humanTrans.position.z);
                break;

            case Target.None:
            default:
                dst = new Vector2Int();
                break;
        }

        PathSearch search = new PathSearch();
        search.start = src;
        search.end = dst;
        search.moveable = ~GenGrid.MapObj.Air;

        var nextGrid = search.NextGrid();
        var nextPos = new Vector3(nextGrid.x, 0, nextGrid.y);

        // todo: move with anim 
        transform.position = nextPos;
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

    private void Start() {
        catTrans = GameObject.FindGameObjectWithTag("Cat").transform;
        humanTrans = GameObject.FindGameObjectWithTag("Human").transform;
        target = Target.Human;
    }

    private void Update() {
        if((catTrans.position - transform.position).sqrMagnitude > 8.1) {
            target = Target.Human; // todo
        }
    }
}
