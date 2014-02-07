using UnityEngine;
using System.Collections;



public class kirbyController : MonoBehaviour {

	private Animator 	kirbyAnimator;
	private float		maxHopDistance = 5.5f;
	private float 		expectedHopDist = 3;
	private float		hopStart;
	private float		lastHeight, lastLastHeight;
	public GameObject	electricPower;
	public GameObject	firePower;
	public GameObject	noPower;
	public float 		speed = 10.5f;
	public float		slowSpeed = 5.5f;
	public float 		jumpSpeed = 7.0f;
	public float 		hopSpeed = 8f;
	public float 		hopAcc = 2f;
	public float 		fallSpeed = -14.0f;
	public float 		slowFallSpeed = -10.0f / 2.0f;
	private float 		hopFlight = 1;
	public float 		drift = 0.3f;
	public int 			direction = 1;
	public bool			grounded = false;
	public bool			overDoor = false;
	public bool			overBackDoor = false;
	public bool			usingPower = false;
	public enum power
	{none, beam, electric, fire}
	public power		currentPower = power.none;

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
		electricPower.SetActive (false);
		firePower.SetActive (false);
		noPower.SetActive (false);
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
	
	void SetHopping(bool x){
		kirbyAnimator.SetBool ("Hopping", x);
	}

	void SetHeight(int x){
		kirbyAnimator.SetInteger ("Height", x);
		/*-1 = floor squish
		 * 0 = normal
		 * 1 = inflated 
		 * 2 = hopping */
	}
	
	void ChangePosX(float deltaX) {
		Vector2 newPos = transform.position;
		newPos.x += deltaX;
		transform.position = newPos;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other) {
			if (other.tag == "Terrain") {
				grounded = true;
				SetHeight (0);
				//print ("Enter");
			}
			if (other.tag == "Door") {
				overDoor = true;
			}
			if (other.tag == "BackDoor") {
				overBackDoor = true;
			}		
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other){
			if (other.tag == "Terrain") {
				grounded = false;
				//print ("Exit");
			}
			if (other.tag == "Door") {
				overDoor = false;
			}
			if (other.tag == "BackDoor") {
				overBackDoor = false;
			}		
		}
	}

	void UsePower (power usedPower) {
		switch (usedPower)
		{
			case power.none:
				noPower.SetActive(true);
				break;
			case power.beam:
				break;
			case power.electric:
				electricPower.SetActive(true);
				break;
			case power.fire:
				firePower.SetActive(true);
				break;
		}
	}

	void haltPowers () {
		electricPower.SetActive(false);
		firePower.SetActive(false);
		noPower.SetActive(false);
	}

	void assignPower (power acquiredPower) {
		haltPowers ();
		currentPower = acquiredPower;
	}



	//*********************************//
	// Update is called once per frame //
	//*********************************//



	void Update () {
		//float vertical = Input.GetAxis ("Vertical");
		float horizontal = Input.GetAxis ("Horizontal");
		int height = kirbyAnimator.GetInteger ("Height");
		bool moving = kirbyAnimator.GetBool ("Moving");
		bool floating = kirbyAnimator.GetBool ("Floating");
		bool jumping = kirbyAnimator.GetBool ("Jumping");
		bool hopping = kirbyAnimator.GetBool ("Hopping");
		Vector3 position = transform.position;
		if (position.z != -0.2f) {
			position.z = -0.2f;
			transform.position = position;
		}

		Vector2 vel = rigidbody2D.velocity;
		if (jumping){
			vel.y = Mathf.Min (vel.y + jumpSpeed / 2.0f, jumpSpeed);
		}
		else if (!grounded) {
			if (floating)
				vel.y = Mathf.Max (vel.y + slowFallSpeed / 10.0f, slowFallSpeed);
			else if (hopping) {
				hopFlight = hopFlight + 0.35f;
				if (transform.position.y < hopStart + expectedHopDist/2.0f)
				{
					vel.y += hopAcc + 3 - hopFlight;
				}
				if (expectedHopDist < maxHopDistance)
				{
					expectedHopDist = expectedHopDist + 1;
				}
				if (transform.position.y >= hopStart + maxHopDistance) {
					vel.y = 5;
					SetHopping (false);
					SetHeight (2);
					hopping = false;
				} else {
					if (transform.position.y == lastHeight && lastHeight == lastLastHeight) {
						//vel.y = 0;
						SetHopping (false);
						hopping = false;
					} else {
						lastLastHeight = lastHeight;
						lastHeight = transform.position.y;
					}
				}
			}
			else{
				grounded = false;
				if (hopFlight > 1)
				{
					hopFlight = hopFlight - 0.6f;
					vel.y = 0;
				}
				else{
					vel.y = Mathf.Max (vel.y + fallSpeed / 5.0f, fallSpeed);;
				}
			}
		} else if (hopping) {
			vel.y = hopSpeed;
		} else {
			vel.y = slowFallSpeed;
		}

		if (height == 0) {
			if (moving) {
				if (floating)
					vel.x = horizontal * slowSpeed;
				else
					vel.x = horizontal * speed;
			} else{
				vel.x = 0;
			}
		} else if (height == -1) {
			if (vel.x > 0.1f)
				vel.x = vel.x - drift;
			else if (vel.x < -0.1f)
				vel.x = vel.x + drift;
			else
				vel.x = 0;
		} else if (height == 1) {
			if (moving)
				vel.x = horizontal * slowSpeed;
			else
				vel.x = 0;
		} else {
			// This is when you're HOP-JUMPING, not when you're standing still
			vel.x = horizontal * speed;
		}

		if (usingPower == false) {
			haltPowers ();
		}

		// Assume NOT using power
		usingPower = false;


		// If you don't hold the hop button, you stop hopping; if you land, you aren't hopping.
		if (hopping && !Input.GetKey(KeyCode.A)) {		
			SetHopping (false);
			hopping = false;
			expectedHopDist = maxHopDistance;
		}
		if ((height == -1) && !Input.GetKey (KeyCode.DownArrow) && grounded){
			SetHeight (0);
		}

		// Set height and moving parameters
		if ((Input.GetKey (KeyCode.DownArrow) && Input.GetKey ( KeyCode.UpArrow)) ||
		    (Input.GetKey (KeyCode.LeftArrow) && Input.GetKey ( KeyCode.RightArrow)))
		{
			// Pressing conflicting keys
			SetMoving (false);
		}
		else if (Input.GetKey (KeyCode.DownArrow) && !Input.GetKey (KeyCode.S) && grounded)
		{
			// Pressing down only, still can't move though
			SetHeight (-1);
			SetMoving (false);

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
		else if (Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.UpArrow))
		{
			if (floating){
				SetFloating (false);
				SetJumping (false);
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				//PUT SLIDE-KICK CODE HERE!
				SetHeight (-1); // It should be already, but there's a glitch.
			} else {
				usingPower = true;
				SetMoving (false);
				UsePower(currentPower);
			}
		}
		else if (Input.GetKey (KeyCode.UpArrow) || (Input.GetKeyDown (KeyCode.A) && floating))
		{
			if (overDoor == true)
			{
				ChangePosX(83f);
				overDoor = false;
			} else if (overBackDoor == true)
			{
				ChangePosX(-83.0f);
				overBackDoor = false;
			} else {
				SetHeight (1);
				grounded = false;
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
		}
		else if (Input.GetKeyDown (KeyCode.A) && !floating && grounded) {
			SetJumping (false);
			SetHopping (true);
			SetHeight (2);
			hopping = true;
			grounded = false;
			hopStart = transform.position.y;
			expectedHopDist = 3;
			lastHeight = -3000f;
			lastLastHeight = -6000f;
			hopFlight = 1;
		}
		else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.LeftArrow))
		{
			// SetMoving left or right
			SetMoving (true);
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
		}
		if (Input.GetKey (KeyCode.W))
		{
			currentPower = power.none;
			haltPowers();
		}

		rigidbody2D.velocity = vel;
	}
}