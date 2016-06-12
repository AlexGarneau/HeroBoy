using UnityEngine;
using System.Collections;

public class PirateParrot : AbstractEnemyControl
{
	protected enum ParrotState
	{
		idle,
		position,
		launch,
		die
	}
	;
	protected ParrotState parrotState;

	private int _damage = 20;
	private int _knockback = 0;
	private float delayTime = 0;
	private float _positionTime = 3f;
	private float _launchTime = 6f;
	private float _launchVelocity = 4f;
	private GameObject _target;

	public GameObject healItem;

	protected override void Start ()
	{
		_anim = GetComponent<Animator> ();
		EnemyAbstractBehaviour[] eabs = _anim.GetBehaviours<EnemyAbstractBehaviour> ();
		for (var i = eabs.Length - 1; i >= 0; i--) {
			Debug.Log ("Behaviours: " + eabs [i]);
			eabs [i].enemy = this;
		}
		parrotState = ParrotState.idle;
	}

	protected override void Update ()
	{
		// Yes. Alright, state time.
		switch (parrotState) {
		case ParrotState.idle:
			// Idle. Do nothing. Duh.
			break;
		case ParrotState.position:
			// Parrot has its target. Float around on their x-axis.
			transform.position = Vector3.Lerp (transform.position, new Vector3 (transform.position.x, _target.transform.position.y, transform.position.z), .5f);
			delayTime -= Time.deltaTime;
			if (delayTime <= 0) {
				parrotState = ParrotState.launch;
				delayTime = _launchTime;
			}
			break;
		case ParrotState.launch:
			// Parrot fires at target!
			transform.Translate (Vector3.left * _launchVelocity * Time.deltaTime);
			delayTime -= Time.deltaTime;
			if (delayTime <= 0) {
				parrotState = ParrotState.die;
			}
			break;
		case ParrotState.die:
			// Parrot is just resting. Pining. Or maybe stunned. It's your fault. No refunds.
			Destroy (gameObject);
			break;
		}
	}

	public void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.gameObject == _target && collider.tag != "AllyHazard") {
			// Only damage the target.
			AbstractClass ac = collider.GetComponent<AbstractClass> ();
			if (ac) {
				ac.damage (_damage, AbstractDamageCollider.DamageType.medium, _knockback);
				_anim.SetTrigger ("Die");
				parrotState = ParrotState.die;
			}
		}
	}
	
	public void flyAtTarget (GameObject target)
	{
		_target = target;
		_anim.SetTrigger ("Fly");
		parrotState = ParrotState.position;
		delayTime = _positionTime;
	}
	
	public override void damage (int damage, AbstractDamageCollider.DamageType type, int knockback)
	{
		// Only has one hitpoint.
		parrotState = ParrotState.die;
		_anim.SetTrigger ("Die");
		
		// Got hit by something. Drop item.
		drop (healItem);
	}
	
	public override void onAnimationState (string animState)
	{
		Debug.Log ("ANIMATION STATE: " + animState);
		switch (animState) {
		case AbstractEnemyControl.ANIM_DEATH_END:
			// Death anim done.
			Destroy (gameObject);
			break;
		}
	}

}


