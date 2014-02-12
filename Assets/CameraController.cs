using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	public float distance = 24;
	public Vector3 LowerLeftCorner;
	public Vector3 UpperRightCorner;

	// Note: You need to drag "KirbyWalk_0" to the Target parameter
	// 	     in the inspector

	void Start() {
		Vector3 pos = transform.position;
		pos.y = 6;
		transform.position = pos;
	}

	void Update () {
		LowerLeftCorner = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		UpperRightCorner = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, distance));
		float minX = LowerLeftCorner.x;
		float maxX = UpperRightCorner.x;

		float rightBound = (maxX + minX)/2;
		float leftBound = rightBound - 3;

		Vector3 position = transform.position;

		if (target.position.x - 16 <= 0) {
			position.x = 19;
		}
		else if ((target.position.x + 19 >= 132) && (target.position.x + 16 < 190)){
			position.x = 113;
		}
		else if ((target.position.x > 190) && (target.position.x <= 218.7f - 3)){
			position.x = 218.7f;
		}
		else if ((target.position.x >= 312) && (target.position.x < 372)){
			position.x = 312;
		}
		else if ((target.position.x > 370) && (target.position.x <= 418.8f - 3)){
			position.x = 418.8f;
		}
		else if ((target.position.x >= 512.4f) && (target.position.x < 570)){
			position.x = 512.4f;
		}
		else if ((target.position.x > 600) && (target.position.x <= 619.5f - 3 )) {
			position.x = 619.5f;
		}
		else if ((target.position.x >= 710 + 3) && (target.position.x < 780)) {
			position.x = 710 + 3;
		}
		else if ((target.position.x > 800) && (target.position.x < 819 - 3)) {
			position.x = 819;
		}
		else if ((target.position.x >= 911.3f + 2) && (target.position.x < 980)) {
			position.x = 911.3f + 2;
		}
		else if (target.position.x >= rightBound)
			position.x = target.position.x;
		else if (target.position.x <= leftBound)
			position.x = target.position.x + 3;




		position.z = target.position.z - distance;

		transform.position = position;
	}
}
