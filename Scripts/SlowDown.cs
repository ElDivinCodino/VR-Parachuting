using UnityEngine;
using System.Collections;

public class SlowDown : MonoBehaviour {

	public ReachPosition control;

	public bool slowDown = false;

	void Update() {
		if (slowDown) {
			control.currentSpeed = Mathf.Lerp (control.currentSpeed, 60, Time.deltaTime / 10);
		}
	}

	public void OnTriggerEnter(Collider other) {
		slowDown = true;
	}
}
