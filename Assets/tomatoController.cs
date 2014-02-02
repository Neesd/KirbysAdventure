using UnityEngine;
using System.Collections;

public class tomatoController : MonoBehaviour {

	public GameObject HUD;
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			this.gameObject.SetActive(false);
			HUD.SendMessage("setHealth", 6);
		}
	}
}
