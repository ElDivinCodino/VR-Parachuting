using UnityEngine;
using System.Collections;

public class BlinkingAnim : MonoBehaviour {

    public float blinkingSpeed;

    private Renderer rend;
    private Color temp;

    void Start () {
        rend = GetComponent<Renderer>();
    }
	

	void Update () {
        float lerp = Mathf.PingPong(Time.time / blinkingSpeed, .7f);
        temp = rend.material.color;
        temp.a = lerp;
        rend.material.color = temp;
    }
}
