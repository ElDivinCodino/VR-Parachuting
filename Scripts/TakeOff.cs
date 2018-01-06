using UnityEngine;
using System.Collections;

public class TakeOff : MonoBehaviour {

    public float heightToReach;
    public Animator pilotAnimator;
    [HideInInspector]
    public bool playerReady = false;
    [HideInInspector]
    public bool startTour = false;
	public GameObject terrain;

    private float initialAltitude;
    private bool alreadyStarted = false;
    private RotateRotors rotor;
    private float rotorsSpeed;
    private float maxRotorsSpeed;
	private float tilt;
    private Vector3 tilting;
    private float oRange;	// oscillations initial range
	
	void Start() {
		initialAltitude = GetComponent<Transform> ().position.y;
		heightToReach += initialAltitude;
        rotor = GameObject.Find("bonnet_hi_ok").GetComponent<RotateRotors>();
        maxRotorsSpeed = rotor.topSpeed;
        tilting = transform.rotation.eulerAngles;
    }

	void Update () {
		if(rotorsSpeed > (maxRotorsSpeed - 1800) && playerReady) {
            StartingTakeOff();
		} else {
            rotorsSpeed = rotor.currentSpeed;
        }

		if(alreadyStarted && transform.position.y < heightToReach) {
			Tilt();
			transform.position = (transform.position + (Vector3.up/20));
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.LookRotation(GetComponent<ReachPosition>().wayPointList[0].transform.position), Time.deltaTime / 10);
		} else if (alreadyStarted && transform.position.y >= heightToReach) { // start tour and make terrain invisible slightly, then destroy it
          
            startTour = true;
        }

	}

	void StartingTakeOff() {
		if (!alreadyStarted) {
			alreadyStarted = true;
            pilotAnimator.SetTrigger("takingOff");
			oRange = Random.Range(2f, 5f); 	// oscillations initial range

           // HideTerrain();
		}
	}

	void Tilt() {
        tilt = (Mathf.PingPong(Time.time * 1.75f, 2 * oRange) - oRange); // 1.75 rules the oscillation speed

        tilting = transform.rotation.eulerAngles;
        tilting.z = tilt;

        transform.rotation = Quaternion.Euler(tilting);
	}

    void HideTerrain() {
        Vector3 newPos = terrain.transform.localPosition;
        newPos.y -= 2;
        terrain.transform.localPosition = newPos;
    }
}
