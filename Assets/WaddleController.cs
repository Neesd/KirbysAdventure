using UnityEngine;
using System.Collections;

public class WaddleController : MonoBehaviour {
	
	private Animator WaddleAnimator;
	public Transform target;
	public float moveSpeed = 5;
	public float spawnDirection = 0;
	public float prevDirection = -1;	
	private Vector3 originalPos;
	private Vector3 prevPos;
	private bool spawned;
	private bool spawnReady;

	void Flip (){
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	bool onScreen (Vector3 pos){
		float distance = 25;
		Vector3 LowerLeftCorner = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		Vector3 UpperRightCorner = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, distance));
		float minX = LowerLeftCorner.x;
		float minY = LowerLeftCorner.y;
		float maxX = UpperRightCorner.x;
		float maxY = UpperRightCorner.y;
		
		return ((minX < pos.x) && (minY < pos.y) && (maxX > pos.x) && (maxY > pos.y));
	}

	// Use this for initialization
	void Start () {
		WaddleAnimator = this.GetComponent<Animator> ();
		// Adjusts the framerate of the animations
		WaddleAnimator.speed = 0.5f;
		Vector3 size = transform.localScale;
		size.x = 13;
		size.y = 13;
		transform.localScale = size;

		BoxCollider2D boxCol = this.GetComponent<BoxCollider2D> ();
		Vector3 boxSize = boxCol.size;
		boxSize.x = 0.15f;
		boxSize.y = 0.15f;
		boxCol.size = boxSize;

		target = GameObject.FindWithTag ("Player").transform;
		Vector3 targetPos = target.position;
		Vector3 position = transform.position;
		originalPos = position;
		if (targetPos.x < position.x)
		{
			spawnDirection = -1;
		}
		else if (targetPos.x > position.x)
		{
			spawnDirection = 1;
		}

		if (prevDirection != spawnDirection)
		{
			prevDirection = spawnDirection;
			Flip ();
		}
		prevPos = originalPos;

		if (onScreen (originalPos)) 
		{
			spawned = true;
			spawnReady = false;
		}
		else
		{
			spawned = false;
			renderer.enabled = false;
			spawnReady = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPos = target.position;
		Vector3 position = transform.position;

		if (spawned)
		{
			if (!onScreen (position))
			{
				// Moved off screen, no longer exist
				spawned = false;
				renderer.enabled = false;
			}
			position.x += spawnDirection * moveSpeed * Time.deltaTime;
			position.z = -0.2f;
			prevPos = transform.position;
			transform.position = position;
		}
		else
		{
			if (onScreen (originalPos) && spawnReady)
			{
				// Can only respawn once the spawn point is back onscreen.
				spawned = true;
				renderer.enabled = true;
				transform.position = originalPos;
				prevPos = originalPos;
				if (targetPos.x < originalPos.x)
				{
					spawnDirection = -1;
				}
				else if (targetPos.x > originalPos.x)
				{
					spawnDirection = 1;
				}
				spawnReady = false;
			}
			else if (!onScreen (originalPos))
			{
				// Can't set spawnReady until the respawn point is off screen.
				spawnReady = true;
			}
			if (prevDirection != spawnDirection)
			{
				prevDirection = spawnDirection;
				Flip ();
			}
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "Player") 
		{
			spawned = false;
			renderer.enabled = false;
		}
		else if (col.gameObject.tag == "Terrain")
		{
			spawnDirection *= -1;
			Flip ();
		}
	}

	void Die ()
	{
		spawned = false;
		renderer.enabled = false;
	}
}
