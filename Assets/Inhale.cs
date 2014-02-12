using UnityEngine;
using System.Collections;

public class Inhale : MonoBehaviour {

	public enum power
	{none, beam, electric, fire}
	public power powerGained;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "Enemy") 
		{
			col.gameObject.SendMessage("Die");
			if (col.gameObject.layer == 13)
				SendMessageUpwards("addPoints", 900);
			else
				SendMessageUpwards ("addPoints", 200);
		} else if (col.gameObject.tag == "BeamEnemy") {
			powerGained = power.beam;
			SendMessageUpwards("assignPower", (int)powerGained);
			SendMessageUpwards ("addPoints", 300);
			col.gameObject.SendMessage("Die");
		} else if (col.gameObject.tag == "ElectricEnemy") {
			powerGained = power.electric;
			SendMessageUpwards("assignPower", (int)powerGained);
			SendMessageUpwards ("addPoints", 300);
			col.gameObject.SendMessage("Die");
		} else if (col.gameObject.tag == "FireEnemy") {
			powerGained = power.fire;
			SendMessageUpwards("assignPower", (int)powerGained);
			SendMessageUpwards ("addPoints", 300);
			col.gameObject.SendMessage("Die");
		}
	}
}
