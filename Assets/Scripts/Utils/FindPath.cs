using UnityEngine;
using System.Collections;

public class FindPath {
    // Should've learnt A* before the jam...
    public static Vector2Int FindNextGrid(Vector2Int src, Vector2Int dst, GenGrid genGrid) {
        if(src == dst)
            return src;

        // this code cant work.
        // whatever...

        var deltas = new Vector2Int[] {
            new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1),
        };

        var minDis = int.MaxValue;
        var minPos = new Vector2Int(src.x, src.y);
        foreach(var delta in deltas) {
            var newPos = src + delta;
            if(genGrid.GetFloorAt(newPos) == null)
                continue;
            var newDis = (newPos - dst).sqrMagnitude;
            if(newDis < minDis) {
                minDis = newDis;
                minPos = newPos;
            }
        }

        return minPos;
    }
}
