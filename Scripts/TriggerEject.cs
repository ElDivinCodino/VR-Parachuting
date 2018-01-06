using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerEject : MonoBehaviour {

    public float draggingPower;
    public ResetCamPos resetCamPos;
    public Animator animDoor;
    public Transform placeWhereCrouch;
    public InstructionWrite screen;
    public LayerMask defaultLayer;
	public LayerMask targetLayer;
	public GameObject terrain;
    public Camera finalCamera;
    public SlowDown slowDown;
	public ReachPosition reachPosition;
    public Phrasing phrasing;
    public Text helper;
	public AudioClip[] parachuteClips;
	public GameObject[] rudders;
    [HideInInspector]
    public bool triggerPressed = false;
    [HideInInspector]
    public bool handlePulled = false;

    private Animator animPlayer;
	private Animator animPara;
	private GameObject player;
	private Rigidbody rb;
    private bool canStand = false;
    private bool playerStand = false;
    private bool ejecting = false;
    private bool canOpenPara = false;
    private bool paraOpened = false;
    private Vector3 wind;
	private string[] phrases;
    private AudioClip[] audios;
    private AudioSource playerAudio;

    void Start() {
		player = GameObject.FindGameObjectWithTag ("Player");
		animPlayer = player.GetComponent<Animator> ();
		animPara = GameObject.FindGameObjectWithTag ("Parachute").GetComponentInChildren<Animator> ();
		rb = player.GetComponent<Rigidbody>();
		phrases = phrasing.selectedLanguage;
        audios = phrasing.selectedAudio;
        playerAudio = player.GetComponent<AudioSource>();
		wind = Vector3.zero;
    }

    void FixedUpdate() {

		Ray ray = new Ray(player.transform.position, Vector3.down);

        if (playerStand) {
            rb.MovePosition(Vector3.MoveTowards(rb.position, placeWhereCrouch.position, 10 * Time.fixedDeltaTime));
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, placeWhereCrouch.rotation, 30 * Time.fixedDeltaTime));
        }

        if (canStand && (Input.GetButtonDown("Jump") || triggerPressed)) {
            animPlayer.SetTrigger("standUp");
            playerStand = true;
            StartCoroutine(Eject());
        }

        if (ejecting && (Input.GetButtonDown("Jump") || triggerPressed)) {
            helper.text = "";
            playerStand = false;
            animPlayer.SetTrigger("jump");
            player.transform.parent = null;
            resetCamPos.moving = false;
            float maxSpeed = 54; // metres per second
            if (rb.velocity.magnitude > maxSpeed) {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

            TurnAudio3D();
            reachPosition.canGo = true;// restart helicopter ride
            StartCoroutine(OpenPara());
        }

        if (canOpenPara) {
          ejecting = false;
            
			if (Input.GetButtonDown("Jump") || handlePulled) {
                helper.text = "";
                playerAudio.clip = parachuteClips[2];
                playerAudio.Play();
                playerAudio.loop = false;
                StartCoroutine(ChangeAudio());
                animPlayer.SetTrigger("openPara");
                animPara.SetTrigger("openPara");
                paraOpened = true;
                player.GetComponent<GlidePlayer>().enabled = true;
				StartCoroutine (WindSimulation());
                canOpenPara = false;
                rb.drag = draggingPower;
                resetCamPos.moving = true;
            }      
        }

        if (!paraOpened && canOpenPara) { // otherwise ragdolling at start

			if (Physics.Raycast(ray, 80f, defaultLayer | targetLayer)) { // if too low activate ragdoll to simulate crashing
                player.GetComponent<Animator>().enabled = false;

                if (Physics.Raycast(ray, 1f, defaultLayer | targetLayer)) { // if too low activate ragdoll to simulate crashing
                    StartCoroutine(Death());
                }
            }
        }

		if (paraOpened) {

			rb.AddForce (wind, ForceMode.Impulse);

			if (Physics.Raycast(ray, 1f, targetLayer)) { // when slightly above target zone, lands!
                StartCoroutine(Land());
			}
		}
    }

        void OnTriggerEnter(Collider other) {
        if (other.gameObject == player) {
            StartCoroutine(PrepareForEject());
            animDoor.SetBool("doorOpened", true);
			playerAudio.clip = audios [3];
			playerAudio.Play ();
            reachPosition.canGo = false;
            reachPosition.currentSpeed = 0;	// stop helicopter ride
            slowDown.slowDown = false;
        }
    }

    IEnumerator PrepareForEject() {
        yield return new WaitForSeconds(3);
        screen.StartWriting(phrases[5], true, false);
        canStand = true;
    }

    IEnumerator Eject() {
		yield return new WaitForSeconds(3);
		playerAudio.clip = audios [4];
		playerAudio.Play ();
		helper.text = phrases [6];
        ejecting = true;
        canStand = false;
    }

    IEnumerator OpenPara() {
        ejecting = false;
        yield return new WaitForSeconds(1);
        rb.isKinematic = false;
        rb.AddForce((player.transform.forward * 250 + Vector3.up * 100), ForceMode.Impulse);
        playerAudio.clip = parachuteClips[0];
        playerAudio.Play();
        yield return new WaitForSeconds(10);
        playerAudio.clip = parachuteClips[1];
        playerAudio.Play();
        playerAudio.loop = true;
        yield return new WaitForSeconds(12);
		helper.text = phrases [8];
        GameObject[] handlers = GameObject.FindGameObjectsWithTag("Handle");
        if(handlers[0] != null) {
            handlers[0].GetComponent<BoxCollider>().isTrigger = true;
            handlers[1].GetComponent<BoxCollider>().isTrigger = true;
        }
        canOpenPara = true;
    }

	IEnumerator WindSimulation() {

		while(paraOpened) {
			
			float a = (Random.value * 10) - 5; //random value between -5 and 5
			float b = (Random.value * 10) - 5; //random value between -5 and 5

			wind = new Vector3 (a, 0, b);

			yield return new WaitForSeconds (5);
		}
	}

    IEnumerator Death() {
        playerAudio.Stop(); //sostiuisci con rumore morte
		paraOpened = false;
        yield return new WaitForSeconds(2);
        ChangeCamera(finalCamera); // get a black background, make all readable also in vr
        helper.text = phrases[10];
    }

    IEnumerator Land() {
        animPlayer.SetTrigger("land");
		paraOpened = false;
        playerAudio.clip = parachuteClips[2];
        playerAudio.Play();
        playerAudio.loop = false;
        yield return new WaitForSeconds(1);
        ChangeCamera(finalCamera); // get a black background, make all readable also in vr
        helper.text = phrases[11];
    }

    private void TurnAudio3D() {
        AudioSource[] audios = placeWhereCrouch.parent.GetComponents<AudioSource>();

        foreach (AudioSource source in audios) {
            source.spatialBlend = 1.0f;
        }
    }

    private void ChangeCamera(Camera cam) {
        Camera[] cameras = Camera.allCameras;

        foreach(Camera enabledCam in cameras) {
            enabledCam.enabled = false;
        }

        cam.enabled = true;

        GameObject display = GameObject.FindGameObjectWithTag("Display");
        Vector3 pos = display.transform.localPosition;
        pos.z = 4; // sentence too long, need to be farthest from camera
        Quaternion rot = display.transform.localRotation;
        display.transform.parent = cam.transform;
        display.transform.localPosition = pos;
        display.transform.localRotation = rot;
    }

    IEnumerator ChangeAudio() {
        yield return new WaitForSeconds(2);

        playerAudio.clip = parachuteClips[3];
        playerAudio.Play();
        playerAudio.loop = true;
    }

}