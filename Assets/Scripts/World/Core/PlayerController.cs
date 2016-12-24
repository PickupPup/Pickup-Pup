using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Agent
{

	//script to receive key inputs and move player sprite

	//Speed variable public for easy manipulation
	public float baseSpeed = 2;
	float speed = 2;

	//current movement direction
	float xVec;
	float yVec;

	//enums for input status
	enum xDir
	{
		pos,
		none,
		neg}

	;

	enum yDir
	{
		pos,
		none,
		neg}

	;

	//enum vars
	xDir xIn;
	yDir yIn;

	//-----------------------------------------------------------------------------------------


	// Use this for initialization
	void Start ()
	{
		xVec = 0;
		yVec = 0;

		xIn = xDir.none;
		yIn = yDir.none;

	}
	//-----------------------------------------------------------------------------------------

	// Update is called once per frame
	void Update ()
	{
		KeyListener ();

		ApplyMotion ();

		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}

	}
	//-----------------------------------------------------------------------------------------
	void KeyListener ()
	{
		//listen for xInput, set xInput enum
		if (Input.GetKey (KeyCode.D) && !Input.GetKey (KeyCode.A)) {
			xIn = xDir.pos;
			xVec = 1;
		} else if (Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.D)) {
			xIn = xDir.neg;
			xVec = -1;
		} else {
			xIn = xDir.none;
			xVec = 0;
		}

		//listen for yInput, set yInput enum
		if (Input.GetKey (KeyCode.W) && !Input.GetKey (KeyCode.S)) {
			yIn = yDir.pos;
			yVec = 1;
		} else if (Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.W)) {
			yIn = yDir.neg;
			yVec = -1;
		} else {
			yIn = yDir.none;
			yVec = 0;
		}
		//'Sprint' because why not
		if (Input.GetKey (KeyCode.LeftShift)) {
			speed = baseSpeed * 2f;
		} else {
			speed = baseSpeed;
		}
					

	}
	//-----------------------------------------------------------------------------------------

	void ApplyMotion ()
	{
		transform.position = new Vector3 (transform.position.x + ((xVec * speed)*Time.deltaTime), transform.position.y + ((yVec * speed) * Time.deltaTime));
	}

}





