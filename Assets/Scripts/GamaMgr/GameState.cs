using UnityEngine;
using System.Collections;

// todo
public class GameState : MonoBehaviour {
    public enum TurnOf { Cat, Human, Enemy, GameOver, EndGame, GameInit };

    private TurnOf turn;
    public TurnOf Turn { get => turn; private set => turn = value; }

    public void EndCatTurn() {
        if(Turn != TurnOf.Cat)
            return;

        // todo: start human turn
        Turn = TurnOf.Human;

        // todo: start enemy turn
        // this should be done by human
        Turn = TurnOf.Enemy;

        // this should be done by Enemy
        Turn = TurnOf.Cat;
    }

    private void Start() {
        turn = TurnOf.Cat;
    }

    private void Update() {
        // testcode
        if(Input.GetKeyDown(KeyCode.Space))
            EndCatTurn();
    }
}
