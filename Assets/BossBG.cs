using UnityEngine;
using System.Collections;

public class BossBG : MonoBehaviour {

	public Material bossFrame01;
	public Material bossFrame02;
	public Material bossFrame03;
	public Material bossFrame04;
	public Material bossFrame05;
	public Material bossFrame06;
	public Material bossFrame07;
	public Material bossFrame08;
	public Material bossFrame09;
	public Material bossFrame10;
	public Material bossFrame11;
	public Material bossFrame12;
	public Material bossFrame13;
	public Material bossFrame14;
	public Material bossFrame15;
	public Material bossFrame16;
	public Material bossFrame17;
	public Material bossFrame18;
	public Material bossFrame19;
	public Material bossFrame20;
	public Material bossFrame21;
	public Material bossFrame22;
	public Material bossFrame23;
	public Material bossFrame24;
	public Material bossFrame25;
	private Material[] bossFrames;
	int currentFrame;
	private float counter; 

	// Use this for initialization
	void Start () {
		bossFrames = new Material[]{bossFrame01,bossFrame02,bossFrame03,bossFrame04,bossFrame05,
			bossFrame06,bossFrame07,bossFrame08,bossFrame09,bossFrame10,bossFrame11,bossFrame12,
			bossFrame13,bossFrame14,bossFrame15,bossFrame16,bossFrame17,bossFrame18,bossFrame19,
			bossFrame20,bossFrame21,bossFrame22,bossFrame23,bossFrame24,bossFrame25};
		renderer.material = bossFrames[0];
		currentFrame = 0;
		counter = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		counter = (counter + 1) % 7;
		if (counter == 4) {
			currentFrame = (currentFrame + 1) % 25;
			renderer.material = bossFrames[currentFrame];
		}
	}
}
