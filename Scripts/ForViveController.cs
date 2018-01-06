using UnityEngine;
using System.Collections;

public class ForViveController : MonoBehaviour {

    public GameObject[] handles;
    public GameObject[] rudders;
    public TriggerEject triggerEject;
    public GlidePlayer glidePlayer;

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;

    private GameObject handle;
    private GameObject previousParent;
    private Vector3 initialHandlePosition;
    private Quaternion initialHandleRotation;
    private Vector3 handlePos;

    void Start() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update() {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        if (device.GetPressDown(triggerButton) && handle != null) {
            initialHandlePosition = handle.transform.localPosition;
            initialHandleRotation = handle.transform.localRotation;
            previousParent = handle.transform.parent.gameObject;
            handle.transform.parent = this.transform;
            handlePos = handle.transform.position;
        }

        if (device.GetPress(triggerButton) && handle != null && handle.tag == "Handle") {
            CheckIfHandleIsPulled();
        }

        if (device.GetPress(triggerButton) && handle != null && handle.tag == "Rudder") {
            RudderPulled();
        }

        if (device.GetPressUp(triggerButton) && handle != null) {
            handle.transform.parent = previousParent.transform;
            StartCoroutine(Reposition(handle, initialHandlePosition, initialHandleRotation));
            handle = null;
        }

        if (device.GetPressDown(triggerButton) && handle == null) {
            triggerEject.triggerPressed = true;
        }

        if (device.GetPressUp(triggerButton) && handle == null) {
            triggerEject.triggerPressed = false;
        }
    }

    public void CollidedWith(GameObject collided) {
        device.TriggerHapticPulse(500);
        if (handle == null) {
            handle = collided;
        }

    }

    public void FinishedColliding(GameObject collided) {
        if (collided == handle) {
            handle = null;
        }
            
    }

    private void CheckIfHandleIsPulled() {
        if ((previousParent.transform.position - handle.transform.position).sqrMagnitude > 1f) {
            triggerEject.handlePulled = true;
        }
    }

    private void RudderPulled() {
        if (handle.name == "LeftRudder") {
            glidePlayer.leftPull = Mathf.Clamp((handlePos - handle.transform.position).sqrMagnitude, 0, 1);
        }
        else if (handle.name == "RightRudder") {
            glidePlayer.rightPull = Mathf.Clamp((handlePos - handle.transform.position).sqrMagnitude, 0, 1);
        }
    }

    public float GetX() {
        return device.GetAxis().x;
    }

    public float GetY() {
        return device.GetAxis().y;
    }

    IEnumerator Reposition(GameObject handle, Vector3 initialHandlePosition, Quaternion initialHandleRotation) {

        while ((handle.transform.localPosition - initialHandlePosition).sqrMagnitude > 0.001f) {
            handle.transform.localPosition = Vector3.Lerp(handle.transform.localPosition, initialHandlePosition, Time.deltaTime);
            handle.transform.localRotation = Quaternion.Slerp(handle.transform.localRotation, initialHandleRotation, Time.deltaTime);
        }

        yield return null;
    }
}