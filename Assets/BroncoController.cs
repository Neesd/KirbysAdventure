using UnityEngine;
using System.Collections;

public class BroncoController : MonoBehaviour {

	public GameObject parent;
	private Animator BroncoAnimator;
	public Transform target;
	public string type = "ground";
	public float moveSpeed = 7.5f;
	private float spawnDirection = 0;
	private float prevDirection = -1;
	private float dips = 5;
	private float counter = 1;
	private float dist = 2.4f;
	private float curveDirection = -1;
	private float height = 5;
	private Vector3 originalPos;
	private bool started = false;
	
	void Flip()
	{
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
		BroncoAnimator = this.GetComponent<Animator> ();
		// Adjusts the framerate of the animations
		BroncoAnimator.speed = 0.5f;
		BroncoAnimator = this.GetComponent<Animator> ();
		spawnDirection = -1 * transform.localScale.x / 13;	
		
		BoxCollider2D boxCol = this.GetComponent<BoxCollider2D> ();
		Vector3 boxSize = boxCol.size;
		boxSize.x = 0.15f;
		boxSize.y = 0.15f;
		boxCol.size = boxSize;

		originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPos = target.position;
		Vector3 position = transform.position;
		if (!onScreen (position))
		{
			Die ();
		}

		if (type == "ground")
		{
			position.x += spawnDirection * moveSpeed * Time.deltaTime;
			position.y += moveSpeed * Time.deltaTime * 0.75f;
		}
		else if (type == "sky")
		{
			position.x += spawnDirection * moveSpeed * Time.deltaTime;
			if (dips > 0)
			{
				position.y += curveDirection * counter * counter * 0.05f;
				if (((originalPos.y - position.y < height/2) && curveDirection == -1) ||
				    ((originalPos.y - position.y > height/2) && curveDirection == 1))
				{
					counter = counter + 0.05f;
				}
				else if (((originalPos.y - position.y >= height/2) && curveDirection == -1) ||
				         ((originalPos.y - position.y <= height/2) && curveDirection == 1))
				{
					counter = counter - 0.05f;
				}
				if (counter <= 1)
				{
					dips = dips - 1;
					curveDirection *= -1;
				}
			}
			else
			{
				position.y += curveDirection * counter * counter * 0.05f;
				counter = counter + 0.05f;
			}
			
		}
		else if (type == "platform")
		{
			// First, don't move the bug until Kirby is close
			if ((Mathf.Abs (target.position.x - position.x) > 8) && !started)
			{
				BroncoAnimator.enabled = false;
				curveDirection = 1;
				height = 5 - 2.8f;
			}
			else if (curveDirection != 0)
			{
				started = true;
				BroncoAnimator.enabled = true;
				if ((position.y <= originalPos.y + 2.8f) && curveDirection == -1)
				{
					curveDirection = 0;
					counter = 0;
				}
				else if ((position.y < originalPos.y + 5) && curveDirection == 1)
				{
					position.y += curveDirection * counter * counter * 0.05f;
					counter += 0.05f;
				}
				else if ((position.y < originalPos.y + 5) && curveDirection == -1)
				{
					position.y += curveDirection * counter * counter * 0.07f;
					if ((originalPos.y + 5 - position.y < height/2) && curveDirection == -1)
					{
						counter = counter + 0.03f;
					}
					else if ((originalPos.y + 5 - position.y >= height/2) && curveDirection == -1)
					{
						counter = counter - 0.03f;
					}
				}
				else if (position.y >= originalPos.y + 5)
				{
					curveDirection = -1;
					position.y += curveDirection * counter * counter * 0.07f;
					counter = 0.3f;
				}
			}
			else
			{
				if (counter > 1)
				{
					if (position.x == originalPos.x)
					{

						prevDirection = -1 * transform.localScale.x / 13;
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
					}
					position.x += spawnDirection * counter * counter * moveSpeed * Time.deltaTime;
					counter += 0.04f;
				}
				else
				{
					counter += 0.2f;
				}
				
			}
		}
		else if (type == "homing"){
			dist += 0.005f;
			position.x += spawnDirection * dist * Time.deltaTime;

			if (targetPos.y > position.y)
				position.y += dist * Time.deltaTime;
			else if (targetPos.y < position.y)
				position.y -= dist * Time.deltaTime;

			prevDirection = -1 * transform.localScale.x / 13;
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

		}
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
				col.gameObject.SendMessage ("addPoints", 1200);
				Die ();
			}
		}
	}
	
	void Die ()
	{
		started = false;
		counter = 1;
		dist = 2.4f;
		parent.SendMessage("kill", spawnDirection);
	}
}
