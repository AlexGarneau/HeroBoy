using UnityEngine;
using System.Collections;

public class BossBruno: AbstractBossControl
{
    public Collider2D lightHit;

	protected override void Start ()
	{
        base.Start();
        base._bossMaxHealth = base._bossHealth = 500f;
		base._enemMoveSpeed = 1f;
		base._enemDamage = 1;
		base._attackRange = 1.2f;
		base._vertRange = 0.2f;
		base.isAlive = true;
		base.isMoving = false;
        
        _controller = gameObject.GetComponent<MovementController2D> ();

        _damageColliders = gameObject.GetComponentsInChildren<AbstractDamageCollider> (true);
		if (_damageColliders != null && _damageColliders.Length > 0) {
			// Sets the damage of damage colliders. TODO: Independent damage set to different colliders, if multiple exist.
			for (int i = _damageColliders.Length - 1; i >= 0; i--) {
				_damageColliders [i].gameObject.SetActive (false);
			}
		}
	}

	protected override void Update()
	{
        _anim.SetFloat ("Health", _bossHealth);
		_anim.SetBool ("FacingLeft", facingLeft);

		base.Update ();
	}

	public override void setBossAction (BossAction newState)
	{
		switch (newState) {
		case BossAction.move:
			_anim.SetBool ("IsMoving", true);
			break;
		case BossAction.attack:
			_anim.SetBool ("IsMoving", false);
            
            // Randomize between a punch and kick.
			_anim.SetTrigger ((Random.value > 0.5f) ? "Attack" : "Kick");
			break;
		case BossAction.dying:
			_anim.SetBool ("IsMoving", false);

            // No death animation; gets cutscene interrupted.
            SendMessageUpwards("bossDead", null, SendMessageOptions.DontRequireReceiver);
			break;
		}

		base.setBossAction (newState);
	}
    
    public override void onAnimationState (string state)
	{
		switch (state) {
		    case AbstractEnemyControl.ANIM_SPAWN_END:
                setBossAction(BossAction.move);
                break;
		    case AbstractEnemyControl.ANIM_ATTACK_START:
			    break;
		    case AbstractEnemyControl.ANIM_ATTACK_END:
			    setBossAction (BossAction.move);
			    break;
		    case AbstractEnemyControl.ANIM_INJURED_END:
			    setBossAction (BossAction.move);
			    break;
		    case AbstractEnemyControl.ANIM_DEATH_END:
			    Destroy (gameObject);
			    break;
		}
	}

	public override void damage (int damage, AbstractDamageCollider.DamageType type, int knockback)
	{
		base.damage (damage, type, knockback);

        if (isInvincible)
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
            //stun(0.5f);
			break;
		}
	}
}
