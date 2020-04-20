using UnityEngine;
using System.Collections;

// todo
// unused yet
public class Movable : MonoBehaviour {

    private bool busyMoving = false;
    public bool BusyMoving { get; private set; }

    private Transform moveTarget;
    private Transform looktarget;

    private IEnumerator SmoothMove(Vector3 target, float smoothing = 1) {
        BusyMoving = true;
        while((transform.position - target).sqrMagnitude > .005f) {
            transform.position = Vector3.Lerp(transform.position, target, smoothing * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
        BusyMoving = false;
    }
}
