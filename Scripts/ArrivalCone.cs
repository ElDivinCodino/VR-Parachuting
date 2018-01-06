using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArrivalCone : MonoBehaviour {

	public Phrasing phrasing;

    private Text helper;
    private GameObject player;
    private bool blinking = false;

    void Start() {
        helper = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<Text>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

	void Update() {
		if (blinking) {
			StartCoroutine (BlinkingText (helper));
            blinking = false;
		}
	}

	public void OnTriggerEnter(Collider other) {
        if(other.gameObject == player) {
            helper.text = "";
		    blinking = false;
        }
	}

	public void OnTriggerExit(Collider other) {
        if (other.gameObject == player && player.GetComponent<Animator>().isActiveAndEnabled) {
            helper.text = phrasing.selectedLanguage[9];
            blinking = true;
        }
	}

	IEnumerator BlinkingText(Text text) {
        while (true) {
            yield return new WaitForSeconds(.5f);
		    text.enabled = !text.enabled;
        }
		
	}
}