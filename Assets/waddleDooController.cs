using UnityEngine;
using System.Collections;

public class waddleDooController : MonoBehaviour {

	public GameObject parent;
	public GameObject BeamPower;
	private Animator waddleDooAnimator;
	public float moveSpeed = 4;
	public float jumpSpeed = 7;
	public float spawnDirection;
	private bool grounded = true;
	private bool attacking;
	private bool jumping;
	private float counter = 20;
	private float walkingCounter = 20;
	private float jumpingCounter = 20;
	private bool walking;
	
	public void Flip (){
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
		scale = BeamPower.transform.localScale;
		scale.x *= -1;
		BeamPower.transform.localScale = scale;
		Vector3 pos = BeamPower.transform.localPosition;
		pos.x += scale.x * 0.3f;
		BeamPower.transform.localPosition = pos;
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
		waddleDooAnimator = this.GetComponent<Animator> ();
		// Adjusts the framerate of the animations
		waddleDooAnimator.speed = 0.5f;
		spawnDirection = -1 * transform.localScale.x / 13;	
		// Determines if you're starting facing left or right by + or -
		BeamPower.SetActive (false);
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
		if (attacking)
		{
			if (counter == 0){
				attacking = false;
				BeamPower.SetActive (false);
				counter = 20;
			}
			vel.x = 0;
			vel.y = 0;
			counter = counter - 1;
		}
		else if (jumping)
		{
			if (jumpingCounter == 0){
				jumping = false;
				jumpingCounter = 20;
				rigidbody2D.gravityScale = 40;
				rigidbody2D.mass = 1;
			}
			vel.x = moveSpeed * spawnDirection;
			vel.y = jumpSpeed;
			jumpingCounter = jumpingCounter - 1;
		}
		else if (walking) 
		{
			if (walkingCounter == 0){
				walking = false;
				walkingCounter = 20;
			}
			vel.x = moveSpeed * spawnDirection;
			vel.y = 0;
			walkingCounter = walkingCounter - 1;
		}
		else
		{
			int rand = Random.Range (0, 100);
			if (rand < 10 && grounded){
				attacking = true;
				BeamPower.SetActive (true);
				vel.x = 0;
				vel.y = 0;
			}
			else if (rand < 15 && grounded){
				grounded = false;
				jumping = true;
				rigidbody2D.gravityScale = 0;
				rigidbody2D.mass = 200;
				vel.x = moveSpeed * spawnDirection;
				vel.y = jumpSpeed;
			}
			else if (rand < 30 && grounded){
				walking = true;
				vel.x = moveSpeed * spawnDirection;
				vel.y = 0;
			}
			else{
				vel.x = moveSpeed * spawnDirection;
				vel.y = -1 * jumpSpeed;
			}
		}
		rigidbody2D.velocity = vel;
	}
	
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col && col.gameObject)
		{
			if (col.gameObject.tag == "Player") 
			{
				col.gameObject.SendMessage("getHit");
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
