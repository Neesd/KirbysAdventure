using UnityEngine;
using System.Collections;


public class hotheadController : MonoBehaviour {

	public GameObject parent;
	public GameObject FirePower;
	private Animator hotheadAnimator;
	public float moveSpeed = 4;
	public float spawnDirection;
	private bool grounded = false;
	private bool attacking;
	private float counter = 20;
	private float walkingCounter = 20;
	private bool walking;
	
	public void Flip (){
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
		scale = FirePower.transform.localScale;
		scale.x *= -1;
		FirePower.transform.localScale = scale;
		Vector3 pos = FirePower.transform.localPosition;
		pos.x += scale.x * 0.3f;
		FirePower.transform.localPosition = pos;
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
		hotheadAnimator = this.GetComponent<Animator> ();
		// Adjusts the framerate of the animations
		hotheadAnimator.speed = 0.5f;
		spawnDirection = -1 * transform.localScale.x / 13;	
		// Determines if you're starting facing left or right by + or -
		FirePower.SetActive (false);
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
				FirePower.SetActive (false);
				counter = 20;
			}
			vel.x = 0;
			vel.y = 0;
			counter = counter - 1;
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
			if (rand < 10){
				attacking = true;
				FirePower.SetActive (true);
				vel.x = 0;
				vel.y = 0;
			}
			else if (rand < 20){
				walking = true;
				vel.x = moveSpeed * spawnDirection;
				vel.y = 0;
			}
			else{
				vel.x = moveSpeed * spawnDirection;
				vel.y = 0;
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
				col.gameObject.SendMessage ("addPoints", 600);
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
