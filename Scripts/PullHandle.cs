using UnityEngine;
using System.Collections;

public class PullHandle : MonoBehaviour {

    private ForViveController controller;

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Controller" && controller == null) {
            Debug.Log("uno");
            controller = collider.gameObject.GetComponent<ForViveController>();
            controller.CollidedWith(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (controller && collider.gameObject.name == controller.gameObject.name) {
            Debug.Log("ahiauno");
            //controller.FinishedColliding(this.gameObject);
            controller = null;
        }
    }
}
