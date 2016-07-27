using UnityEngine;
using System.Collections;

public class Bandit : AbstractEnemyControl
{
    public Collider2D lightHit;

    protected float fleeSpeed = 9f;
    protected float vanishDistance = 100f;
    protected bool highGround;

	protected override void Start ()
	{
        base.Start();
        base._enemHealth = 80f;
		base._enemMoveSpeed = 1.5f;
		base.enemDamage = 1;
		base._attackRange = 1.2f;
		base._vertRange = 0.2f;
		base.isAlive = true;
		base.isMoving = false;
        base.hasGun = false;

        _controller = gameObject.GetComponent<MovementController2D> ();

        _damageColliders = gameObject.GetComponentsInChildren<EnemyDamageCollider> (true);
		if (_damageColliders != null && _damageColliders.Length > 0) {
			// Sets the damage of damage colliders. TODO: Independent damage set to different colliders, if multiple exist.
			for (int i = _damageColliders.Length - 1; i >= 0; i--) {
				_damageColliders [i].gameObject.SetActive (false);
			}
		}

        EnemyAbstractBehaviour[] eabs = _anim.GetBehaviours<EnemyAbstractBehaviour>();
        for (var i = eabs.Length - 1; i >= 0; i--)
        {
            eabs[i].enemy = this;
        }
    }

	protected override void Update()
	{
		switch (state) {
		case EnemyStates.dead:
            AnimatorStateInfo info = _anim.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("BanditfleeL")) {
                this.transform.Translate(fleeSpeed * Time.deltaTime, 0, 0);
            } else if (info.IsName("Banditflee"))
                {
                this.transform.Translate(-fleeSpeed * Time.deltaTime, 0, 0);
            }
            if (Mathf.Abs(this.transform.position.x - _player.transform.position.x) > vanishDistance)
            {
                // Ran far enough. Destroy it.
                Destroy(gameObject);
            }
            break;
		}

		_anim.SetBool ("HighGround", highGround);
		HighGroundCheck ();

		base.Update ();
	}

	protected override void setState (EnemyStates newState)
	{
        if (state == EnemyStates.dead)
        {
            // What? I'm fleeing for my life! Shut up!
            return;
        }

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
            GetComponent<BoxCollider2D>().enabled = false;
            break;
		}

		base.setState (newState);
	}
    
    public override void onAnimationState (string animState)
	{
		switch (animState) {
		    case AbstractEnemyControl.ANIM_ATTACK_END:
			    setState (baseState);
			    break;
		    case AbstractEnemyControl.ANIM_INJURED_END:
			    setState (baseState);
			    break;
            case AbstractEnemyControl.ANIM_DEATH_START:
                setState(EnemyStates.dead);
                SendMessageUpwards ("enemyDied", this, SendMessageOptions.DontRequireReceiver);
                break;
            case AbstractEnemyControl.ANIM_DEATH_END:
			    break;
		}

        base.onAnimationState(animState);
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

	void HighGroundCheck ()
	{
		if (transform.position.y > 0) {
			highGround = true;
		} else {
			highGround = false;
		}
	}
}
