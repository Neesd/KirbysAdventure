﻿using UnityEngine;
using System.Collections;

public class Power : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag.Contains("Enemy")){
			SendMessageUpwards("addPoints", 600);
			col.gameObject.SendMessage("Die");
		}
	}
}
