using UnityEngine;
using System.Collections;

public class SetupGame : MonoBehaviour {

	public string language;
	public Phrasing phrasing;
	public GameObject male;
	public GameObject female;
	public GameObject spawnPoint;

	void Awake () {
		GameObject setup = GameObject.Find ("GameSetup");
		language = setup.GetComponent<Setup> ().language;

		phrasing.SetLanguage (language);
	}

}
