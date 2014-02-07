using UnityEngine;
using System.Collections;

public class poppyController : MonoBehaviour {

	public GameObject parent;
	private Animator PoppyAnimator;
	public float moveSpeed = 3;
	public float jumpSpeed = 6;
	public float spawnDirection;
	private bool grounded = true;
	
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
		PoppyAnimator = this.GetComponent<Animator> ();
		spawnDirection = -1 * transform.localScale.x / 13;	
		PoppyAnimator.SetBool ("Jumping", true);
	}
	
	void Die () {
		grounded = false;
		parent.SendMessage("kill", spawnDirection);
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 vel = rigidbody2D.velocity;

		if (grounded) {
			int rand = Random.Range (0, 100);
			if (rand < 15){
				PoppyAnimator.SetBool ("Jumping", true);
				grounded = false;
				vel.y = jumpSpeed;
				vel.x = moveSpeed * spawnDirection;
			}
			else {
				vel.x = 0;
				vel.y = 0;			
			}
		}
		else {
			vel.x = moveSpeed * spawnDirection;
			vel.y -= jumpSpeed * Time.deltaTime * 2.5f;
		}

		rigidbody2D.velocity = vel;
	
		spawnDirection = -1 * transform.localScale.x / 13;	

		if (!onScreen (transform.position))
		{
			Die ();
		}
	}
	
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col && col.gameObject)
		{
			if (col.gameObject.tag == "Player") 
			{
				Die();
			}
			else if (col.gameObject.tag == "Terrain")
			{
				PoppyAnimator.SetBool ("Jumping", false);
				grounded = true;
			}
			else if (col.gameObject.tag == "Wall")
			{
				Flip ();
				spawnDirection *= -1;
			}
		}
	}
}