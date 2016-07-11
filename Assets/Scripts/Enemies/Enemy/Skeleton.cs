using UnityEngine;
using System.Collections;

public class Skeleton : AbstractEnemyControl
{
	public Collider2D lightHit;
	public GameObject healItem;

	protected override void Start ()
	{
        base.Start();
		base._enemHealth = 200f;
		base._enemMoveSpeed = 1f;
		base._enemDamage = 2;
		base._attackRange = 1.2f;
		base._vertRange = 0.2f;
		base.isAlive = true;
		base.isMoving = false;

        _controller = GetComponent<MovementController2D> ();

		_damageColliders = GetComponentsInChildren<EnemyDamageCollider> ();
		if (_damageColliders != null && _damageColliders.Length > 0) {
			// Sets the damage of damage colliders. TODO: Independent damage set to different colliders, if multple exist.
			for (int i = _damageColliders.Length - 1; i >= 0; i--) {
				_damageColliders [i].gameObject.SetActive (false);
			}
		}

		EnemyAbstractBehaviour[] eabs = _anim.GetBehaviours<EnemyAbstractBehaviour> ();
		for (var i = eabs.Length - 1; i >= 0; i--) {
			eabs [i].enemy = this;
		}
        _anim.SetFloat("Health", _enemHealth);

        // Set the first state.
        setState (EnemyStates.spawn);
	}

	protected override void Update ()
	{
        Debug.Log("SKELETON UPDATE: " + _enemHealth);

		switch (state) {
		case EnemyStates.move:
			break;
		case EnemyStates.attack:
			break;
		case EnemyStates.dead:
			//DeathTimerDestroy ();
			break;
		}

		_anim.SetFloat ("Health", _enemHealth);
		_anim.SetBool ("FacingLeft", facingLeft);
        _anim.SetInteger("PlayerHealth", _playerControl.playerHealth);

		base.Update ();
	}

	protected override void setState (EnemyStates newState)
	{
		switch (newState) {
		case EnemyStates.move:
			_anim.SetBool ("IsMoving", true);
			break;
		case EnemyStates.attack:
			_anim.SetBool ("IsMoving", false);
			_anim.SetTrigger ("Attack");
			break;
		case EnemyStates.dead:
			_anim.SetBool ("IsMoving", false);
			break;
		}

		base.setState (newState);
	}

	public override void onAnimationState (string animState)
	{
		switch (animState) {
		case AbstractEnemyControl.ANIM_SPAWN_END:
			setState (EnemyStates.move);
			break;
		case AbstractEnemyControl.ANIM_ATTACK_START:

			break;
		case AbstractEnemyControl.ANIM_ATTACK_END:
            if (state != EnemyStates.stun)
            {
                setState(EnemyStates.move);
            }
            break;
		case AbstractEnemyControl.ANIM_INJURED_END:
			if (_enemHealth > 0 && state != EnemyStates.stun) {
				setState (EnemyStates.move);
			}
			break;
        case AbstractEnemyControl.ANIM_STUN_END:
            _anim.SetBool("IsStunned", false);
            setState(EnemyStates.move);
            break;
		case AbstractEnemyControl.ANIM_DEATH_END:
			randomdrop (healItem);
			Destroy (gameObject);
			break;
		}
	}

	public override void damage (int damage, AbstractDamageCollider.DamageType type, int knockback)
	{
		base.damage (damage, type, knockback);

		if (facingLeft == true) {
			xForce = knockback * .01f;
		} else {
			xForce = -knockback * .01f;
		}

		switch (type) {
		case AbstractDamageCollider.DamageType.light:
			_anim.SetTrigger ("IsHit");
			break;
		case AbstractDamageCollider.DamageType.medium:
			_anim.SetTrigger ("IsHit2");
			break;
		case AbstractDamageCollider.DamageType.heavy:
            _anim.SetTrigger("IsHit2");
            _anim.SetBool("IsStunned", true);
            setState(EnemyStates.stun);
            break;
		}
	}

	public override void stun (float timeInSec)
	{
		base.stun (timeInSec);
		_anim.SetTrigger ("IsStunned");
	}
}
