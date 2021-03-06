﻿using UnityEngine;
using System.Collections;

public class AbstractBossControl : AbstractClass
{
	public const string ANIM_SPAWN_END = "spawnEnd";

	public const string ANIM_ATTACK_START = "attackStart";
	public const string ANIM_ATTACK_UPDATE = "attackUpdate";
	public const string ANIM_ATTACK_END = "attackEnd";

	public const string ANIM_RETREAT_START = "retreatStart";
	public const string ANIM_RETREAT_UPDATE = "retreatUpdate";
	public const string ANIM_RETREAT_END = "retreatEnd";

	public const string ANIM_REAPPEAR_START = "reappearStart";
	public const string ANIM_REAPPEAR_UPDATE = "reappearUpdate";
	public const string ANIM_REAPPEAR_END = "reappearEnd";

	public const string ANIM_SPECIAL_START = "specialStart";
	public const string ANIM_SPECIAL_UPDATE = "specialEnd";
	public const string ANIM_SPECIAL_END = "specialUpdate";

    public const string ANIM_RANGED_START = "rangedStart";
    public const string ANIM_RANGED_UPDATE = "rangedEnd";
    public const string ANIM_RANGED_END = "rangedUpdate";

    public const string ANIM_DYING_START = "dyingStart";
	public const string ANIM_DYING_UPDATE = "dyingUpdate";
	public const string ANIM_DYING_END = "dyingEnd";

	public const string ANIM_STUN_START = "stunStart";
	public const string ANIM_STUN_END = "stunEnd";

	public const string ANIM_DEATH_END = "deathEnd";

	public int bossState;

	public float _bossHealth;
	public float _bossMaxHealth;
	public float _attackRange;
	public float enemDamage;
	public float _enemMoveSpeed;
	public float _vertRange;
	public bool isAlive;
	public bool isMoving;
    public bool isInvincible = false;
    public float damageToRage = 50;
    public float rageDecayPerSec = 1;
    public RageMeter rageMeter;

    protected bool _useRage = false;
    protected float _rageLevel = 0;
    protected float _rageDecayTimer = 1f;
    protected bool _enraged = false;

    protected GameObject _player;
	protected float xForce = 0;
	protected float yForce = 0;
	protected float friction = 0.005f;
    protected float meleeCooldown = 0f;

    protected float _attackCooldown = 0f;
    protected float _attackCooldownTime = 0f;
    protected float _specialCooldown = 0f;
    protected float _specialCooldownTime = 0f;

    protected Vector3 _vel;
    protected float velocityXSmoothing;
    protected float velocityYSmoothing;

	protected Transform bulletSpawn;
    protected MovementController2D _controller;
    protected AbstractDamageCollider[] _damageColliders;
	
	public bool facingLeft;

	public bool isDead {
		get {
			return state == BossAction.dead;
		}
	}

	public enum BossAction
	{
		spawn,
		/* Spawning animation. */
		move,
		/* Moves like normal ad.*/
		stand,
		/* Stands still, unmoving.*/
		attack,
		/* Attacks like normal ad.*/
		retreat,
		/* Goes through retreat or phase-switching animation.*/
		reappear,
		/* Reappears for another round.*/
		special,
		/* Uses special custom ability*/
		stun,
		/* Stunned by a powerful blow. Same as ad.*/
		dying,
		/* Last blow was made. May have final last-ditch attack.*/
		dead
		/* Finally dead.*/}
	;

	public BossAction state;

	protected Animator _anim;

	protected override void Start ()
	{
        _player = GameObject.FindGameObjectWithTag("Player");

        _anim = GetComponent<Animator> ();
        if (_anim != null) {
            BossAbstractBehaviour[] eabs = _anim.GetBehaviours<BossAbstractBehaviour>();
            for (var i = eabs.Length - 1; i >= 0; i--) {
                eabs[i].boss = this;
            }
        }

        if (rageMeter != null)
        {
            _useRage = true;
            rageMeter.setRageLevelMax(damageToRage);
        }
        
		// Set the first state.
		setBossAction (BossAction.spawn);
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

        _anim.SetFloat("Health", _bossHealth);
        _anim.SetBool("FacingLeft", facingLeft);

        if (_attackCooldown > 0) {
            _attackCooldown -= Time.deltaTime;
        }
        if (_specialCooldown > 0) {
            _specialCooldown -= Time.deltaTime;
        }

        switch (state) {
            case BossAction.stand:
                break;
		    case BossAction.move:
			    MoveToPlayer ();
			    break;
		    case BossAction.attack:
			    Attack ();
			    break;
		}

        if (_useRage && _rageLevel > 0)
        {
            // Rage meter ticks down fast. Otherwise it's a second per damage point; way too long.
            _rageLevel -= Time.deltaTime * rageDecayPerSec;
            rageMeter.setDirection(facingLeft);
            rageMeter.subtractRageLevel(Time.deltaTime * rageDecayPerSec);
            if (_enraged && _rageLevel <= 0)
            {
                _enraged = false;
            }
        }
    }

	public virtual void setBossAction (BossAction newState)
	{
		if (state == newState) {
			// Already in this state. Do nothing.
			return;
		}
		
		// Switch to the new state.
		state = newState;
		
		// Do stuff to initialize new state.
		switch (newState) {
		case BossAction.spawn:
			// Boss is moving to attack.
			_anim.SetTrigger ("Entry");
			break;
		case BossAction.move:
			// Boss is moving to attack.
			isMoving = true;
			break;
		case BossAction.stand:
			// Boss is standing still. Manually switch the state.
			isMoving = false;
			break;
		case BossAction.attack:
			isMoving = false;
			break;
		case BossAction.stun:
			break;
		case BossAction.dead:
			break;
		}
	}

	public virtual void onAnimationState (string animState)
	{
		switch (animState) {
		case AbstractEnemyControl.ANIM_SPAWN_END:
			setBossAction (BossAction.move);
			break;
		case AbstractBossControl.ANIM_ATTACK_END:
			setBossAction (BossAction.move);
			break;
		case AbstractBossControl.ANIM_SPECIAL_END:
			setBossAction (BossAction.move);
			break;
		case AbstractBossControl.ANIM_DEATH_END:
			SendMessageUpwards ("bossDead", SendMessageOptions.DontRequireReceiver);
			Destroy (gameObject);
			break;
		}
	}

	protected virtual void MoveToPlayer ()
	{
		float hD = _player.transform.position.x - this.transform.position.x;
		float vD = _player.transform.position.y - this.transform.position.y;
		
		// Y-positioning - move enemy to the player's level at all times.
		if (_player.transform.position.y - _vertRange > this.transform.position.y) {
			transform.Translate (new Vector3 (0, _enemMoveSpeed * Time.deltaTime, 0));
		} else if (_player.transform.position.y + _vertRange < this.transform.position.y) {
			transform.Translate (new Vector3 (0, -_enemMoveSpeed * Time.deltaTime, 0));
		}
		
		// X-positioning - move enemy to its closest horizontal range. If too close to the player, back away.
		if (hD > _attackRange) {
			// Zombie is to the left of player. Move right.
			facingLeft = false;
			transform.Translate (new Vector3 (_enemMoveSpeed * Time.deltaTime, 0, 0));
		} else if (hD < -_attackRange) {
			// Zombie is to the right of player. Move left.
			facingLeft = true;
			transform.Translate (new Vector3 (-_enemMoveSpeed * Time.deltaTime, 0, 0));
		} else if (hD < _attackRange - 0.1f && hD >= 0) {
			// Zombie is to the left of player, but too close. Move left.
			facingLeft = false;
			transform.Translate (new Vector3 (-_enemMoveSpeed * Time.deltaTime, 0, 0));
		} else if (hD > -_attackRange + 0.1f && hD < 0) {
			// Zombie is to the right of player, but too close. Move right.
			facingLeft = true;
			transform.Translate (new Vector3 (_enemMoveSpeed * Time.deltaTime, 0, 0));
		}
		
		if (hD <= _attackRange && hD >= -_attackRange && vD <= _vertRange && vD >= -_vertRange) {
			setBossAction (BossAction.attack);
		}
		
		_anim.SetBool ("FacingLeft", facingLeft);
	}

    protected virtual void CheckToAttack () {
        float hD = _player.transform.position.x - this.transform.position.x;
        float vD = _player.transform.position.y - this.transform.position.y;

        if (vD <= _vertRange && vD >= -_vertRange) {
            // In vertical range. Try horizontal.
            if (hD <= _attackRange && hD >= -_attackRange) {
                // In attack range. Strike!
                if (_attackCooldown <= 0) {
                    _attackCooldown = _attackCooldownTime;
                    setBossAction(BossAction.attack);
                }
            }
        }
    }

    protected virtual void Attack () { }
    protected virtual void Special () { }

    public override void damage (int damage, AbstractDamageCollider.DamageType type, int knockback)
	{		
		if (_bossHealth > 0) {
            if (_enraged)
            {
                // Only takes half damage if enraged.
                damage /= 2;
            }

            _bossHealth -= damage;
            if (_useRage)
            {
                _rageLevel += damage;
                if (!_enraged && _rageLevel >= damageToRage)
                {
                    // Boss is now enraged.
                    _enraged = true;
                }
            }

            if (_bossHealth <= 0) {
				// Enemy is dead. Set state.
				setBossAction (BossAction.dying);
				SendMessageUpwards ("bossDying", SendMessageOptions.DontRequireReceiver);
				// Don't do anything else. Make-Dead code will handle the rest.
				return;
			}
		}
	}

	public virtual void stun (float timeInSec)
	{
		StartCoroutine (doStun (timeInSec));
	}

	protected virtual IEnumerator doStun (float timeInSec)
	{
		BossAction origState = state;
		setBossAction (BossAction.stun);
		yield return new WaitForSeconds (timeInSec);
		setBossAction (origState);
	}

	
	public virtual void adsKilled ()
	{
		// GameController announced ads are dead. End retreat state.
	}
}

