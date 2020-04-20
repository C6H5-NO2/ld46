using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class HumanCtrl : MonoBehaviour, IHasHealth {
    private Text humanHealthText;

    private int health;
    public int Health {
        get => health;
        private set {
            health = value > 0 ? value : 0;
            humanHealthText.text = "Human Health: " + health;
        }
    }

    public void TakeDmage(int dmg) { Health -= dmg; }

    private Transform catTrans;
    private CatMeow catMeow;
    //private GenGrid genGrid;

    void Start() {
        humanHealthText = GameObject.Find("HumanHealthText").GetComponent<Text>();
        Health = 10;

        var cat = GameObject.FindGameObjectWithTag("Cat");
        catTrans = cat.transform;
        catMeow = cat.GetComponent<CatMeow>();
    }

    void Update() {
        if(GameState.Instance.Turn == GameState.TurnOf.Human) {
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
                    dst = new Vector2Int(GenGrid.gridSizeX - 1, GenGrid.gridSizeY - 1);
                    break;
            }

            PathSearch search = new PathSearch();
            search.start = src;
            search.end = dst;
            search.moveable = ~GenGrid.MapObj.Air;

            var nextGrid = search.NextGrid();
            var nextPos = new Vector3(nextGrid.x, 0, nextGrid.y);

            // todo: move with anim 
            transform.position = nextPos;

            GameState.Instance.EndHumanTurn();
        }
    }
}
