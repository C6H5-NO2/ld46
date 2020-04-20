using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMove : MonoBehaviour {
    private GenGrid genGrid;
    private CatHunger catHunger;
    private CatMeow catMeow;

    private void Start() {
        genGrid = GameManager.Instance.GetComponent<GenGrid>();
        catHunger = GetComponent<CatHunger>();
        catMeow = GetComponent<CatMeow>();
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
        if(catHunger.Health > 0 && GameState.Instance.Turn == GameState.TurnOf.Cat)
            HandleMove();
    }
}
