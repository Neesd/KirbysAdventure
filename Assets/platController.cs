using UnityEngine;
using System.Collections;

public class platController : MonoBehaviour {

	public GameObject kirby;

	// Update is called once per frame
	void Update () {
		Physics2D.IgnoreLayerCollision(11, 27, (kirby.rigidbody2D.velocity.y > 0.0f));
	}
}
