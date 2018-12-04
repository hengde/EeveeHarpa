using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralCircle : MonoBehaviour {

	public float r;
	public float R;
	public float theta;
	public float thetaRad;
	public float speed;

	int gameWidth = 60;
	int gameHeight = 60;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		theta = theta > 359.99 ? 0 : theta;
		theta += speed;
		thetaRad = theta * Mathf.Deg2Rad;
		transform.localPosition = new Vector3(
			(gameWidth/2 + (R-r) * Mathf.Cos(thetaRad) + r * Mathf.Cos((R/r-1)*thetaRad)),
			(gameHeight/2 + (R-r) * Mathf.Sin(thetaRad) + r * Mathf.Sin((R/r-1)*thetaRad)),
			transform.localPosition.z
		);
	}
}
