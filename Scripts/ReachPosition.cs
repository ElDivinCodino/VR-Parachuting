using UnityEngine;
using System.Collections;

public class ReachPosition : MonoBehaviour {

    public TakeOff takeOff;
    public Transform[] wayPointList;
    public float maxSpeed;
    [HideInInspector] public float currentSpeed = 0;
    public float maxGlide; // gliding boundary
    public float rotationSpeed = 0.5f;
    public bool canGo = true;

    public Transform targetWayPoint;
    public int currentWayPoint = 0;
    private float offset; // angle between current z axis and next z axis

    void Update() {
        if(takeOff.startTour && canGo) {

            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime/5);

            // check if we have somewere to go
			if (currentWayPoint < (this.wayPointList.Length - 1)) {
                if (targetWayPoint == null)
                    targetWayPoint = wayPointList[currentWayPoint];
                ToNextWaypoint();
            } else { // if you reach last checkpoint, destroy helicopter
				//Destroy(gameObject);
            }
        }     
    }

    private void ToNextWaypoint() {
        // rotate slightly towards the target
		offset = Mathf.Clamp(Vector3.Angle(transform.forward, targetWayPoint.transform.forward), -maxGlide, maxGlide);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetWayPoint.rotation, rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward * -offset * Time.deltaTime);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, currentSpeed * Time.deltaTime);

        if (transform.position == targetWayPoint.position) {
            currentWayPoint++;
            targetWayPoint = wayPointList[currentWayPoint];
        }
    }
}