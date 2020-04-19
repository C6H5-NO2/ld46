using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMove : MonoBehaviour {
    public Material dst, src;
    public GenGrid genGrid;

    private void Start() {

    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(cameraRay, out RaycastHit hitInfo, 100, LayerMask.GetMask("Floor"))) {
                var pos = new Vector3Int((int)hitInfo.point.x, 0, (int)hitInfo.point.z);
                Debug.Log(hitInfo.point + " " + pos);

                transform.position = pos;
                // testcode
                // todo: a toggled floor can not be toggled again
                StartCoroutine(ToggleFloorTile(genGrid.GetFloorAt(pos.x, pos.z).GetComponentInChildren<Renderer>()));
            }
            else {
                Debug.Log("no hit");
            }
        }
    }

    private IEnumerator ToggleFloorTile(Renderer floorRenderer) {
        var srcMat = floorRenderer.material;
        floorRenderer.material = dst;
        yield return new WaitForSeconds(1);
        floorRenderer.material = srcMat;
    }
}
