using UnityEngine;
using System.Collections;

public class PowerBeamEnemy : MonoBehaviour {


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (transform.rotation.eulerAngles.z > 235 || transform.rotation.eulerAngles.z < 89)
			transform.rotation = Quaternion.Euler (0f, 0f, 90f);
		else {
			Vector3 rotate = new Vector3(0f, 0f, 4f);
			transform.Rotate (rotate);
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "Player") 
		{
			col.gameObject.SendMessage("getHit");
		}
	}

	void OnBecameVisible () {
		transform.rotation = Quaternion.Euler (0f, 0f, 90f);
	}
}
