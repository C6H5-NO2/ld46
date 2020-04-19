using UnityEngine;
using System.Collections;

public class TurnCtrl : MonoBehaviour {
    private bool catTurnFinished = false;

    void Update() {
        if(!catTurnFinished && Input.GetButtonDown("space")) {
            catTurnFinished = true;
            // todo: start human turn
            // todo: start enemy turn
            catTurnFinished = false;
        }
    }
}
