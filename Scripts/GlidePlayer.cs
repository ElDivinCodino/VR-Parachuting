using UnityEngine;
using System.Collections;

public class GlidePlayer : MonoBehaviour {

	public ForViveController[] viveControllers;

	[HideInInspector]public float rightPull;
	[HideInInspector]public float leftPull;

	private Rigidbody rb;
	private Vector3 glideForce;
	private Vector3 glideMovement;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		if(!(viveControllers[0] == null)) {
			moveHorizontal = rightPull - leftPull;
			if(moveHorizontal < 0.2f) {	// check if rudders are almost pulled the same way
                Debug.Log("Move1");
                if (rightPull > 0 && leftPull > 0) {	// check if both rudders are pulled
                    Debug.Log("Move2");
				} else {    // neither left nor right are pulled 
                    Debug.Log("Move3");
				}
			}

            float i = viveControllers[0].GetY();
            float j = viveControllers[1].GetY();

            if (Mathf.Abs(i) >= Mathf.Abs(j)) {
                moveVertical = i;
            }
            else {
                moveVertical = j;
            }
            Debug.Log("Go " + moveHorizontal);
		}

		Glide(moveHorizontal, moveVertical);
        rightPull = 0;
        leftPull = 0;
	}

	private void Glide(float h, float v) {
		glideForce = new Vector3 (h, 0, -v);
		glideMovement = new Vector3 (v, 0, -h);

		rb.AddForce (glideForce * 10, ForceMode.Impulse);
		rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.Euler (glideMovement * 10), Time.fixedDeltaTime/2));

		if (h == 0 && v == 0) {
			float verticalGlide = 0;
			Mathf.PingPong (verticalGlide, 2);

			glideMovement = new Vector3 (verticalGlide - 1, 0, 0);

			rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.Euler (glideMovement), Time.fixedDeltaTime/2)); // motivo per cui appena aperto il paracadute si gira di 90 gradi
		}
	}
}
