using UnityEngine;
using System.Collections;

public class TriggerGettingIn : MonoBehaviour {

    public Animator animDoor;
    public TakeOff takeOff;
    public Transform placeWhereClimb;
    public AltitudeWrite altitude;
	public Phrasing phrases;
    public float movementSpeed;

	public GameObject player;
    private Rigidbody rb;
    private Animator animPlayer;
    private Transform helicopter;
    private bool climbUp = false;
    private bool repositioning = false;
    private bool rerotating = false;
    private Vector3 direction;

	void Start() {
		player = GameObject.FindGameObjectWithTag ("Player");
        rb = player.GetComponent<Rigidbody>();
        helicopter = takeOff.gameObject.transform;
		animPlayer = player.GetComponent<Animator> ();
    }

    void FixedUpdate() {

        if (repositioning) {
            direction = ((placeWhereClimb.position - Vector3.right) - player.transform.position).normalized;
            rb.MovePosition(player.transform.position + direction * movementSpeed * Time.fixedDeltaTime);
        }

        if (rerotating) {
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, (Quaternion.Euler(new Vector3(0, 90, 0))), movementSpeed * 100 * Time.fixedDeltaTime));
        }

        if (climbUp) {
            rb.MovePosition(Vector3.MoveTowards(player.transform.position, (placeWhereClimb.position + new Vector3(-0.04f, 0.15f, -0.03f)), movementSpeed * Time.fixedDeltaTime));
        }

        if (animPlayer.GetCurrentAnimatorStateInfo(0).IsName("stand_to_sit")) {
            climbUp = false;
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, helicopter.rotation, movementSpeed * 100 * Time.fixedDeltaTime));
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject == player) {
            player.GetComponent<MovePlayer>().enabled = false;
            GetComponent<Renderer>().enabled = false;
            animDoor.SetBool("doorOpened", true);
            animPlayer.SetFloat("speed", 0f);
            helicopter.gameObject.GetComponent<Collider>().enabled = false;
            repositioning = true;
            animPlayer.SetFloat("speed", 0.11f);
            rerotating = true;

            TurnAudio2D();

            StartCoroutine(GetIn());
        }
    }

    IEnumerator GetIn() {
		AudioSource playerAudio = player.GetComponent<AudioSource> ();
        yield return new WaitForSeconds(1);
        animPlayer.SetFloat("speed", 0f);
        yield return new WaitForSeconds(2);
        JumpIntoHelicopter();
		playerAudio.clip = phrases.selectedAudio [1];
		playerAudio.Play ();
		yield return new WaitForSeconds(5);
		playerAudio.clip = phrases.selectedAudio [2];
		playerAudio.Play ();
        animDoor.SetBool("doorOpened", false);
        takeOff.playerReady = true;
        altitude.starting = true;
    }

    private void JumpIntoHelicopter() {
        animPlayer.SetTrigger("getIn");
        rb.isKinematic = true;
        player.transform.parent = helicopter;
        repositioning = false;
        rerotating = false;
        climbUp = true;
    }

    private void TurnAudio2D() {
        AudioSource[] audios = takeOff.gameObject.GetComponents<AudioSource>();

        foreach(AudioSource source in audios) {
            source.spatialBlend = 0.0f;
        }
    }
}
