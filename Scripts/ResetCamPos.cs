using UnityEngine;
using System.Collections;

public class ResetCamPos : MonoBehaviour {

    public Transform head;
    public Transform parent;
    public GameObject spine;
    public bool moving = true;

    private Vector3 distanceFromHead;

    void Start() {
        UnityEngine.VR.InputTracking.Recenter();

        Valve.VR.OpenVR.System.ResetSeatedZeroPose(); // HTC Vive
        Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated); // HTC Vive

        distanceFromHead = transform.position - head.transform.position;
    }

    void LateUpdate() {     // modify body along with Oculus to avoid clipping

        if (Input.GetKeyDown(KeyCode.P)) {
            UnityEngine.VR.InputTracking.Recenter(); // Oculus Rift DK2

            Valve.VR.OpenVR.System.ResetSeatedZeroPose(); // HTC Vive
            Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated); // HTC Vive
        }

        RepositionCamHeight();

        //float bodyTilt = UnityEngine.VR.InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.Head).z;

        //head.localScale = Vector3.Min (Vector3.one, Vector3.one + new Vector3(Mathf.Clamp(bodyTilt * 10 , -1, 0), 1, Mathf.Clamp(bodyTilt * 10, -1, 0)));
        
        //spine.transform.Rotate(Mathf.Clamp(bodyTilt * 50, -30,-7.85f), 0, 0);
        //spine.transform.Rotate(0, 0, UnityEngine.VR.InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.Head).x * -100);
    }


    private void RepositionCamHeight() {
        if (moving) {
            transform.position = head.transform.position + distanceFromHead;
            transform.rotation = Quaternion.LookRotation(parent.forward);
        } else {
            transform.position = head.transform.position + distanceFromHead;
            Vector3 rot = parent.rotation.eulerAngles;
            rot.x = 81;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rot), Time.deltaTime / 5);
        }
        
    }
}
