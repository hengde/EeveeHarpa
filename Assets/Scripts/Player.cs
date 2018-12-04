using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float moveSpeed;
	public KeyCode upKey;
	public KeyCode downKey;
	public KeyCode leftKey;
	public KeyCode rightKey;
	public KeyCode btn1;
	public KeyCode btn2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(upKey)){
			transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
		}
		if(Input.GetKeyDown(downKey)){
			transform.Translate(Vector3.down * Time.deltaTime * moveSpeed);
		}
		if(Input.GetKeyDown(leftKey)){
			transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
		}
		if(Input.GetKeyDown(rightKey)){
			transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
		}

		if(Input.GetKeyDown(btn1)) {
			spawnCircle();
		}
	}

	public void spawnCircle(){
		
	}
}
