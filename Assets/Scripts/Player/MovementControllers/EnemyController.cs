using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class EnemyController : MonoBehaviour
{
	public int enemyHealth;

	public float tempInvulnTimer;
	
	[HideInInspector]
	public float
		timerReset = 0f;


	public float jumpHeight = 4f;
	public float timeToJumpApex = .35f;
	float accelTimeAirbourne = 0.2f;
	float accelTimeGrounded = 0.1f;

	float moveSpeed = 3f;
	bool moving;

	float jumpVel;
	float _gravity;
	Vector3 _vel;

	float velocityXSmoothing;

	Controller2D _controller;

	Animator _anim;

	public GameObject _player;

	void Start ()
	{
		enemyHealth = 100;
		_controller = GetComponent<Controller2D> ();
		_gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVel = Mathf.Abs (_gravity) * timeToJumpApex;
		_anim = GetComponent<Animator> ();
		//_player = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update ()
	{
		EnemyMovement ();
		if (_controller._collisions.above || _controller._collisions.below) {
			_vel.y = 0;
		}

		_vel.y += _gravity * Time.deltaTime;
		_controller.Move (_vel * Time.deltaTime);
	}

	void EnemyMovement ()
	{
		Vector2 _enemPos = this.transform.position;
		Vector2 _playPos = _player.transform.position;
		float distance = Vector2.Distance (_enemPos, _playPos);

		if (distance > 2 || distance < -2) {
			moving = true;
		}
		if (moving == true) {
			if (_enemPos.x < _playPos.x) {
				transform.Translate ((Vector2.right * moveSpeed) * Time.deltaTime);
			} else if (_enemPos.x > _playPos.x) {
				transform.Translate ((-Vector2.right * moveSpeed) * Time.deltaTime);
			}
		}
		if (distance > -2 || distance < 2) {
			moving = false;
			//Check if player is far above or below enemy. If true, do not attack.
			//else RNG between light attack, heavy attack, long-range attack, or block.
		}
	}
}
