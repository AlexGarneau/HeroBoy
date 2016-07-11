using UnityEngine;
using System.Collections;

public class BossKnight: AbstractBossControl
{
    public Collider2D lightHit;
    protected bool enraged = false;
    protected float rageMeter = 0f;
    protected float rageMax = 50;

	protected override void Start ()
	{
        base.Start();

        base._bossMaxHealth = base._bossHealth = 3f;
        _anim.SetFloat("Health", _bossHealth);

        base._enemMoveSpeed = 1f;
		base._enemDamage = 1;
		base._attackRange = 1.2f;
		base._vertRange = 0.2f;
		base.isAlive = true;
		base.isMoving = false;
        
        _controller = gameObject.GetComponent<MovementController2D> ();
	}

	protected override void Update()
	{
        base.Update ();

        // Update animator values.
        _anim.SetFloat ("Health", _bossHealth);
		_anim.SetBool ("FacingLeft", facingLeft);

        // Wear out the rage meter as time goes by.
        rageMeter -= Time.deltaTime;
        if (enraged && rageMeter < rageMax) {
            enraged = false;
        }

        // Knight must stay in the level boundaries.
        this.transform.position = LevelBoundary.adjustPositionToBoundary(this.transform.position);
	}

	public override void setBossAction (BossAction newState)
	{
		switch (newState) {
		case BossAction.move:
			_anim.SetBool ("IsMoving", true);
			break;
		case BossAction.attack:
			_anim.SetBool ("IsMoving", false);
            
            // Jump when enraged. Normal attack if calm.
			_anim.SetTrigger (enraged ? "JumpAttack" : "Attack");
			break;
		case BossAction.dead:
			_anim.SetBool ("IsMoving", false);
			break;
		}

		base.setBossAction (newState);
	}
    
    public override void onAnimationState (string state)
	{
        Debug.Log("BossKnight Animation State: " + state);
		switch (state) {
		    case AbstractBossControl.ANIM_SPAWN_END:
                setBossAction(BossAction.move);
                break;
		    case AbstractBossControl.ANIM_ATTACK_START:
			    break;
		    case AbstractBossControl.ANIM_ATTACK_END:
			    setBossAction (BossAction.move);
			    break;
		    case AbstractBossControl.ANIM_STUN_END:
			    setBossAction (BossAction.move);
			    break;
            case AbstractBossControl.ANIM_DYING_START:
                setBossAction(BossAction.dead);
                BoxCollider2D[] col = GetComponentsInChildren<BoxCollider2D>();
                for (var i = col.Length - 1; i >= 0; i--) {
                    col[i].enabled = false;
                }
                break;
            case AbstractBossControl.ANIM_DYING_END:
                // Knight is dead. But is it the end?
                _anim.SetBool("IsMoving", false);
                SendMessageUpwards("knightDead", null, SendMessageOptions.DontRequireReceiver);
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

        rageMeter += damage;
        if (!enraged && rageMeter >= rageMax) {
            enraged = true;
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
