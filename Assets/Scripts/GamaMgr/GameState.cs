using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
    public static GameState Instance = null; // only set this by GameManager

    public enum TurnOf { Cat, Human, Enemy, GameOver, EndGame, GameInit };

    private TurnOf turn = TurnOf.Cat;
    public TurnOf Turn { get => turn; private set { turn = value; Debug.Log("Turn of: " + turn); } }

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

    public void EndEnemyTurn() {
        if(Turn != TurnOf.Enemy)
            return;
        Turn = TurnOf.Cat;
    }

    public void GameRestart() {
        Turn = TurnOf.Cat;
    }

    public void GameOver() {
        Turn = TurnOf.GameOver;
        Debug.Log("Game Over");
        // todo
    }
}
