using UnityEngine;
using System.Collections;

public class Clown : AbstractEnemyControl
{
	bool highGround;
	float gunRangeMax = 5f;
	float gunCooldown = 0f;
	float gunCooldownTime = 4f;

	public Collider2D lightHit;

	public GameObject healItem;
	public GameObject clownWater;

    private GameObject[] bodyParts;
    private bool doShudder = false;
    private float shudderIntensity = 4f;

    // Phasing variables.
    private float phaseDelay = 1f;
    private float phaseTime = 0f;
    private float phaseProgress = 0f;
    private float phaseDuration = .5f;
    private float phaseChance = .1f;
    private Vector2 phaseDirection;
    private float phaseDistance = .2f;
    private bool isPhasing = false;

	protected override void Start ()
	{
        base.Start();
		base._enemHealth = 50f;
		base._enemMoveSpeed = .9f;
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

        // Get body parts.
        Transform body = transform.FindChild("Clownsprite");
        bodyParts = new GameObject[body.childCount];
        for (int i = body.childCount - 1; i >= 0; i--) {
            bodyParts[i] = body.GetChild(i).gameObject;
        }

        // Set the first state.
        setState (EnemyStates.move);
	}

	protected override void Update ()
	{
		switch (state) {
		    case EnemyStates.move:
                if (isPhasing) {
                    // Phasing is a quick little jump in a random direction. Can be used for dodging, homing in, etc. In this case, it's meant to make the clowns less predictable.
                    phaseProgress += Time.deltaTime;
                    if (phaseProgress >= phaseDuration) {
                        // Phase complete.
                        phaseProgress = phaseDuration;
                        isPhasing = false;
                    } else {
                        // Phase the clown!
                        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
                        Vector2 dest = new Vector2(pos.x + phaseDirection.x * phaseDistance, pos.y + phaseDirection.y * phaseDistance);
                        pos = Vector2.Lerp(pos, dest, phaseProgress / phaseDuration);
                        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
                    }
                } else {
                    // Not phasing. Give it a go after delaying an RNG check.
                    phaseTime += Time.deltaTime;
                    if (phaseTime > phaseDelay) {
                        phaseTime = 0;
                        if (Random.value < phaseChance) {
                            phase();
                        }
                    }
                }
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

        if (doShudder) { shudder(); }

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
				if (gunCooldown <= 0) {
					// OK, gun's ready. Set cooldown and fire.
					gunCooldown = gunCooldownTime;
					_anim.SetTrigger ("Fire");
				}
			} else if (meleeCooldown <= 0) {
                _anim.SetTrigger ("Attack");
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
		case AbstractEnemyControl.ANIM_SHOOT_START:
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
				if (vD <= _vertRange && vD >= -_vertRange && gunCooldown <= 0) {
					setState (EnemyStates.attack);
				}
			} else if (hD > -gunRangeMax && hD < 0) {
				// Within gun range. Move back left as you fire.
				facingLeft = false;
				normHD = -1;
				targetX = _player.transform.position.x - gunRangeMax;
				if (vD <= _vertRange && vD >= -_vertRange && gunCooldown <= 0) {
					setState (EnemyStates.attack);
				}
			} else {
				// Exactly at gun range. How about that?
				normHD = 0;
				targetX = this.transform.position.x;
				if (vD <= _vertRange && vD >= -_vertRange && gunCooldown <= 0) {
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
        if (clownWater == null)
        {
            setState(EnemyStates.move);
            return;
        }

		GameObject go;
		ClownWater bullet;
		if (facingLeft) {
			go = Instantiate (clownWater);
			bullet = go.GetComponent<ClownWater> ();
			bulletSpawn.position.Set (-Mathf.Abs (bulletSpawn.position.x), bulletSpawn.position.y, bulletSpawn.position.z);
			bullet.direction = Vector2.left;
		} else {
			go = Instantiate (clownWater);
			bullet = go.GetComponent<ClownWater> ();
			bulletSpawn.position.Set (Mathf.Abs (bulletSpawn.position.x), bulletSpawn.position.y, bulletSpawn.position.z);
			bullet.direction = Vector2.right;
		}

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

    public void phase () {
        phaseDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        phaseProgress = 0;
        isPhasing = true;
    }

	public override void stun (float timeInSec)
	{
		base.stun (timeInSec);
		_anim.SetTrigger ("IsStunned");
	}

    public void setShudder (float intensity = 1) {
        shudderIntensity = intensity;
        doShudder = true;
    }

    public void setPhaseChance (float chance) {
        phaseChance = chance;
    }

    public void setMoveSpeed (float speed) {
        _enemMoveSpeed = speed;
    }

    private void shudder () {
        for (var i = bodyParts.Length - 1; i >= 0; i--) {
            Vector3 pos = bodyParts[i].transform.position;
            Vector3 rot = bodyParts[i].transform.eulerAngles;
            /*
            bodyParts[i].transform.position = new Vector3(
                pos.x + Random.Range(-shudderIntensity, shudderIntensity) * .01f,
                pos.y + Random.Range(-shudderIntensity, shudderIntensity) * .01f,
                pos.z
            );
            */
            bodyParts[i].transform.eulerAngles = new Vector3(
                rot.x + Random.Range(-shudderIntensity, shudderIntensity),
                rot.y + Random.Range(-shudderIntensity, shudderIntensity),
                rot.z + Random.Range(-shudderIntensity, shudderIntensity)
            );
        }
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
