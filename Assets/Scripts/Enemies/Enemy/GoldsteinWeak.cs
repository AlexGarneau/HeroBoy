using UnityEngine;
using System.Collections;

public class GoldsteinWeak : AbstractEnemyControl
{
    public Collider2D lightHit;

    protected float fleeSpeed = 9f;
    protected float vanishDistance = 100f;
    protected bool highGround;

	protected override void Start ()
	{
        base.Start();
        base._enemHealth = 1f;
		base._enemMoveSpeed = 1f;
		base.enemDamage = 1;
		base._attackRange = 1.7f;
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

	protected override void setState (EnemyStates newState)
	{
        if (state == EnemyStates.dead)
        {
            return;
        }

		switch (newState) {
		case EnemyStates.move:
			_anim.SetBool ("IsMoving", true);
			break;
		case EnemyStates.attack:
            damage(1, AbstractDamageCollider.DamageType.light, 0);
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

	public override void damage (int damage, AbstractDamageCollider.DamageType type = AbstractDamageCollider.DamageType.light, int knockback = 0)
	{
		base.damage (damage, type, knockback);

        if (invincible)
        {
            // Can't hurt this boy.
            return;
        }

        // Dies from one hit. Deal with it.
        _anim.SetTrigger("Death");
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
