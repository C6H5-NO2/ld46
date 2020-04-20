using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMeow : MonoBehaviour {
    public enum MeowState { NoMeow, MeowAttrackHuman, MeowAttrackEnemy, Meow3, Meow4, PassOut, };

    private MeowState state;
    public MeowState State {
        get { return state; }
        private set { state = value; Debug.Log("Cat meow state: " + state); }
    }

    //public void ResetMeowState() { State = MeowState.NoMeow; }

    private CatHunger catHunger;

    private void HandleMeow() {
        if(Input.GetKeyDown(KeyCode.Z)) {
            State = MeowState.MeowAttrackHuman;
            GameState.Instance.EndCatTurn();
        }
        else if(Input.GetKeyDown(KeyCode.X)) {
            State = MeowState.MeowAttrackEnemy;
            GameState.Instance.EndCatTurn();
        }
        else if(Input.GetKeyDown(KeyCode.C)) {
            State = MeowState.Meow3;
            GameState.Instance.EndCatTurn();
        }
        else if(Input.GetKeyDown(KeyCode.V)) {
            State = MeowState.Meow4;
            GameState.Instance.EndCatTurn();
        }
    }

    private void Start() {
        State = MeowState.NoMeow;
        catHunger = GetComponent<CatHunger>();
    }

    private void Update() {
        if(catHunger.Health > 0) {
            if(GameState.Instance.Turn == GameState.TurnOf.Cat) {
                HandleMeow();
            }
        }
        else {
            State = MeowState.PassOut;
            GameState.Instance.EndCatTurn();
        }
    }
}
