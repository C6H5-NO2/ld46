using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
    public enum TurnOf { Cat, Human, Enemy, GameOver, EndGame, GameInit };

    private TurnOf turn;
    public TurnOf Turn { get => turn; private set => turn = value; }

    public void EndCatTurn() {
        if(Turn != TurnOf.Cat)
            return;
        Turn = TurnOf.Human;
    }

    public void EndHumanTurn() {
        if(Turn != TurnOf.Human)
            return;
        Turn = TurnOf.Enemy;
    }

    // this should be done by Enemy
    public void EndEnemyTurn() {
        if(Turn != TurnOf.Enemy)
            return;
        Turn = TurnOf.Cat;
    }

    public void GameOver() {
        Turn = TurnOf.GameOver;
        Debug.Log("Game Over");
        // todo
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
