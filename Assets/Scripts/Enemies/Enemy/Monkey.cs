using UnityEngine;
using System.Collections;

public class Monkey : AbstractEnemyControl
{
	protected bool highGround;

	public GameObject monkeyBomb;
	public Collider2D lightHit;

	public GameObject healItem;

	protected override void Start ()
	{
        base.Start();
		base._enemHealth = 40f;
		base._enemMoveSpeed = 1f;
		base.enemDamage = 2;
		base._attackRange = 1.2f;
		base._vertRange = 0.1f;
		base.isAlive = true;
		base.isMoving = false;
        base.hasAttack = false;

        base.bulletSpawn = transform.Find ("BulletSpawn");

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
	}

	protected override void setState (EnemyStates newState)
	{
		base.setState (newState);
		switch (newState) {
		    case EnemyStates.move:
                // Reset bomb cooldown.
                _gunCooldown = _gunCooldownTime;
                _anim.SetBool ("IsMoving", true);
			    break;
            case EnemyStates.shoot:
                if (_playerControl.playerHealth > 0) {
                    // Only attack if player is still alive.
                    _anim.SetTrigger("Fire");
                    _anim.SetBool("IsMoving", false);
                } else {
                    // Stay on move.
                    setState(EnemyStates.move);
                }
                break;
            case EnemyStates.dead:
			    _anim.SetBool ("IsMoving", false);
			    break;
		}
	}

	public override void onAnimationState (string animState)
	{
		switch (animState) {
		    case AbstractEnemyControl.ANIM_SPAWN_END:
                _gunCooldown = _gunCooldownTime;
			    break;
		    case AbstractEnemyControl.ANIM_ATTACK_START:
            case AbstractEnemyControl.ANIM_SHOOT_START:
			    Shoot ();
			    break;
		    case AbstractEnemyControl.ANIM_ATTACK_END:
			    setState (baseState);
			    break;
		    case AbstractEnemyControl.ANIM_INJURED_END:
			    if (_enemHealth > 0) {
				    setState (baseState);
			    }
			    break;
		    case AbstractEnemyControl.ANIM_DEATH_END:
			    randomdrop (healItem);
			    Destroy (gameObject);
			    break;
		}
	}

	protected override void MoveToPlayer ()
	{
		//Debug.Log ("This is working: MoveToPlayer.");
		float vD = _player.transform.position.y - this.transform.position.y;
		
		// Y-positioning - move enemy to the player's level at all times.
		if (_player.transform.position.y - _vertRange > this.transform.position.y) {
			transform.Translate (new Vector3 (0, _enemMoveSpeed * Time.deltaTime, 0));
		} else if (_player.transform.position.y + _vertRange < this.transform.position.y) {
			transform.Translate (new Vector3 (0, -_enemMoveSpeed * Time.deltaTime, 0));
		}

		// Pirate is different than zombie. If he's less than half the gun range, he moves in for attack.
		// If more, then he will move back while firing his gun.
		float hD = this.transform.position.x - _player.transform.position.x;
        float targetX = this.transform.position.x;
        float targetY = this.transform.position.y;

        // At gun range (or other pirate in melee range). Back away.
        if (hD > _gunRange) {
            // Out of gun range. Move in right.
            facingLeft = true;
            normHD = -1;
            targetX = _player.transform.position.x + _gunRange;
        } else if (hD < -_gunRange) {
            // Out of gun range. Move in left.
            facingLeft = false;
            normHD = 1;
            targetX = _player.transform.position.x - _gunRange;
        } else if (hD < _gunRange && hD > 0) {
            // Within gun range. Move back right as you fire.
            facingLeft = true;
            normHD = 1;
            targetX = _player.transform.position.x + _gunRange;
        } else if (hD > -_gunRange && hD < 0) {
            // Within gun range. Move back left as you fire.
            facingLeft = false;
            normHD = -1;
            targetX = _player.transform.position.x - _gunRange;
        } else {
            // Exactly at gun range. How about that?
            normHD = 0;
            targetX = this.transform.position.x;
        }

		float targetVelX = normHD * _enemMoveSpeed;
        float targetVelY = normVD * _enemMoveSpeed;
        _vel.x = Mathf.SmoothDamp (_vel.x, targetVelX, ref velocityXSmoothing, .1f);
		_vel.y = Mathf.SmoothDamp (_vel.y, targetVelY, ref velocityYSmoothing, .1f);

		_controller.Move (_vel* Time.deltaTime);
	}

	protected override void Shoot ()
	{
		// Create a bomb and make it fly.
		GameObject go = Instantiate (monkeyBomb);
		PirateBomb bomb = go.GetComponent<PirateBomb> ();
		
		// Position the spawner and the direction.
		if (facingLeft) {
			bulletSpawn.position.Set (-Mathf.Abs (bulletSpawn.position.x), bulletSpawn.position.y, bulletSpawn.position.z);
			bomb.direction = Vector2.left;
		} else {
			bulletSpawn.position.Set (Mathf.Abs (bulletSpawn.position.x), bulletSpawn.position.y, bulletSpawn.position.z);
			bomb.direction = Vector2.right;
		}
		
		// Setup the bomb's spawn and target. It will animate itself from spawn to the target by means of physics!
		bomb.setSpawnAndTarget (bulletSpawn.position, new Vector2 (_player.transform.position.x, _player.transform.position.y));
		
		// Put the bomb on the stage.
		bomb.transform.parent = transform.parent;
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
			_anim.SetTrigger ("IsHit");
			break;
		case AbstractDamageCollider.DamageType.heavy:
			_anim.SetTrigger ("IsHit");
			break;
		}
	}

	public override void stun (float timeInSec)
	{
		base.stun (timeInSec);
		_anim.SetTrigger ("IsStunned");
	}
}
