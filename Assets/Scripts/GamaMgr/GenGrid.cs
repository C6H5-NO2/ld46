using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenGrid : MonoBehaviour {
    public GameObject floorPrefab;
    public Vector2Int gridSize = new Vector2Int(10, 10);

    private GameObject floorHolder;
    private List<GameObject> floors = new List<GameObject>();

    public GameObject GetFloorAt(int x, int y) { return floors[x * gridSize.y + y]; }
    public GameObject GetFloorAt(Vector2Int pos) { return GetFloorAt(pos.x, pos.y); }

    [ContextMenu("InitFloor")]
    private void InitFloor() {
        floors.Capacity = gridSize.x * gridSize.y;
        floorHolder = new GameObject("FloorHolder");
        floorHolder.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        for(var x = 0; x < gridSize.x; ++x)
            for(var z = 0; z < gridSize.y; ++z) {
                // todo: update rand alg
                if(Random.Range(0, 10) % 7 == 0) {
                    floors.Add(null);
                    continue;
                }

                var floor = Instantiate(floorPrefab, floorHolder.transform);
                floor.transform.localPosition = new Vector3(x, 0, z);
                floors.Add(floor);
            }
    }

    [ContextMenu("ClearAllFloors")]
    private void ClearAllFloors() {
        Destroy(floorHolder);
        floors.Clear();
    }

    private void Start() {
        InitFloor();
    }

    private void Update() {

    }

    // useless
    //private IEnumerator DoInitFloor() {
    //    var floorHolder = new GameObject("FloorHolder").transform;
    //    floorHolder.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    //    for(var x = 0; x < gridSize.x; ++x)
    //        for(var z = 0; z < gridSize.y; ++z) {
    //            var floor = Instantiate(floorPrefab, floorHolder);
    //            floor.transform.localPosition = new Vector3(x, 0, z);
    //            floors.Add(floor);
    //            yield return null;
    //        }
    //}

    // too slow
    //private IEnumerator smoothMoveUp() {
    //    var hasmove = true;
    //    while(hasmove) {
    //        hasmove = false;
    //        for(var x = 0; x < gridSize.x; ++x)
    //            for(var z = 0; z < gridSize.y; ++z) {
    //                var trans = GetFloorAt(x, z).transform;
    //                var targetPos = trans.position;
    //                targetPos.y = 0;
    //                if((trans.position - targetPos).sqrMagnitude > 1e-3) {
    //                    trans.position = Vector3.Lerp(trans.position, targetPos, .7f);
    //                    hasmove = true;
    //                    yield return null; //new WaitForSeconds(.4f);
    //                }
    //                else
    //                    trans.position = targetPos;
    //            }
    //    }
    //}
}
