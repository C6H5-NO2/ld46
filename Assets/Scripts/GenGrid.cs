using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenGrid : MonoBehaviour {
    public GameObject floorPrefab;
    public Vector2Int gridSize = new Vector2Int(10, 10);

    private void InitFloor() {
        var floorHolder = new GameObject("FloorHolder").transform;
        floorHolder.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        for(var x = 0; x < gridSize.x; ++x)
            for(var z = 0; z < gridSize.y; ++z) {
                var floor = Instantiate(floorPrefab, floorHolder);
                floor.transform.localPosition = new Vector3(x, 0, z);
            }
    }

    private void Start() {
        InitFloor();
    }
}
