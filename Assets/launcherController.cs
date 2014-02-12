using UnityEngine;
using System.Collections;

public class launcherController : MonoBehaviour {
	public kirbyController kirby;
	public CameraController cam;
	private Vector3 position;
	public HUDcontroller hud;

	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (kirby.transform.position.x > 970f) {
			position = cam.transform.position;
			position.z = 0f;
			transform.position = position;
			if (kirby.transform.position.x >= 1130f) {
				kirbyController.power tempPower = kirby.currentPower;
				hud.addPoints(50000);
				hud.superDie();
				kirby.assignPower(tempPower);
			}
		}
	}
}
