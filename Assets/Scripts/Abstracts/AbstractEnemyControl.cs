using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MovementController2D))]
public class AbstractEnemyControl : AbstractClass
{
	public const string ANIM_SPAWN_END = "spawnEnd";
	public const string ANIM_ATTACK_START = "attackStart";
	public const string ANIM_ATTACK_END = "attackEnd";
    public const string ANIM_DEATH_START = "deathStart";
    public const string ANIM_DEATH_END = "deathEnd";
	public const string ANIM_SHOOT_START = "shootStart";
	public const string ANIM_BOMB_THROW = "bombThrow";
	public const string ANIM_INJURED_END = "injuredEnd";
    public const string ANIM_STUN_START = "stunStart";
    public const string ANIM_STUN_END = "stunEnd";

    public bool facingLeft = false;
    public bool invincible = false;

    public bool isAlive;
	public bool isMoving;
    public bool inMeleeRange;
    public bool hasAttack = true;
    public bool hasGun = true;

    protected float _enemHealth;
    protected float _attackRange = 2f;
    protected float _attackCooldown = 0f;
    protected float _attackCooldownTime = 1f;
    protected float _gunRange = 5f;
    protected float _gunCooldown = 0f;
    protected float _gunCooldownTime = 4f;
    protected float _paceBackRange = 4f;
    protected float _paceForthRange = 2f;
    protected float _paceTimer = 0f;
    protected float _paceTimerMax = 5f;
    protected float _enemMoveSpeed;
	protected float _vertRange;
    protected Vector3 _paceTarget;

    protected int _enemDamage;
	public int enemDamage {
		get { 
			return _enemDamage;
		}
		set {
			_enemDamage = value;
            if (_damageColliders == null)
            {
                _damageColliders = gameObject.GetComponentsInChildren<EnemyDamageCollider>(true);
            }
			if (_damageColliders != null && _damageColliders.Length > 0) {
				// Sets the damage of damage colliders. TODO: Independent damage set to different colliders, if multple exist.
				for (int i = _damageColliders.Length - 1; i >= 0; i--) {
					_damageColliders [i].damage = value;
				}
			}
		}
	}

	protected GameObject _player;
    protected Animator _anim;
    protected PlayerControl _playerControl;
	protected float xForce = 0;
	protected float yForce = 0;
	protected float friction = 0.005f;
    protected float meleeCooldown = 0f;

    protected EnemyRangeCollider pRangeCollider;

    protected Rigidbody body;
	protected Transform bulletSpawn;

	protected EnemyDamageCollider[] _damageColliders;
	protected MovementController2D _controller;
	protected Vector3 _vel = Vector3.zero;
	protected float velocityXSmoothing;
	protected float velocityYSmoothing;
	protected float normVD = 0;
	protected float normHD = 0;
	
    // To prevent pile-ups
    protected bool lineUpInRange;
    protected EnemyRangeCollider withinRangeCollider;

	public enum EnemyStates
	{
        idle, // Enemy is idle. Doesn't do anything but stand and look pretty.
		spawn, // Enemy goes through spawn animation.
        stand, // Enemy holds position and attacks if player gets close. Good for snipers or guards.
        paceBack, // Enemy keeps his distance, waiting for an opening. Will attack if close enough or in range.
        paceForth, // Enemy moves forward a bit to keep it interesting. Will attack if close enough or in range.
		move, // Enemy moves and homes in on player.
		attack, // Enemy currently attacking. Usually doesn't move during this.
        shoot, // Another variation of attack, though usually at a greater range.
		stun, // Enemy is stunned. Use your imagination.
		dead // Enemy is dead. Big surprise.
    }
	;

	public EnemyStates state;
    protected EnemyStates baseState;

	public bool isDead {
		get {
			return state == EnemyStates.dead;
		}
	}

	protected virtual void Start ()
	{
        _anim = gameObject.GetComponent<Animator>();
        GameObject[] list = GameObject.FindGameObjectsWithTag("Player");
        for (var i = list.Length - 1; i >= 0; i--)
        {
            PlayerControl player = list[i].GetComponent<PlayerControl>();
            if (player != null)
            {
                _player = player.gameObject;
                _playerControl = player;
                pRangeCollider = player.GetComponentInChildren<EnemyRangeCollider>();
                break;
            }
        }

        baseState = EnemyStates.move;

        // Set the first state.
        setState(EnemyStates.spawn);
    }

	protected virtual void Update ()
	{
		base.Update ();

		// Move enemy based on force.
		transform.Translate (new Vector3 (xForce, yForce, 0));
		if (xForce > friction) {
			xForce -= friction;
		} else if (xForce < -friction) {
			xForce += friction;
		} else if (xForce != 0) {
			xForce = 0;
		}
		if (yForce > friction) {
			yForce -= friction;
		} else if (yForce < -friction) {
			yForce += friction;
		} else if (yForce != 0) {
			yForce = 0;
		}

		switch (state) {
		case EnemyStates.spawn:
			break;
		case EnemyStates.move:
			MoveToPlayer ();
            CheckToAttack();
            break;
        case EnemyStates.stand:
            CheckToAttack();
            break;
        case EnemyStates.attack:
            // Enemy animates, but otherwise doesn't do anything. The animation state will call for change.
			break;
        case EnemyStates.shoot:
            // Enemy animates, but otherwise doesn't do anything. The animation state will call for change.
            break;
        case EnemyStates.paceBack:
        case EnemyStates.paceForth:
            MoveToTarget(_paceTarget);
            CheckToAttack();
            break;
        case EnemyStates.stun:
			break;
		case EnemyStates.dead:
			break;
		}

        // Tick down the cooldowns.
        if (_attackCooldown > 0) {
            _attackCooldown -= Time.deltaTime;
            if (_attackCooldown < 0) {
                _attackCooldown = 0;
            }
        }
        if (_gunCooldown > 0) {
            _gunCooldown -= Time.deltaTime;
            if (_gunCooldown < 0) {
                _gunCooldown = 0;
            }
        }

        if (_paceTimer > 0) {
            _paceTimer -= Time.deltaTime;
            if (_paceTimer < 0) {
                if (state == EnemyStates.paceBack) {
                    setState(EnemyStates.paceForth);
                } else if (state == EnemyStates.paceForth) {
                    setState(EnemyStates.paceBack);
                }
            }
        }

        if (_anim != null) {
            _anim.SetFloat("Health", _enemHealth);
            _anim.SetBool("FacingLeft", facingLeft);
            _anim.SetInteger("PlayerHealth", _playerControl.playerHealth);
        }

        // Confine the enemy to the level boundaries.
        if (state != EnemyStates.dead) {
            this.transform.position = LevelBoundary.adjustPositionToBoundary(this.transform.position);
        }
	}

    public void setEnemyState(EnemyStates newState)
    {
        if (_anim == null)
        {
            _anim = gameObject.GetComponent<Animator>();
        }
        setState(newState);
    }

    public void setBaseState(EnemyStates bs) {
        baseState = bs;
    }

    protected virtual void setState (EnemyStates newState)
	{
		if (state == newState) {
			// Already in this state. Do nothing.
			return;
		}

        // Switch to the new state. Store the old one.
        if (state == EnemyStates.move || state == EnemyStates.stand || state == EnemyStates.paceBack || state == EnemyStates.paceForth)
        {
            baseState = state;
        }
		state = newState;

        float angle;

		// Do stuff to initialize new state.
		switch (newState) {
		    case EnemyStates.move:
			    isMoving = true;
                _anim.SetBool("IsMoving", true);
                break;
		    case EnemyStates.attack:
			    isMoving = false;
                _anim.SetBool("IsMoving", false);
                break;
		    case EnemyStates.stun:
                isMoving = false;
                _anim.SetBool("IsMoving", false);
                break;
            case EnemyStates.paceBack:
                isMoving = true;
                _anim.SetBool("IsMoving", true);

                // Gets a random position.
                if (this.transform.position.x > _player.transform.position.x) {
                    // On the right side of player. Face right.
                    facingLeft = true;
                    angle = (Random.value * 180f) - 90f;
                } else {
                    // On the left side of player. Face right.
                    facingLeft = false;
                    angle = ((Random.value * 90f) + 90f) * (Random.value > .5 ? -1f : 1f);
                }
                _paceTarget = new Vector3(_player.transform.position.x + (_paceBackRange * Mathf.Cos(angle)), _player.transform.position.y + (_paceBackRange * Mathf.Sin(angle)) * .5f);
                _paceTimer = _paceTimerMax;
                break;
            case EnemyStates.paceForth:
                isMoving = true;
                _anim.SetBool("IsMoving", true);

                // Gets a random position.
                if (this.transform.position.x > _player.transform.position.x) {
                    // On the right side of player.
                    facingLeft = true;
                    angle = (Random.value * 180f) - 90f;
                } else {
                    // On the left side of player.
                    facingLeft = false;
                    angle = ((Random.value * 90f) + 90f) * (Random.value > .5 ? -1f : 1f);
                }
                _paceTarget = new Vector3(_player.transform.position.x + (_paceForthRange * Mathf.Cos(angle)), _player.transform.position.y + (_paceForthRange * Mathf.Sin(angle)) * .5f);
                _paceTimer = _paceTimerMax;
                break;
            case EnemyStates.dead:
                _anim.SetBool("IsMoving", false);
                break;
		}
	}

    public virtual void setEnemyDamage (int damage)
    {
        enemDamage = damage;
    }

	public virtual void onAnimationState (string animState)
	{
        switch (animState) {
            case AbstractEnemyControl.ANIM_SPAWN_END:
                setState(baseState);
                break;
            /*
            case AbstractEnemyControl.ANIM_ATTACK_END:
                if (state != EnemyStates.stun)
                {
                    setState(baseState);
                }
                break;
            case AbstractEnemyControl.ANIM_INJURED_END:
                if (_enemHealth > 0 && state != EnemyStates.stun)
                {
                    setState(baseState);
                }
                break;
            case AbstractEnemyControl.ANIM_STUN_END:
                _anim.SetBool("IsStunned", false);
                setState(baseState);
                break;
            */
        }
	}

    // Move toward the player and attack.
    protected virtual void MoveToPlayer ()
	{
		float hD = _player.transform.position.x - this.transform.position.x;
		float vD = _player.transform.position.y - this.transform.position.y;
		
		// Y-positioning - move enemy to the player's level at all times.
		if (_player.transform.position.y - _vertRange > this.transform.position.y) {
			normVD = 1;
		} else if (_player.transform.position.y + _vertRange < this.transform.position.y) {
			normVD = -1;
		} else {
			normVD = 0;
		}

		// X-positioning - move enemy to its closest horizontal range. If too close to the player, back away.
		if (hD > _attackRange) {
            // Enemy is to the left of player. Move right.
            facingLeft = false;
			normHD = 1;
		} else if (hD < -_attackRange) {
            // Enemy is to the right of player. Move left.
            facingLeft = true;
			normHD = -1;
		} else if (hD < _attackRange - 0.1f && hD >= 0) {
            // Enemy is to the left of player, but too close. Move left.
            facingLeft = false;
			normHD = -1;
		} else if (hD > -_attackRange + 0.1f && hD < 0) {
			// Enemy is to the right of player, but too close. Move right.
			facingLeft = true;
			normHD = 1;
		} else {
			normHD = 0;
		}

		float targetVelX = normHD * _enemMoveSpeed;
		float targetVelY = normVD * _enemMoveSpeed;
		_vel.x = Mathf.SmoothDamp (_vel.x, targetVelX, ref velocityXSmoothing, .1f);
		_vel.y = Mathf.SmoothDamp (_vel.y, targetVelY, ref velocityYSmoothing, .1f);
		_controller.Move (_vel * Time.deltaTime);
	}

    // Move toward the player and attack.
    protected virtual void MoveToTarget (Vector3 target) {
        float hD = target.x - this.transform.position.x;
        float vD = target.y - this.transform.position.y;
        float pHD = _player.transform.position.x - this.transform.position.x;

        // Keep the enemy facing the player. Common sense, after all.
        if (pHD > 0) {
            // Enemy is to the left of player. Face right.
            facingLeft = false;
        } else {
            // Enemy is to the right of player. Face left.
            facingLeft = true;
        }

        float angle = Mathf.Atan2(vD, hD);

        float distance = Mathf.Abs(Vector3.Distance(target, this.transform.position));
        if (distance <= _enemMoveSpeed * Time.deltaTime) {
            // Close enough to the target. This should plop them right on top.
            _vel.x = Mathf.Cos(angle) * distance;
            _vel.y = Mathf.Sin(angle) * distance;

            // Hit the target. Switch pace states if they're used.
            if (state == EnemyStates.paceBack) {
                setState(EnemyStates.paceForth);
            } else if (state == EnemyStates.paceForth) {
                setState(EnemyStates.paceBack);
            }
        } else {
            // Move towards the target.
            _vel.x = Mathf.Cos(angle) * _enemMoveSpeed;
            _vel.y = Mathf.Sin(angle) * _enemMoveSpeed;
        }

        _controller.Move(_vel * 0.75f * Time.deltaTime);
    }

    protected virtual void CheckToAttack () {
        float hD = _player.transform.position.x - this.transform.position.x;
        float vD = _player.transform.position.y - this.transform.position.y;

        if (vD <= _vertRange && vD >= -_vertRange) {
            // In vertical range. Try horizontal.
            if (hasAttack && hD <= _attackRange && hD >= -_attackRange) {
                // In attack range. Strike!
                if (_attackCooldown <= 0) {
                    _attackCooldown = _attackCooldownTime;
                    setState(EnemyStates.attack);
                }
            } else if (hasGun && hD <= _gunRange && hD >= -_gunRange) {
                // In shooting range. Shoot!
                if (_gunCooldown <= 0) {
                    _gunCooldown = _gunCooldownTime;
                    setState(EnemyStates.shoot);
                }
            }
        }
    }

    // Used during attack animation to apply special effects (like stepping in, jumping, etc).
    protected virtual void Attack () { }

    // Used during shoot animation to spawn bullets.
    protected virtual void Shoot () { }

    public override void damage (int damage, AbstractDamageCollider.DamageType type, int knockback)
	{		
        if (!invincible && _enemHealth > 0) {
			_enemHealth -= damage;
			if (_enemHealth <= 0) {
				// Enemy is dead. Set state.
				setState (EnemyStates.dead);
				SendMessageUpwards ("enemyDied", this, SendMessageOptions.DontRequireReceiver);
				// Don't do anything else. Make-Dead code will handle the rest.
				return;
			}
		}
	}

	public virtual void stun (float timeInSec)
	{
        if (!invincible)
        {
            StartCoroutine(doStun(timeInSec));
        }
	}

	protected virtual IEnumerator doStun (float timeInSec)
	{
		setState (EnemyStates.stun);
		yield return new WaitForSeconds (timeInSec);
		setState (EnemyStates.move);
	}
}

