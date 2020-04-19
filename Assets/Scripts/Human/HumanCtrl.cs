using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanCtrl : MonoBehaviour, IHasHealth {
    public Text humanHealthText;

    private int health;
    public int Health {
        get => health;
        private set {
            health = value;
            humanHealthText.text = "Human Health: " + health;
        }
    }

    public void TakeDmage(int dmg) { Health -= dmg; }

    private Transform catTrans;
    private CatMeow catMeow;
    private GameState gameState;
    private GenGrid genGrid;

    void Start() {
        health = 10;
        var cat = GameObject.FindGameObjectWithTag("Cat");
        catTrans = cat.transform;
        catMeow = cat.GetComponent<CatMeow>();
        var gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameState = gameManager.GetComponent<GameState>();
        genGrid = gameManager.GetComponent<GenGrid>();
    }

    void Update() {
        if(gameState.Turn == GameState.TurnOf.Human) {
            var src = new Vector2Int((int)transform.position.x, (int)transform.position.z);
            Vector2Int dst;
            switch(catMeow.State) {
                case CatMeow.MeowState.MeowAttrackHuman:
                    dst = new Vector2Int((int)catTrans.position.x, (int)catTrans.position.z);
                    break;

                case CatMeow.MeowState.PassOut:
                // todo

                case CatMeow.MeowState.NoMeow:
                default:
                    // todo: find nearest enemy
                    dst = new Vector2Int(GenGrid.gridSizeX, GenGrid.gridSizeY);
                    break;
            }

            var newPos = FindPath.FindNextGrid(src, dst, genGrid);

            transform.position = new Vector3(newPos.x, 0, newPos.y);
            // todo: move with anim 

            gameState.EndHumanTurn();
        }
    }
}
