using UnityEngine;
using System.Collections;

public class launcherController : MonoBehaviour {
	public kirbyController kirby;
	public CameraController cam;
	public WaddleController waddle;
	public BroncoController bronco;
	private Vector3 position;

	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (kirby.transform.position.x > 970f) {
			position = cam.transform.position;
			position.z = 0f;
			transform.position = position;
		}
	}
}
