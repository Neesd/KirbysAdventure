using UnityEngine;
using System.Collections;


public class kirbyController : MonoBehaviour {

	private Animator 	kirbyAnimator;
	public HUDcontroller	hud;
	public GameObject	beamPower;
	public GameObject	electricPower;
	public GameObject	firePower;
	public GameObject	noPower;
	public float 		speed = 10.5f;
	public float		slowSpeed = 5.5f;
	public float 		jumpSpeed = 7.0f;
	public float 		hopSpeed = 13;
	private bool 		initHop = false;
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
	private float		initialTime;
	private bool		powerInterrupt = false;

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
		beamPower.SetActive (false);
		electricPower.SetActive (false);
		firePower.SetActive (false);
		noPower.SetActive (false);
		initialTime = Time.time - 20f;
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

	void SetFalling(bool x){
		kirbyAnimator.SetBool ("Falling", x);
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
			if (other.tag == "Terrain" || other.tag == "fancyPlatform") {
				grounded = true;
				SetHeight (0);
				SetHopping (false);
				SetFalling (false);
			}
			if (other.tag == "Door") {
				overDoor = true;
			}
			if (other.tag == "BackDoor") {
				overBackDoor = true;
			}
			if (other.tag == "KillBlock") {
				hud.die();
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other){
			if (other.tag == "Terrain" || other.tag == "fancyPlatform") {
				grounded = false;
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
				beamPower.SetActive(true);
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
		beamPower.SetActive(false);
		electricPower.SetActive(false);
		firePower.SetActive(false);
		noPower.SetActive(false);
	}

	public void assignPower (power acquiredPower) {
		haltPowers ();
		powerInterrupt = true;
		currentPower = acquiredPower;
	}

	public void addPoints(int num){
		hud.addPoints (num);
	}

	public void getHit () {
		if (Time.time - initialTime >= 2) {
			initialTime = Time.time;
			currentPower = power.none;
			powerInterrupt = true;
			haltPowers ();
			hud.loseHealth();
		}
	}

	public void reset () {
		Vector3 position = new Vector3 (5.405118f + (transform.position.x - transform.position.x % 200f), 5.086665f, -0.2f);
		transform.position = position;
		currentPower = power.none;
		haltPowers ();
	}

	public void superReset () {
		Vector3 position = new Vector3 (5.405118f, 5.086665f, -0.2f);
		transform.position = position;
		currentPower = power.none;
		haltPowers ();
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
				if (hopFlight < 15){
					hopFlight = hopFlight + 1;
				}
				else {
					SetHopping (false);
					initHop = false;
				}
				if (initHop)
				{
					vel.y = hopSpeed + hopFlight;
					initHop = false;
				}

				if (vel.y <= 0){
					SetFalling (true);
				}
			}
			else {
				if (vel.y <= 0){
					SetFalling (true);
				}
				vel.y -= hopSpeed * Time.deltaTime * 2.6f;
			}
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
			if (currentPower != power.none)
				haltPowers ();
		} else {
			vel.x = 0f;
		}

		// Assume NOT using power
		usingPower = false;


		// If you don't hold the hop button, you stop hopping; if you land, you aren't hopping.
		if (hopping && !Input.GetKey(KeyCode.Z)) {		
			SetHopping (false);
			hopping = false;
		}
		if ((height == -1) && !Input.GetKey (KeyCode.DownArrow) && grounded){
			SetHeight (0);
		}

		if (!Input.GetKey (KeyCode.X) || Input.GetKey (KeyCode.UpArrow)) {
			// No perma inhale
			haltPowers();
		}

		// Set height and moving parameters
		if ((Input.GetKey (KeyCode.DownArrow) && Input.GetKey ( KeyCode.UpArrow)) ||
		    (Input.GetKey (KeyCode.LeftArrow) && Input.GetKey ( KeyCode.RightArrow)))
		{
			// Pressing conflicting keys
			SetMoving (false);
		}
		else if (Input.GetKey (KeyCode.DownArrow) && !Input.GetKey (KeyCode.X) && grounded)
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
		else if (Input.GetKey (KeyCode.X) && !Input.GetKey (KeyCode.UpArrow) && floating)
		{
			if (floating){
				SetFloating (false);
				SetJumping (false);
			}
		} else if ((Input.GetKeyDown (KeyCode.X) || (Input.GetKey (KeyCode.X) && !powerInterrupt))&& !Input.GetKey (KeyCode.UpArrow)) {
			if (Input.GetKey (KeyCode.DownArrow)) {
				//PUT SLIDE-KICK CODE HERE!
				SetHeight (-1); // It should be already, but there's a glitch.
			} else {
				powerInterrupt = false;
				usingPower = true;
				SetMoving (false);
				UsePower(currentPower);
			}
		}
		else if (Input.GetKey (KeyCode.UpArrow) || (Input.GetKey (KeyCode.Z) && floating))
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
				SetFalling (false);
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
		else if (Input.GetKeyDown (KeyCode.Z) && !floating && grounded) {
			SetJumping (false);
			SetHopping (true);
			SetFalling (false);
			SetHeight (2);
			hopping = true;
			grounded = false;
			initHop = true;
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
		if (Input.GetKey (KeyCode.Space))
		{
			currentPower = power.none;
			haltPowers();
		}

		rigidbody2D.velocity = vel;
	}
}