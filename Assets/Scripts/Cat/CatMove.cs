using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapObj = GridManager.MapObj;

public class CatMove : Movable {
    private CatHunger catHunger;
    private CatMeow catMeow;

    private new void Start() {
        base.Start();

        catHunger = GetComponent<CatHunger>();
        catMeow = GetComponent<CatMeow>();

        Map[(int)transform.position.x, (int)transform.position.z] |= MapObj.Cat;
    }

    private void HandleMove() {
        if(!Input.GetMouseButtonDown(0))
            return;

        var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(cameraRay, out RaycastHit hitInfo, 100, LayerMask.GetMask("Floor"))) {
            var search = new PathSearch(transform.position, hitInfo.point, MapObj.Air | MapObj.Enemy);
            var path = search.Solve();
            if(path != null && path.Count > 1)
                StartCoroutine(SmoothMove(path));
        }
    }

    private void Update() {
        if(!BusyMoving && catHunger.Health > 0 && GameState.Instance.Turn == GameState.TurnOf.Cat)
            HandleMove();
    }

    private IEnumerator SmoothMove(List<Vector2Int> path) {
        const float moveTime = .1f, inverseMoveTime = 1 / moveTime;

        BusyMoving = false;

        foreach(var grid in path) {
            if(!BusyMoving) {
                BusyMoving = true;
                Map[(int)transform.position.x, (int)transform.position.z] &= ~MapObj.Cat;
                continue;
            }

            var target = new Vector3(grid.x, 0, grid.y);
            while((target - transform.position).sqrMagnitude > float.Epsilon) {
                transform.position = Vector3.MoveTowards(transform.position, target, inverseMoveTime * Time.deltaTime);
                //transform.position = Vector3.Lerp(transform.position, target, 5 * Time.deltaTime);
                yield return null;
            }
            transform.position = target;
        }

        catHunger.DoMove();
        Map[(int)transform.position.x, (int)transform.position.z] |= MapObj.Cat;
        BusyMoving = false;
    }
}
