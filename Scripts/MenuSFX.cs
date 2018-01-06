using UnityEngine;
using System.Collections;

public class MenuSFX : MonoBehaviour {

	public AudioClip selection;
	public AudioClip confirmation;
	public AudioClip start;

	private AudioSource sound;

	void Start() {
		sound = GetComponent<AudioSource> ();
	}

	public void PlaySelection() {
		sound.clip = selection;
		sound.Play ();
	}

	public void PlayConfirmation() {
		sound.clip = confirmation;
		sound.Play ();
	}

	public void PlayStart() {
		sound.clip = start;
		sound.Play ();
	}
}
