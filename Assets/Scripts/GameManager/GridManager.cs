using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour {
    public static GridManager Instance = null; // only set this by GameManager

    public GameObject floorPrefab;
    public const int gridSizeX = 11, gridSizeY = 11; // Should be odd.

    private GameObject floorHolder;

    [Flags]
    public enum MapObj {
        Null = 0x00,
        Air = 0x01,
        Floor = 0x02,
        Enemy = 0x04,
        Cat = 0x08,
        Human = 0x10,
        Key = 0x11,
        Fish = 0x12
    }

    public MapObj[,] map;

    private void AddFloor(int x, int y) {
        var floor = Instantiate(floorPrefab, floorHolder.transform);
        floor.transform.localPosition = new Vector3(x, 0, y);
    }

    private void GenMap(int[,] grid, int floorId) {
        int w = grid.GetLength(0);
        int h = grid.GetLength(1);

        map = new MapObj[w - 2, h - 2];
        for (int i = 1; i < w - 1; ++i) {
            for (int j = 1; j < h - 1; ++j) {
                if (grid[i, j] == floorId) {
                    map[i - 1, j - 1] = MapObj.Floor;
                    AddFloor(i - 1, j - 1);
                }
                else {
                    map[i - 1, j - 1] = MapObj.Air;
                }
            }
        }
    }

    [ContextMenu("InitFloor")]
    public void InitFloor() {
        floorHolder = new GameObject("FloorHolder");
        floorHolder.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        int w = gridSizeX + 2;
        int h = gridSizeY + 2;

        // 0 -> wall or air
        // 1 -> floor
        // 2 -> floor been reached
        const int Wall = 0;
        const int Air = 1;
        const int Floor = 2;

        int[,] grid = new int[w, h];
        for (int i = 0; i < w; ++i) {
            for (int j = 0; j < h; ++j) {
                grid[i, j] = i % 2 == 1 && j % 2 == 1 ? Air : Wall;
            }
        }

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        while (true) {
            int x = Random.Range(1, w - 1);
            int y = Random.Range(1, h - 1);
            if (grid[x, y] == Air) {
                stack.Push(new Vector2Int(x, y));
                grid[x, y] = Floor;
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

                if (grid[next.x, next.y] == Air) {
                    grid[next.x, next.y] = Floor;
                    grid[(next.x + curr.x) / 2, (next.y + curr.y) / 2] = Floor;
                    stack.Push(next);
                    found = true;
                    ++depth;
                    break;
                }
            }

            if (!found || depth >= maxDepth) {
                depth = 0;
                stack.Pop();
            }
        }

        // Randomly connect some floors
        float probability = 0.3f;
        for (int i = 1; i < w - 2; ++i) {
            for (int j = 1; j < h - 2; ++j) {
                if (grid[i, j] == Wall && Random.value < probability) {
                    grid[i, j] = Floor;
                }
            }
        }

        GenMap(grid, Floor);
    }

    [ContextMenu("ClearAllFloors")]
    private void ClearAllFloors() {
        Destroy(floorHolder);
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
