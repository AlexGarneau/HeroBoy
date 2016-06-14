using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MovementController2D))]
public class AbstractEnemyControl : AbstractClass
{
	public const string ANIM_SPAWN_END = "spawnEnd";
	public const string ANIM_ATTACK_START = "attackStart";
	public const string ANIM_ATTACK_END = "attackEnd";
	public const string ANIM_DEATH_END = "deathEnd";
	public const string ANIM_SHOOT_START = "shootStart";
	public const string ANIM_BOMB_THROW = "bombThrow";
	public const string ANIM_INJURED_END = "injuredEnd";
    public const string ANIM_STUN_START = "stunStart";
    public const string ANIM_STUN_END = "stunEnd";

    public float _enemHealth;
	public float _attackRange;
	public float _enemMoveSpeed;
	public float _vertRange;
	public bool isAlive;
	public bool isMoving;
    public bool inMeleeRange;

    private int __enemDamage;

	public int _enemDamage {
		get { 
			return __enemDamage;
		}
		set {
			__enemDamage = value;
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
	protected Vector3 _vel;
	protected float velocityXSmoothing;
	protected float velocityYSmoothing;
	protected float normVD = 0;
	protected float normHD = 0;
	
	public bool facingLeft;
    public bool invincible = false;

    // To prevent pile-ups
    protected bool lineUpInRange;
    protected EnemyRangeCollider withinRangeCollider;

	public enum EnemyStates
	{
        idle, // Enemy is idle. Doesn't do anything but stand and look pretty.
		spawn, // Enemy goes through spawn animation.
        stand, // Enemy holds position and attacks if player gets close. Good for snipers or guards.
		move, // Enemy moves and homes in on player.
		attack, // Enemy currently attacking. Usually doesn't move during this.
		stun, // Enemy is stunned. Use your imagination.
		dead} // Enemy is dead. Big surprise.
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
			MoveToAttack ();
			break;
        case EnemyStates.stand:
            StandToAttack();
            break;
        case EnemyStates.attack:
			Attack ();
			break;
		case EnemyStates.stun:
                
			break;
		case EnemyStates.dead:
			break;
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

    protected virtual void setState (EnemyStates newState)
	{
		if (state == newState) {
			// Already in this state. Do nothing.
			return;
		}

        // Switch to the new state. Store the old one.
        if (state == EnemyStates.move || state == EnemyStates.stand)
        {
            baseState = state;
        }
		state = newState;
		
		// Do stuff to initialize new state.
		switch (newState) {
		case EnemyStates.move:
			isMoving = true;
			break;
		case EnemyStates.attack:
			isMoving = false;
			break;
		case EnemyStates.stun:
            isMoving = false;
            break;
		case EnemyStates.dead:
            break;
		}
	}

    public virtual void setEnemyDamage (int damage)
    {
        _enemDamage = damage;
    }

	public virtual void onAnimationState (string animState)
	{
		switch (animState) {
		case AbstractEnemyControl.ANIM_SPAWN_END:
			setState (EnemyStates.move);
			break;
		case AbstractEnemyControl.ANIM_ATTACK_START:

			break;
		case AbstractEnemyControl.ANIM_ATTACK_END:
			setState (baseState); // Can attack from move or stand. Set previous state.
			break;
		case AbstractEnemyControl.ANIM_INJURED_END:
			if (_enemHealth > 0 && state != EnemyStates.stun) {
				setState (EnemyStates.move);
			}
			break;
		}
	}

    // Attack if the player gets close, but hold position.
    protected virtual void StandToAttack()
    {
        float hD = _player.transform.position.x - this.transform.position.x;
        float vD = _player.transform.position.y - this.transform.position.y;
        if (hD <= _attackRange && hD >= -_attackRange && vD <= _vertRange && vD >= -_vertRange)
        {
            setState(EnemyStates.attack);
        }
    }

    // Move toward the player and attack.
    protected virtual void MoveToAttack ()
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
		//Debug.Log (hD + " - " + _attackRange);
		if (hD > _attackRange) {
			// Zombie is to the left of player. Move right.
			facingLeft = false;
			normHD = 1;
		} else if (hD < -_attackRange) {
			// Zombie is to the right of player. Move left.
			facingLeft = true;
			normHD = -1;
		} else if (hD < _attackRange - 0.1f && hD >= 0) {
			// Zombie is to the left of player, but too close. Move left.
			facingLeft = false;
			normHD = -1;
		} else if (hD > -_attackRange + 0.1f && hD < 0) {
			// Zombie is to the right of player, but too close. Move right.
			facingLeft = true;
			normHD = 1;
		} else {
			normHD = 0;
		}

		if (hD <= _attackRange && hD >= -_attackRange && vD <= _vertRange && vD >= -_vertRange) {
			setState (EnemyStates.attack);
		}

		float targetVelX = normHD * _enemMoveSpeed;
		float targetVelY = normVD * _enemMoveSpeed;
		_vel.x = Mathf.SmoothDamp (_vel.x, targetVelX, ref velocityXSmoothing, .1f);
		_vel.y = Mathf.SmoothDamp (_vel.y, targetVelY, ref velocityYSmoothing, .1f);
		_controller.Move (_vel * Time.deltaTime);
	}

	protected virtual void Attack ()
	{
		// Enemy leaves attack state only when finished attacking (or hit, killed, etc).
		/*
		float hD = _player.transform.position.x - this.transform.position.x;
		float vD = _player.transform.position.y - this.transform.position.y;
		if ((hD > _attackRange || hD < -_attackRange) || (vD > _vertRange || vD < -_vertRange)) {
			setState (EnemyStates.move);
		}*/

	}

	protected virtual void Shoot ()
	{

	}

	public override void damage (int damage, AbstractDamageCollider.DamageType type, int knockback)
	{		
        if (!invincible && _enemHealth > 0) {
			_enemHealth -= damage;
			if (_enemHealth <= 0) {
				// Enemy is dead. Set state.
				setState (EnemyStates.dead);
				SendMessageUpwards ("enemyDied", SendMessageOptions.DontRequireReceiver);
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

