using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralCircleManager : MonoBehaviour {

	public GameObject circlePrefab;
	public int numCircles;
	public List<SpiralCircle> circles;

	// Use this for initialization
	void Start () {
		for(int i=0; i< numCircles; i++) {
			SpiralCircle circle = Instantiate(circlePrefab, Vector3.zero, Quaternion.identity).GetComponent<SpiralCircle>();
			circle.transform.SetParent(transform);
			circle.R = Random.Range(8f, 30f);
			circle.r = Random.Range(3f, 5f);
			circle.theta = Mathf.Floor(Random.Range(0f,360f));
			circle.speed = Random.Range(5f,10f);
			circle.speed = circle.speed == 0 ? -1 : circle.speed;
			circle.speed *= 2/circle.R;
			circles.Add(circle);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
