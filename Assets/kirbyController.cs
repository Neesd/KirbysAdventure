using UnityEngine;
using System.Collections;

public class kirbyController : MonoBehaviour {

	private Animator kirbyAnimator;
	public float 		speed = 12.5f;
	public float		slowSpeed = 12.5f / 2.0f;
	public float 		jumpSpeed = 7.0f;
	public float 		hopSpeed = 7.0f * 2.0f;
	public float 		fallSpeed = -10.0f;
	public float 		slowFallSpeed = -10.0f / 2.0f;
	public float 		drift = 0.3f;
	public int 			direction = 1;
	public bool			grounded = false;

	// Use this for initialization
	void Start () {
		kirbyAnimator = this.GetComponent<Animator> ();
		// Adjusts the framerate of the animations
		kirbyAnimator.speed = 0.5f;

		// Adjusts the size of the animation to what you want it to be
		// The sprites are small, but all match the same dimensions
		Vector3 size = transform.localScale;
		size.x = 13;
		size.y = 13;
		transform.localScale = size;
	}
	
	// Helper functions here
	void SetMoving(bool x){
		kirbyAnimator.SetBool ("Moving", x);
	}

	void SetFloating(bool x){
		kirbyAnimator.SetBool ("Floating", x);
	}
	
	void SetJumping(bool x){
		kirbyAnimator.SetBool ("Jumping", x);
	}

	void SetHeight(int x){
		kirbyAnimator.SetInteger ("Height", x);
		/*-1 = floor squish
		 * 0 = normal
		 * 1 = inflated */
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Terrain") {
			grounded = true;
			print ("Enter");
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Terrain") {
			grounded = false;
			print ("Exit");
		}
	}

	// Update is called once per frame
	void Update () {
		//float vertical = Input.GetAxis ("Vertical");
		float horizontal = Input.GetAxis ("Horizontal");
		int height = kirbyAnimator.GetInteger ("Height");
		bool moving = kirbyAnimator.GetBool ("Moving");
		bool floating = kirbyAnimator.GetBool ("Floating");
		bool jumping = kirbyAnimator.GetBool ("Jumping");

		Vector2 vel = rigidbody2D.velocity;
		if (jumping)
			vel.y = Mathf.Min (vel.y + jumpSpeed / 2.0f, jumpSpeed);
		else if (!grounded) {
			if (floating)
				vel.y = Mathf.Max (vel.y + slowFallSpeed / 10.0f, slowFallSpeed);
			else
				vel.y = Mathf.Max (vel.y + fallSpeed / 5.0f, fallSpeed);
		} /*else {
			vel.y = 0;
		}*/

		if (height == 0) 
		{
			if (moving)
			{
				if (floating)
					vel.x = horizontal * slowSpeed;
				else
					vel.x = horizontal * speed;
			}
			else
				vel.x = 0;
		}
		else if (height == -1)
		{
			if (vel.x > 0.1f)
				vel.x = vel.x - drift;
			else if (vel.x < -0.1f)
				vel.x = vel.x + drift;
			else
				vel.x = 0;
		}
		else if (height == 1)
		{
			if (moving)
				vel.x = horizontal * slowSpeed;
			else
				vel.x = 0;
		}
		else
		{
			// This is when you're HOP-JUMPING, not when you're standing still
			vel.x = 0;
			vel.y = 0;
		}

		// Set height and moving parameters
		if ((Input.GetKey (KeyCode.DownArrow) && Input.GetKey ( KeyCode.UpArrow)) ||
		    (Input.GetKey (KeyCode.LeftArrow) && Input.GetKey ( KeyCode.RightArrow)))
		{
			// Pressing conflicting keys
			SetHeight (0);
			SetMoving (false);
		}
		else if (Input.GetKey (KeyCode.DownArrow))
		{
			// Pressing down only, still can't move though
			SetHeight (-1);
			SetMoving (false);
		}
		else if (Input.GetKey (KeyCode.S) && floating)
		{
			SetHeight (0);
			SetFloating (false);
			SetJumping (false);
		}
		else if (Input.GetKey (KeyCode.UpArrow))
		{
			SetHeight (1);
			SetFloating (true);
			SetJumping (true);
			if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.LeftArrow))
			{
				// SetMoving left or right
				SetMoving (true);
				Vector3 scale = transform.localScale;
				// Flip the model if you move in a different direction
				// than where you were previously facing
				if ((Input.GetKey (KeyCode.RightArrow) && direction == 0) ||
				    (Input.GetKey (KeyCode.LeftArrow) && direction == 1))
				{
					if (height != -1)
					{
						scale.x *= -1;
						direction = Mathf.Abs(direction - 1);
					}
				}
				transform.localScale = scale;
			} else {
				SetMoving (false);
			}
		}
		else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.LeftArrow))
		{
			// SetMoving left or right
			SetMoving (true);
			SetHeight (0);
			SetJumping (false);

			Vector3 scale = transform.localScale;
			// Flip the model if you move in a different direction
			// than where you were previously facing
			if ((Input.GetKey (KeyCode.RightArrow) && direction == 0) ||
			    (Input.GetKey (KeyCode.LeftArrow) && direction == 1))
			{
				if (height != -1)
				{
					scale.x *= -1;
					direction = Mathf.Abs(direction - 1);
				}
			}
			transform.localScale = scale;
		}
		else
		{
			// Not pressing anything, still shouldn't move
			SetMoving (false);
			SetJumping (false);
			SetHeight (0);
		}

		rigidbody2D.velocity = vel;
	}
}