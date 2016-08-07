using UnityEngine;
using System.Collections;

public class RobotGun : AbstractEnemyControl
{
	bool highGround;
	float gunRangeMax = 5f;
	float gunCooldown = 0f;
	float gunCooldownTime = 4f;

	public Collider2D lightHit;

	public GameObject healItem;
	public GameObject pirateBullet;

	protected override void Start ()
	{
        base.Start();
		base._enemHealth = 50f;
		base._enemMoveSpeed = 1.2f;
		base._enemDamage = 2;
		base._attackRange = 1.2f;
		base._vertRange = 0.1f;
		base.isAlive = true;
		base.isMoving = false;

        base.bulletSpawn = transform.Find ("BulletSpawn");
		_controller = GetComponent<MovementController2D> ();

		_damageColliders = GetComponentsInChildren<EnemyDamageCollider> ();
		if (_damageColliders != null && _damageColliders.Length > 0) {
			// Sets the damage of damage colliders. TODO: Independent damage set to different colliders, if multple exist.
			for (int i = _damageColliders.Length - 1; i >= 0; i--) {
				_damageColliders [i].gameObject.SetActive (false);
			}
		}

		//Debug.Log (GetComponents<Animator> ().Length);
		EnemyAbstractBehaviour[] eabs = _anim.GetBehaviours<EnemyAbstractBehaviour> ();
		for (var i = eabs.Length - 1; i >= 0; i--) {
			eabs [i].enemy = this;
		}

		// Set the first state. DEBUG
		setState (EnemyStates.move);
	}

	protected override void Update ()
	{
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
        _anim.SetInteger("PlayerHealth", _playerControl.playerHealth);
        //_anim.SetBool ("HighGround", highGround);
        //HighGroundCheck ();

        if (meleeCooldown > 0)
        {
            meleeCooldown -= Time.deltaTime;
            if (meleeCooldown < 0)
            {
                meleeCooldown = 0;
            }
        }

        if (gunCooldown > 0) {
			gunCooldown -= Time.deltaTime;
			if (gunCooldown < 0) {
				gunCooldown = 0;
			}
		}

		base.Update ();
	}

	protected override void setState (EnemyStates newState)
	{
        Debug.Log("SetState: " + newState);

		switch (newState) {
		case EnemyStates.move:
			_anim.SetBool ("IsMoving", true);
			break;
		case EnemyStates.attack:
			if (Mathf.Abs (transform.position.x - _player.transform.position.x) > _attackRange) {
				// Too far for melee. Shoot.
				if (gunCooldown > 0) {
					// Wait, gun's not ready yet. Don't switch state.
					return;
				} else {
					// OK, gun's ready. Set cooldown and fire.
					gunCooldown = gunCooldownTime;
					_anim.SetTrigger ("Fire");
				}
			} else if (meleeCooldown <= 0) {
                    _anim.SetTrigger (Random.value > .5 ? "Attack1" : "Attack2");
			}
			_anim.SetBool ("IsMoving", false);
			break;
        case EnemyStates.stun:
            _anim.SetBool("IsMoving", false);
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
		case AbstractEnemyControl.ANIM_PISTOL_FIRE:
			Shoot ();
			break;
		}
	}

	protected override void MoveToAttack ()
	{
		//Debug.Log ("This is working: MoveToAttack.");
		float vD = this.transform.position.y - _player.transform.position.y;
		
		// Y-positioning - move enemy to the player's level at all times.
		if (_player.transform.position.y - _vertRange > this.transform.position.y) {
			normVD = 1;
		} else if (_player.transform.position.y + _vertRange < this.transform.position.y) {
			normVD = -1;
		} else {
			normVD = 0;
		}

		// Pirate is different than zombie. If he's less than half the gun range, he moves in for attack.
		// If more, then he will move back while firing his gun.
		float hD = this.transform.position.x - _player.transform.position.x;
		var rangedToMeleePoint = gunRangeMax * .5;
		float targetX = this.transform.position.x;
		float targetY = this.transform.position.y;

		if (Mathf.Abs (hD) > rangedToMeleePoint || ((pRangeCollider.inRangeLeft && hD > 0) || (pRangeCollider.inRangeRight && hD < 0))) {
			// At gun range (or other pirate in melee range). Back away.
			if (hD > gunRangeMax) {
				// Out of gun range. Move in right.
				facingLeft = true;
				normHD = -1;
				targetX = _player.transform.position.x + gunRangeMax;
			} else if (hD < -gunRangeMax) {
				// Out of gun range. Move in left.
				facingLeft = false;
				normHD = 1;
				targetX = _player.transform.position.x - gunRangeMax;
			} else if (hD < gunRangeMax && hD > 0) {
				// Within gun range. Move back right as you fire.
				facingLeft = true;
				normHD = 1;
				targetX = _player.transform.position.x + gunRangeMax;
				if (vD <= _vertRange && vD >= -_vertRange) {
					setState (EnemyStates.attack);
				}
			} else if (hD > -gunRangeMax && hD < 0) {
				// Within gun range. Move back left as you fire.
				facingLeft = false;
				normHD = -1;
				targetX = _player.transform.position.x - gunRangeMax;
				if (vD <= _vertRange && vD >= -_vertRange) {
					setState (EnemyStates.attack);
				}
			} else {
				// Exactly at gun range. How about that?
				normHD = 0;
				targetX = this.transform.position.x;
				if (vD <= _vertRange && vD >= -_vertRange) {
					setState (EnemyStates.attack);
				}
			}
		} else {
			// Go in for the melee.
			// X-positioning - move enemy to its closest horizontal range. If too close to the player, back away.
			//Debug.Log (hD + " - " + _attackRange);
			if (hD > _attackRange) {
				// Zombie is to the left of player. Move right.
				facingLeft = true;
				normHD = -1;
				targetX = _player.transform.position.x + _attackRange;
			} else if (hD < -_attackRange) {
				// Zombie is to the right of player. Move left.
				facingLeft = false;
				normHD = 1;
				targetX = _player.transform.position.x - _attackRange;
			} else if (hD < _attackRange - 0.1f && hD >= 0) {
				// Zombie is to the left of player, but too close. Move left.
				facingLeft = true;
				normHD = 1;
				targetX = _player.transform.position.x + _attackRange;
			} else if (hD > -_attackRange + 0.1f && hD < 0) {
				// Zombie is to the right of player, but too close. Move right.
				facingLeft = false;
				normHD = -1;
				targetX = _player.transform.position.x - _attackRange;
			} else {
				normHD = 0;
			}

			if (hD <= _attackRange && hD >= -_attackRange && vD <= _vertRange && vD >= -_vertRange) {
				setState (EnemyStates.attack);
			}
		}

		float targetVelX = normHD * _enemMoveSpeed;
		float targetVelY = normVD * _enemMoveSpeed;
		_vel.x = Mathf.SmoothDamp (_vel.x, targetVelX, ref velocityXSmoothing, .1f);
		_vel.y = Mathf.SmoothDamp (_vel.y, targetVelY, ref velocityYSmoothing, .1f);

		_controller.Move (_vel * Time.deltaTime);

		_anim.SetBool ("FacingLeft", facingLeft);
	}

	protected override void Shoot ()
	{
		GameObject go;
		PirateBullet bullet;
		if (facingLeft) {
			go = Instantiate (pirateBullet);
			bullet = go.GetComponent<PirateBullet> ();
			bulletSpawn.position.Set (-Mathf.Abs (bulletSpawn.position.x), bulletSpawn.position.y, bulletSpawn.position.z);
			bullet.direction = Vector2.left;
		} else {
			go = Instantiate (pirateBullet);
			bullet = go.GetComponent<PirateBullet> ();
			bulletSpawn.position.Set (Mathf.Abs (bulletSpawn.position.x), bulletSpawn.position.y, bulletSpawn.position.z);
			bullet.direction = Vector2.right;
		}
	
		/*
		// Create a bullet and make it fly.
		PirateBullet bullet = BulletFactory.createPirateBullet ();

		// Position the spawner and the direction.
		if (facingLeft) {
			bulletSpawn.position.Set (-Mathf.Abs (bulletSpawn.position.x), bulletSpawn.position.y, bulletSpawn.position.z);
			bullet.direction = Vector2.left;
		} else {
			bulletSpawn.position.Set (Mathf.Abs (bulletSpawn.position.x), bulletSpawn.position.y, bulletSpawn.position.z);
			bullet.direction = Vector2.right;
		}
		*/

		// Stick the bullet in the spawner.
		bullet.transform.position = bulletSpawn.position;
		
		// Put the bullet on the stage.
		bullet.transform.parent = transform.parent;
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
			_anim.SetTrigger ("IsHit2");
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

	/*void HighGroundCheck ()
	{
		if (transform.position.y > 0) {
			highGround = true;
		} else {
			highGround = false;
		}
	}*/
}
