using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

    public float motionSpeed;
    public float rotationSpeed;
    public Animator avatarAnim;
    public GameObject cam;
	public ForViveController[] viveControllers;

    private Rigidbody rb;
    private Vector3 movDir;
    public float rotAngle;

    void Start () {
       rb = GetComponent<Rigidbody>();
		rotAngle = 180;
    }
	
	void FixedUpdate () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

		if(viveControllers != null && viveControllers.Length > 0) {

            float i = viveControllers[0].GetX();
            float j = viveControllers[1].GetX();

            if (Mathf.Abs(i) >= Mathf.Abs(j)) {
                moveHorizontal = i;
            } else {
                moveHorizontal = j;
            }

            i = viveControllers[0].GetY();
            j = viveControllers[1].GetY();

            if (Mathf.Abs(i) >= Mathf.Abs(j)) {
                moveVertical = i;
            }
            else {
                moveVertical = j;
            }
		}
        
        Move(moveHorizontal, moveVertical);
    }

    private void Move(float h, float v) {
        if (h != 0f || v != 0f) {

            movDir = new Vector3(0f, 0f, v * motionSpeed);
            movDir = transform.TransformDirection(movDir); // to set movement on local z axis, not global one;
            rotAngle += h;

            rb.MovePosition(transform.position + movDir);
            //cam.transform.position = rb.position;
            rb.rotation = Quaternion.Euler(0f, rotAngle * rotationSpeed, 0f);

            if (v > 0) {                                     // set animation forward or backward
                avatarAnim.SetFloat("speed", movDir.magnitude / motionSpeed);
            } else {
                avatarAnim.SetFloat("speed", -movDir.magnitude / motionSpeed);
            }
        } else {
            avatarAnim.SetFloat("speed", 0f);
            rb.rotation = Quaternion.Euler(0f, rotAngle * rotationSpeed, 0f);
        }
    }
}
