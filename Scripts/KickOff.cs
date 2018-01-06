using UnityEngine;
using System.Collections;

public class KickOff : MonoBehaviour {

    public Phrasing phrase;

    private GameObject player;
    private Animator anim;
    private AudioSource voice;
    private bool hasSpoken = false;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        voice = GetComponent<AudioSource>();
        voice.clip = phrase.selectedAudio[0];
	}
	
	void Update () {
        if(!hasSpoken) {
            transform.LookAt(player.transform);
        }

        if (!hasSpoken && Mathf.Abs((transform.position - player.transform.position).magnitude) < 4) {
            anim.SetTrigger("canShout");
            voice.Play();
            hasSpoken = true;
        }
	}
}
