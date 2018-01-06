using UnityEngine;
using System.Collections;

public class HeliAudioManager : MonoBehaviour {

    public AudioClip[] engineAudios;
    public RotateRotors rotor;
    public AudioSource engineSound;
    public AudioSource rotorSound;

    private bool warmUp;

    void Start () {
        engineSound.clip = engineAudios[0];
        warmUp = true;
	}
	
	void Update () {
	    if(!(engineSound.time < engineSound.clip.length - 1) && warmUp) {
            ChangeClip(engineSound, engineAudios[1], true);
            warmUp = false;
        }

        rotorSound.pitch = Mathf.Clamp(rotor.currentSpeed / 2000, 0, 1);
	}

    private void ChangeClip(AudioSource source, AudioClip clip, bool loop) {
        source.clip = clip;
        source.volume = 0.05f;
        source.loop = loop;
        source.Play();
    }
}
