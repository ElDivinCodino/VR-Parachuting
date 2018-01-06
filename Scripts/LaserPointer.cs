using UnityEngine;
using System.Collections;

public class LaserPointer : MonoBehaviour {

	public Color color;
	public float thickness = 0.002f;
	public GameObject holder;
	public GameObject pointer;

	void Start () {
		holder = new GameObject();
		holder.transform.parent = this.transform;
		holder.transform.localPosition = Vector3.zero;

		pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointer.GetComponent<BoxCollider>().isTrigger = true;
        pointer.AddComponent<MenuSelectionWithVive>();
		pointer.transform.parent = holder.transform;
		pointer.transform.localScale = new Vector3(thickness, thickness, 1000f);
		pointer.transform.localPosition = new Vector3(0f, 0f, 500f);

		Material newMaterial = new Material(Shader.Find("Unlit/Color"));
		newMaterial.SetColor("_Color", color);
		pointer.GetComponent<MeshRenderer>().material = newMaterial;
	}
}
