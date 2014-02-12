using UnityEngine;
using System.Collections;

public class WaddleController : MonoBehaviour {

	public GameObject parent;
	private Animator WaddleAnimator;
	public float moveSpeed = 4;
	public float spawnDirection;
	private bool grounded = false;

	public void Flip (){
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
		spawnDirection = -1 * transform.localScale.x / 13;	
		// Determines if you're starting facing left or right by + or -
	}

	void Die () {
		grounded = false;
		parent.SendMessage("kill", spawnDirection);
	}
	
	// Update is called once per frame
	void Update () {
		spawnDirection = -1 * transform.localScale.x / 13;	
		if (!onScreen (transform.position))
		{
			Die ();
		}
		Vector2 vel = rigidbody2D.velocity;
		vel.x = moveSpeed * spawnDirection;
		rigidbody2D.velocity = vel;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col && col.gameObject)
		{
			if (col.gameObject.tag == "Player") 
			{
				col.gameObject.SendMessage("getHit");
				col.gameObject.SendMessage ("addPoints", 400);
				Die();
			}
			else if (col.gameObject.tag == "Terrain")
			{
				if (!grounded)
				{
					grounded = true;
				}
			}
			else if (col.gameObject.tag == "Wall")
			{
				Flip ();
				spawnDirection *= -1;
			}
		}
	}
}
