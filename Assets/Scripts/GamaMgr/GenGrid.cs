using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenGrid : MonoBehaviour {
    public GameObject floorPrefab;
    public const int gridSizeX = 10, gridSizeY = 10;

    private GameObject floorHolder;
    private List<GameObject> floors = new List<GameObject>();
    //private int[,] minDis = new int[gridSizeX * gridSizeY, gridSizeX * gridSizeY];

    public bool IsInGrid(int x, int y) { return x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY; }
    public int GetFloorInternalIdx(int x, int y) { return x * gridSizeY + y; }
    public GameObject GetFloorAt(int x, int y) { return IsInGrid(x, y) ? floors[GetFloorInternalIdx(x, y)] : null; }
    public GameObject GetFloorAt(Vector2Int pos) { return GetFloorAt(pos.x, pos.y); }

    private void AddFloor(int x, int y) {
        var floor = Instantiate(floorPrefab, floorHolder.transform);
        floor.transform.localPosition = new Vector3(x, 0, y);
        floors.Add(floor);
    }

    [ContextMenu("InitFloor")]
    private void InitFloor() {
        floors.Capacity = gridSizeX * gridSizeX;
        floorHolder = new GameObject("FloorHolder");
        floorHolder.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        int w = gridSizeX + 2;
        int h = gridSizeX + 2;

        // 0 -> wall or air
        // 1 -> floor
        // 2 -> floor been reached
        const int Wall = 0;
        const int Floor = 1;
        const int Reached = 2;

        int[,] map = new int[w, h];
        for (int i = 0; i < w; ++i) {
            for (int j = 0; j < h; ++j) {
                map[i, j] = i % 2 == 1 && j % 2 == 1 ? Floor : Wall;
            }
        }

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        while (true) {
            int x = Random.Range(1, w - 1);
            int y = Random.Range(1, h - 1);
            if (x % 2 == 1 && y % 2 == 1) {
                stack.Push(new Vector2Int(x, y));
                map[x, y] = Reached;
                AddFloor(x, y);
                break;
            }
        }

        // 0 -> Up
        // 1 -> Right
        // 2 -> Down
        // 3 -> Left
        int maxDepth = Math.Max(gridSizeX, gridSizeY) / 2;
        int depth = 0;
        while (stack.Count != 0) {
            Vector2Int curr = stack.Peek();
            Vector2Int next;

            uint candidate = 0;
            bool found = false;
            while (candidate != 0xFu) {
                int dir = Random.Range(0, 4); // Get direction
                if ((candidate & (1u << dir)) != 0)
                    continue;

                candidate |= (1u << dir);
                next = curr;
                switch (dir) {
                    case 0: next.y -= 2; break;
                    case 1: next.x += 2; break;
                    case 2: next.y += 2; break;
                    case 3: next.x -= 2; break;
                }

                // Check range
                if (0 >= next.x || next.x >= w || 0 >= next.y || next.y >= h)
                    continue;

                if (map[next.x, next.y] != Reached) {
                    map[next.x, next.y] = Reached;
                    stack.Push(next);
                    found = true;

                    AddFloor(next.x, next.y);
                    AddFloor((next.x + curr.x) / 2, (next.y + curr.y) / 2);

                    ++depth;
                    break;
                }
            }

            if (!found || depth >= maxDepth) {
                depth = 0;
                stack.Pop();
            }
        }
    }

    [ContextMenu("ClearAllFloors")]
    private void ClearAllFloors() {
        Destroy(floorHolder);
        floors.Clear();
    }

    private void Start() {
        InitFloor();
        //CalMinDis();
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
