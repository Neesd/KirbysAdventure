using UnityEngine;
using System.Collections;

public class platController : MonoBehaviour {

	private bool colliding;
	private bool triggering;

	public void TriggerOn() {
		this.collider2D.isTrigger = true;
	}
	
	public void TriggerOff() {
		this.collider2D.isTrigger = false;
	}
	
	// Update is called once per frame
	void Update () {
		triggering = this.collider2D.isTrigger;
		if (colliding && triggering){
			TriggerOn ();
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other && other.tag == "Player"){
			TriggerOff ();
		}
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (other.collider){
			if (other.collider.tag == "Player" && !triggering){
				colliding = true;
			}
		}
	}
	
}
