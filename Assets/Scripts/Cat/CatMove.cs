using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMove : MonoBehaviour {
    public GenGrid genGrid;

    private CatHunger catHunger;
    private CatMeow catMeow;
    private GameState gameState;

    private void Start() {
        catHunger = GetComponent<CatHunger>();
        catMeow = GetComponent<CatMeow>();
        gameState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameState>();
    }

    private void HandleMove() {
        if(!Input.GetMouseButtonDown(0))
            return;

        var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(cameraRay, out RaycastHit hitInfo, 100, LayerMask.GetMask("Floor"))) {
            var pos = new Vector3Int((int)hitInfo.point.x, 0, (int)hitInfo.point.z);
            Debug.Log(hitInfo.point + " " + pos);

            // todo: move with anim
            transform.position = pos;

            catHunger.DoMove();
        }
        else {
            Debug.Log("no hit");
        }
    }

    private void Update() {
        if(catHunger.Health > 0 && gameState.Turn == GameState.TurnOf.Cat)
            HandleMove();
    }
}
