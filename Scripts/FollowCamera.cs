using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

    public Transform cameraObject;

    void LateUpdate() {
        transform.rotation = cameraObject.rotation;
    }

}
