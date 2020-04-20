using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMeow : MonoBehaviour {
    public enum MeowState { NoMeow, MeowAttrackHuman, MeowAttrackEnemy, /*Meow3, Meow4,*/ PassOut, };

    private MeowState state;
    public MeowState State {
        get { return state; }
        private set { state = value; Debug.Log("Cat meow state: " + state); }
    }

    public delegate void MeowAttrackEnemy();
    public static event MeowAttrackEnemy OnMeowAttrackEnemy;

    private CatHunger catHunger;
    private CatMove catMove;

    private void HandleMeow() {
        if(Input.GetKeyDown(KeyCode.Z)) {
            State = MeowState.MeowAttrackHuman;
            GameState.Instance.EndCatTurn();
        }
        else if(Input.GetKeyDown(KeyCode.X)) {
            State = MeowState.MeowAttrackEnemy;
            OnMeowAttrackEnemy?.Invoke();
            GameState.Instance.EndCatTurn();
        }
        else if(Input.GetKeyDown(KeyCode.C)) {
            State = MeowState.NoMeow;
            GameState.Instance.EndCatTurn();
        }
    }

    private void Start() {
        State = MeowState.NoMeow;
        catHunger = GetComponent<CatHunger>();
        catMove = GetComponent<CatMove>();
    }

    private void Update() {
        if(catHunger.Health > 0) {
            if(!catMove.BusyMoving && GameState.Instance.Turn == GameState.TurnOf.Cat) {
                HandleMeow();
            }
        }
        else {
            State = MeowState.PassOut;
            Invoke("GameState.Instance.EndCatTurn", .06f);
        }
    }
}
