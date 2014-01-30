using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	public float distance = 25;

	// Note: You need to drag "KirbyWalk_0" to the Target parameter
	// 	     in the inspector

	void Update () {
		Vector3 LowerLeftCorner = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		Vector3 UpperRightCorner = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, distance));
		float minX = LowerLeftCorner.x;
		float minY = LowerLeftCorner.y;
		float maxX = UpperRightCorner.x;
		float maxY = UpperRightCorner.y;

		float rightBound = (maxX + minX)/2;
		float leftBound = rightBound - 3;
		float topBound = (maxY + minY) / 2 + 2;
		float bottomBound = topBound - 3;

		Vector3 position = transform.position;

		if (target.position.x >= rightBound)
			position.x = target.position.x;
		else if (target.position.x <= leftBound)
			position.x = target.position.x + 3;

		if (target.position.y >= topBound)
			position.y = target.position.y - 2;
		else if (target.position.y <= bottomBound)
			position.y = target.position.y + 1;


		position.z = target.position.z - distance;

		transform.position = position;
	}
}
