using UnityEngine;
using System.Collections;

public class EnemyMgr : MonoBehaviour {

    void Start() {

    }

    void Update() {
        // todo: testcode
        GameState.Instance.EndEnemyTurn();
    }
}
