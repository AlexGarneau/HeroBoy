using UnityEngine;
using System.Collections;

public class RobotClaw: AbstractEnemyControl
{
    protected const float SPECIAL_ATTACK_COUNTDOWN = 30f;

    public Collider2D lightHit;

    protected bool highGround;

    protected float specialAttackCountdownTimer = 0;

	protected override void Start ()
	{
        base.Start();
        base._enemHealth = 80f;
		base._enemMoveSpeed = 1f;
		base._enemDamage = 1;
		base._attackRange = 1.2f;
		base._vertRange = 0.2f;
		base.isAlive = true;
		base.isMoving = false;

        _controller = gameObject.GetComponent<MovementController2D> ();

        _damageColliders = gameObject.GetComponentsInChildren<EnemyDamageCollider> (true);
		if (_damageColliders != null && _damageColliders.Length > 0) {
			// Sets the damage of damage colliders. TODO: Independent damage set to different colliders, if multiple exist.
			for (int i = _damageColliders.Length - 1; i >= 0; i--) {
				_damageColliders [i].gameObject.SetActive (false);
			}
		}

        specialAttackCountdownTimer = SPECIAL_ATTACK_COUNTDOWN;

        EnemyAbstractBehaviour[] eabs = _anim.GetBehaviours<EnemyAbstractBehaviour>();
        for (var i = eabs.Length - 1; i >= 0; i--)
        {
            eabs[i].enemy = this;
        }
	}

	protected override void Update()
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

        specialAttackCountdownTimer -= Time.deltaTime;

        _anim.SetFloat ("Health", _enemHealth);
		_anim.SetBool ("FacingLeft", facingLeft);
		_anim.SetBool ("HighGround", highGround);
		HighGroundCheck ();

		base.Update ();
	}

	protected override void setState (EnemyStates newState)
	{
        Debug.Log("RobotClaw Set State: " + newState);
		switch (newState) {
		case EnemyStates.move:
			_anim.SetBool ("IsMoving", true);
			break;
		case EnemyStates.attack:
			_anim.SetBool ("IsMoving", false);
            if (specialAttackCountdownTimer > 0) {
                _anim.SetTrigger("Attack");
            } else {
                _anim.SetTrigger("Special");
                specialAttackCountdownTimer = SPECIAL_ATTACK_COUNTDOWN;
            }
			break;
		case EnemyStates.dead:
			_anim.SetBool ("IsMoving", false);
			break;
		}

		base.setState (newState);
	}

    protected override void MoveToAttack()
    {
        float hD = _player.transform.position.x - this.transform.position.x;
        float vD = _player.transform.position.y - this.transform.position.y;

        // Y-positioning - move enemy to the player's level at all times.
        if (_player.transform.position.y - _vertRange > this.transform.position.y)
        {
            normVD = 1;
        }
        else if (_player.transform.position.y + _vertRange < this.transform.position.y)
        {
            normVD = -1;
        }
        else {
            normVD = 0;
        }

        // Setup extended attack range in case player is already attacked by a melee enemy.
        float attackRange = _attackRange;
      
        if (!inMeleeRange && ((pRangeCollider.inRangeLeft && hD > 0) || (pRangeCollider.inRangeRight && hD < 0)))
        {
            attackRange = _attackRange * 2;
        }

        // X-positioning - move enemy to its closest horizontal range. If too close to the player, back away.
        //Debug.Log (hD + " - " + _attackRange);
        if (hD > attackRange)
        {
            // Zombie is to the left of player. Move right.
            facingLeft = false;
            normHD = 1;
        }
        else if (hD < -attackRange)
        {
            // Zombie is to the right of player. Move left.
            facingLeft = true;
            normHD = -1;
        }
        else if (hD < attackRange - 0.1f && hD >= 0)
        {
            // Zombie is to the left of player, but too close. Move left.
            facingLeft = false;
            normHD = -1;
        }
        else if (hD > -attackRange + 0.1f && hD < 0)
        {
            // Zombie is to the right of player, but too close. Move right.
            facingLeft = true;
            normHD = 1;
        }
        else {
            normHD = 0;
        }

        if (hD <= _attackRange && hD >= -_attackRange && vD <= _vertRange && vD >= -_vertRange)
        {
            setState(EnemyStates.attack);
        }

        float targetVelX = normHD * _enemMoveSpeed;
        float targetVelY = normVD * _enemMoveSpeed;
        _vel.x = Mathf.SmoothDamp(_vel.x, targetVelX, ref velocityXSmoothing, .1f);
        _vel.y = Mathf.SmoothDamp(_vel.y, targetVelY, ref velocityYSmoothing, .1f);
        _controller.Move(_vel * Time.deltaTime);
    }

    public override void onAnimationState (string state)
	{
		switch (state) {
		    case AbstractEnemyControl.ANIM_SPAWN_END:
                break;
		    case AbstractEnemyControl.ANIM_ATTACK_START:
			    break;
		    case AbstractEnemyControl.ANIM_ATTACK_END:
			    setState (baseState);
			    break;
		    case AbstractEnemyControl.ANIM_INJURED_END:
			    setState (baseState);
			    break;
		    case AbstractEnemyControl.ANIM_DEATH_END:
			    Destroy (gameObject);
			    break;
		}
	}

	public override void damage (int damage, AbstractDamageCollider.DamageType type, int knockback)
	{
		base.damage (damage, type, knockback);

        if (invincible)
        {
            // Can't hurt this boy.
            return;
        }

		if (facingLeft == true) {
			xForce = damage * .005f;
		} else {
			xForce = -damage * .005f;
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
        if (!invincible)
        {
		    base.stun (timeInSec);
		    _anim.SetTrigger ("IsStunned");
        }
	}

	void HighGroundCheck ()
	{
		if (transform.position.y > 0) {
			highGround = true;
		} else {
			highGround = false;
		}
	}
}
