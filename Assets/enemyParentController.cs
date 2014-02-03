using UnityEngine;
using System.Collections;

public class enemyParentController : MonoBehaviour {

	public GameObject child;
	public Transform target;
	private Vector3 spawnPoint;
	private bool spawned;
	private bool spawnReady;
	private float spawnDirection;
	private float prevDirection = -1;
	
	void Flip (){
		Vector3 scale = child.transform.localScale;
		scale.x *= -1;
		child.transform.localScale = scale;
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

	void kill(float direction) {
		prevDirection = direction;
		child.SetActive (false);
		spawned = false;
	}

	// Use this for initialization
	void Start () {
		spawnPoint = this.transform.position;
		if (!onScreen (spawnPoint)) {
			child.SetActive (false);
			spawned = false;
			spawnReady = true;
		}
		else {
			child.SetActive (true);
			spawned = true;
			spawnReady = false;
		}

		Vector3 targetPos = target.position;
		Vector3 position = transform.position;

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
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPos = target.position;
		if (!spawned && spawnReady && onScreen(spawnPoint))
		{
			child.transform.position = spawnPoint;
			if (targetPos.x < spawnPoint.x)
			{
				spawnDirection = -1;
			}
			else if (targetPos.x > spawnPoint.x)
			{
				spawnDirection = 1;
			}
			if (prevDirection != spawnDirection){
				prevDirection = spawnDirection;
				Flip ();
			}
			child.SetActive (true);
			spawned = true;
			spawnReady = false;

		}
		else if (!spawned && !spawnReady && !onScreen(spawnPoint)){
			spawnReady = true;
		}
	
	}
}
