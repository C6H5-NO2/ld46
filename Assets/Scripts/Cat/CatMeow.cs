using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMeow : MonoBehaviour {
    public enum MeowState { NoMeow, MeowAttrackHuman, MeowAttrackEnemy, Meow3, Meow4, };

    private MeowState state;
    public MeowState State {
        get { return state; }
        private set { state = value; Debug.Log("Cat meow state: " + state); }
    }

    //public void ResetMeowState() { State = MeowState.NoMeow; }

    private CatHunger catHunger;
    private GameState gameState;

    private void HandleMeow() {
        if(Input.GetKeyDown(KeyCode.Z)) {
            State = MeowState.MeowAttrackHuman;
        }
        else if(Input.GetKeyDown(KeyCode.X)) {
            State = MeowState.MeowAttrackEnemy;
        }
        else if(Input.GetKeyDown(KeyCode.C)) {
            State = MeowState.Meow3;
        }
        else if(Input.GetKeyDown(KeyCode.V)) {
            State = MeowState.Meow4;
        }
    }

    private void Start() {
        catHunger = GetComponent<CatHunger>();
        gameState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameState>();
    }

    private void Update() {
        if(catHunger.HungerVal > 0) {
            if(gameState.Turn == GameState.TurnOf.Cat) {
                HandleMeow();
                gameState.EndCatTurn();
            }
        }
        else {
            state = MeowState.NoMeow;
            gameState.EndCatTurn();
        }

    }
}
