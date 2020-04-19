using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMeow : MonoBehaviour {
    public enum MeowState { Meow1, Meow2, Meow3, Meow4, };

    private MeowState state;
    public MeowState State {
        get { return state; }

        private set {
            state = value;
            Debug.Log("Cat meow state: " + state);
        }
    }

    private void Start() {

    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Z)) {
            State = MeowState.Meow1;
        }
        else if(Input.GetKeyDown(KeyCode.X)) {
            State = MeowState.Meow2;
        }
        else if(Input.GetKeyDown(KeyCode.C)) {
            State = MeowState.Meow3;
        }
        else if(Input.GetKeyDown(KeyCode.V)) {
            State = MeowState.Meow4;
        }
    }
}
