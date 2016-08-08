using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovementController2D))]
public class PlayerController : MonoBehaviour
{
	public int playerHealth;

	public float tempInvulnTimer;

	[HideInInspector]
	public float
		timerReset = 0f;

	public float jumpHeight = 4f;
	public float timeToJumpApex = .35f;
	float accelTimeAirbourne = 0.2f;
	float accelTimeGrounded = 0.1f;

	float moveSpeed = 6f;

	float jumpVel;
	float _gravity;
	Vector3 _vel;

	float velocityXSmoothing;

	bool aimHigh;
	bool aimLow;

	[HideInInspector]
	public bool
		facingLeft;
	[HideInInspector]
	public bool
		facingRight;

	Controller2D _controller;

	Animator _anim;

	void Start ()
	{
		playerHealth = 100;
		_controller = GetComponent<Controller2D> ();
		_gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVel = Mathf.Abs (_gravity) * timeToJumpApex;
		_anim = GetComponent<Animator> ();

		facingRight = true;
		facingLeft = false;
	}

	void Update ()
	{
		if (_controller._collisions.above || _controller._collisions.below) {
			_vel.y = 0;
		}

		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		if (input.x < 0) {
			facingRight = false;
			facingLeft = true;
		} else if (input.x > 0) {
			facingRight = true;
			facingLeft = false;
		}

		if (Input.GetKeyDown (KeyCode.Space) && _controller._collisions.below) {
			_vel.y = jumpVel;
		}

		if (Input.GetKey (KeyCode.UpArrow)) {
			aimHigh = true;
		} else {
			aimHigh = false;
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			aimLow = true;
		} else {
			aimLow = false;
		}

		// Splice: Create a TimerController class. Uses a global float that indicates time. 
		// Set it up so that you send a function (like Update or Start) and a time, in seconds. After the time passes, run the function. Simple.
		// Timer function is responsible for updating itself. Either make it a monobehavior and use update to do so, or figure out a way to run a timer or frame based update.
		// If it takes in arguments, also allow you to pass an object or struct of variables.
		if (Input.GetKeyDown (KeyCode.A)) {
			if (aimHigh == true) {
				//fast high punch
			} else if (aimLow == true) {
				//fast kneecapper
			} else {
				//lightpunch
			}
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			if (aimHigh == true) {
				//thrust kick
			} else if (aimLow == true) {
				//low thrust kick
			} else {
				//Light kick
			}
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			if (aimHigh == true) {
				//Uppercut
			} else if (aimLow == true) {
				//Leg-grab
			} else {
				//heavypunch
			}
		}
		if (Input.GetKeyDown (KeyCode.X)) {
			if (aimHigh == true) {
				//High Roundhouse
			} else if (aimLow == true) {
				//Sweep
			} else {
				//Roundhouse
			}
		}

		float targetVelX = input.x * moveSpeed;
		_vel.x = Mathf.SmoothDamp (_vel.x, targetVelX, ref velocityXSmoothing, (_controller._collisions.below) ? accelTimeGrounded : accelTimeAirbourne);
		_vel.y += _gravity * Time.deltaTime;
		_controller.Move (_vel * Time.deltaTime);
	}
}
