using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AltitudeWrite : MonoBehaviour {

    public Transform helicopter;
    public InstructionWrite instructions;
	public Phrasing phrasing;
    [HideInInspector] public bool starting = false;

    private string[] phrases;

    private Text text;
    private bool count = false;

    void Start () {
        text = GetComponent<Text>();
		phrases = phrasing.selectedLanguage;
	}
	
	
	void Update () {
	    if(starting) {
            StartCoroutine(WriteFirstly());
            starting = false;
        }

        if (count) {
            text.text = BuildString();
        }
	}

    IEnumerator WriteFirstly() {
		char[] toWrite = (phrases[0] + '\n' + (System.Math.Round(helicopter.position.y + 450f, 0)).ToString() + phrases[1]).ToCharArray();
        foreach(char c in toWrite) {
            yield return new WaitForSeconds(0.1f);
            text.text += c;
        }
        count = true;
        instructions.starting = true;
    }

    private string BuildString() {
		string currentAltitude = (System.Math.Round(helicopter.position.y + 450f, 0)).ToString() + phrases[1]; // truncate value at the 0 decimal
		return phrases[0] + '\n' + currentAltitude;
    }
}
