﻿using UnityEngine;
using System.Collections;

public class HUDcontroller : MonoBehaviour {
	// This only works if you have 4:3 resolution set up,
	// which is accessible in the top left menu in the game view.

	// Yes, I know this is terrible, just bear with it and 
	// drag them all into the inspector.
	public Texture2D HUDlives0;
	public Texture2D HUDlives1;
	public Texture2D HUDlives2;
	public Texture2D Health0;
	public Texture2D Health1;
	public Texture2D num0;
	public Texture2D num1;
	public Texture2D num2;
	public Texture2D num3;	
	public Texture2D num4;
	public Texture2D num5;
	public Texture2D num6;
	public Texture2D num7;
	public Texture2D num8;
	public Texture2D num9;

	// These simplify code below
	private Texture2D[] HUDlives;
	private Texture2D[] Health;
	private Texture2D[] Numbers;

	private float livesCount = 0;
	public float livesCountMax = 72;  // Must be a multiple of 4
	private float healthCount = 0;
	public float healthCountMax = 38; // Must be a multiple of 2
	public float lives = 4;
	public float health = 6;
	public float points = 0;

	private float heightFactor;
	private float widthFactor;

	void setHealth(int healthPoints){
		health = healthPoints;
	}

	void Start () {
		// Setting up the HUD position and scale.
		Vector3 pos = this.transform.position;
		pos.x = 0.5f;
		pos.y = 0.15f;
		this.transform.position = pos;

		Vector3 scale = this.transform.localScale;
		scale.x = 1;
		scale.y = 0.3f;
		this.transform.localScale = scale;

		// These are the screen dimensions that I used to place and size
		// everything originally. These are used later to resize things as needed.
		heightFactor = (float) Screen.height / (float)382;
		widthFactor = (float) Screen.width/ (float) 509;

		HUDlives = new Texture2D[] {HUDlives0, HUDlives1, HUDlives2, HUDlives1};
		Health = new Texture2D[] {Health0, Health1};
		Numbers = new Texture2D[] {num0, num1, num2, num3, num4, num5, num6, num7, num8, num9};
	}

	void Update () {
		if (livesCount < livesCountMax - 1){
			livesCount = livesCount + 1;
		}
		else {
			livesCount = 0;
		}

		if (healthCount < healthCountMax - 1) {
			healthCount = healthCount + 1;
		}
		else {
			healthCount = 0;
		}
	}

	void OnGUI () {
		// Kirby animation next to lives
		float livesX = 383 * widthFactor;
		float livesY = 302 * heightFactor;
		float livesWidth = 28 * widthFactor;
		float livesHeight = 23 * heightFactor;
		GUI.DrawTexture (new Rect(livesX, livesY, livesWidth, livesHeight), 
		                 HUDlives[Mathf.FloorToInt (livesCount / livesCountMax * 4)]);


		// Number of lives listed
		float x0Diff = 43 * widthFactor;
		float x1Diff = 59.5f * widthFactor;
		float yDiff = 7.5f * heightFactor;
		float numWidth = 16.5f * widthFactor;
		float numHeight = 14.5f * heightFactor;
		
		Rect lives0Rect = new Rect (livesX + x0Diff, livesY + yDiff, numWidth, numHeight);
		Rect lives1Rect = new Rect (livesX + x1Diff, livesY + yDiff, numWidth, numHeight);
		GUI.DrawTexture (lives0Rect, Numbers[Mathf.FloorToInt (lives / 10)]);
		GUI.DrawTexture (lives1Rect, Numbers[Mathf.FloorToInt (lives % 10)]);


		// Health bars
		float healthX = 147.5f * widthFactor;
		float healthY = 292 * heightFactor;
		float healthDist = 16.5f * widthFactor;
		float healthHeight = 25 * heightFactor;
		float healthWidth = 17 * widthFactor;
		for (int i = 0; i < 6; i++){
			if (health > i){
				GUI.DrawTexture (new Rect(healthX + i * healthDist, healthY, 
				                          healthWidth, healthHeight), 
				                 Health[Mathf.FloorToInt (healthCount / healthCountMax * 2)]);
			}
		}

		// Points listed
		float pointsX = (148.2f + 16.5f * 6) * widthFactor;
		float pointsY = 323 * heightFactor;
		float pointsDist = 16.5f * widthFactor;
		float pointsWidth = 16.5f * widthFactor;
		float pointsHeight = 14.5f * heightFactor;

		int[] pointsArray = new int[] {0, 0, 0, 0, 0, 0, 0};
		int j = 0;

		float pointsCopy = points;
		while (Mathf.FloorToInt (pointsCopy) > 0){
			pointsArray[j] = Mathf.FloorToInt(pointsCopy % 10);
			pointsCopy = pointsCopy / 10;
			j = j + 1;
		}

		for (int i = 0; i < 7; i++){
			GUI.DrawTexture (new Rect (pointsX - i * pointsDist, pointsY, 
				                       pointsWidth, pointsHeight), 
				             Numbers[Mathf.FloorToInt (pointsArray[i])]);
		}
	}
}