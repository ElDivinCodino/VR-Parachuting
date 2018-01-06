using UnityEngine;
using System.Collections;

public class RotateRotors : MonoBehaviour {

    public float topSpeed;
	public float incrementalValue;
    public Vector3 rotateAroundAxis;

    [HideInInspector]public float currentSpeed;

	void Start () {
        currentSpeed = 0f;
	}
	
	void Update () {
        currentSpeed = Mathf.Lerp(currentSpeed, topSpeed, incrementalValue * Time.deltaTime);
        transform.RotateAround(GetComponent<Renderer>().bounds.center, rotateAroundAxis, currentSpeed * Time.deltaTime);
		//Quaternion currentRot = transform.rotation;
		//currentRot.z = GetComponentInParent<Transform> ().rotation.z;
		//transform.rotation = currentRot;
	}
}
