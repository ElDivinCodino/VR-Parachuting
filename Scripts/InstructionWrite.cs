using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InstructionWrite : MonoBehaviour {

    public Transform readyWaypoint;
	public Transform helicopter;
	public Phrasing phrasing;
    [HideInInspector] public bool starting = false;

    private Text text;
	private string[] phrases;
    private float target;
    private bool changeText = true;
    private bool calculateResidual = false;

    void Start() {
		text = GetComponent<Text>();
		phrases = phrasing.selectedLanguage;
        target = Mathf.FloorToInt(readyWaypoint.localPosition.y);
        phrases[3] += target.ToString() + phrases[1];
    }


    void Update() {
        if (starting) {
            StartWriting(phrases[2], true, false);
            starting = false;
        }

        if (helicopter.position.y > 1000 && changeText) {
			StartWriting(phrases[3], false, true);
            changeText = false;
        }

        if(calculateResidual) {
            text.text = BuildString();
        }
    }

    public void StartWriting(string toWrite, bool blinking, bool calculate) {
        StartCoroutine(Write(toWrite, blinking, calculate));
    }

    IEnumerator Write(string toWrite, bool blinking, bool calculate) {
        text.text = "";
        StopCoroutine("BlinkingText");
        text.enabled = true;                // force text to be visible (StopCoroutine could stop when text was disabled)
        char[] charToWrite = toWrite.ToCharArray();
        foreach (char c in charToWrite) {
            yield return new WaitForSeconds(0.1f);
            text.text += c;
        }
        if (blinking) {
            StartCoroutine("BlinkingText");
        }
        if (calculate) {
            yield return new WaitForSeconds(1);
            calculateResidual = true;
        }
    }

    IEnumerator BlinkingText() {
        while(true) {
            yield return new WaitForSeconds(.5f);
            text.enabled = !text.enabled;
        }
    }

    private string BuildString() {
		string remain = (target - System.Math.Round(helicopter.position.y, 0)).ToString() + phrases[1]; // truncate value at the 0 decimal
		return phrases[3] + '\n' + phrases[4] + remain;
    }
}
