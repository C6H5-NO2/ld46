using UnityEngine;
using System.Collections;

public class Movable : MonoBehaviour {
    public bool BusyMoving { get; protected set; }

    protected GridManager.MapObj[,] Map { get; private set; }

    protected void Start() {
        BusyMoving = false;
        Map = GridManager.Instance.map;
    }
}
