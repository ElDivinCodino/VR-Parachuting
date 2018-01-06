using UnityEngine;
using System.Collections;

public class ArrivalActivator : MonoBehaviour {

    public GameObject arrival;
	
	public void OnTriggerEnter(Collider other) {
        arrival.SetActive(true);
    }
}
