using UnityEngine;
using System.Collections;

public class WaddleController : MonoBehaviour {

	public GameObject parent;
	private Animator WaddleAnimator;
	public float moveSpeed = 5;
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
		Vector3 size = transform.localScale;
		size.x = 13;
		size.y = 13;
		transform.localScale = size;
		spawnDirection = -1 * transform.localScale.x / 13;	
		// Determines if you're starting facing left or right by + or -
	}

	void Die () {
		grounded = false;
		parent.SendMessage("kill", spawnDirection);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = transform.position;
		spawnDirection = -1 * transform.localScale.x / 13;	

		if (!onScreen (position))
		{
			Die ();
		}

		position.x += spawnDirection * moveSpeed * Time.deltaTime;
		position.z = -0.2f;
		transform.position = position;
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
