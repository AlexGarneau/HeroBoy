using UnityEngine;
using System.Collections;

public class BossSpider: AbstractBossControl
{
    public Collider2D lightHit;

    protected static float ATTACK_COOLDOWN = 4f;
    protected static float SPRAY_COOLDOWN = 10f;

    protected float attackCooldown = 0f;
    protected float sprayCooldown = 0f;

	protected override void Start ()
	{
        base.Start();

        base._bossMaxHealth = base._bossHealth = 300f;
        _anim.SetFloat("Health", _bossHealth);

        base._enemMoveSpeed = 1f;
		base._enemDamage = 1;
		base._attackRange = 1.2f;
		base._vertRange = 0.2f;
		base.isAlive = true;
		base.isMoving = false;
        
        _controller = gameObject.GetComponent<MovementController2D> ();
        attackCooldown = ATTACK_COOLDOWN;
        sprayCooldown = SPRAY_COOLDOWN;
    }

	protected override void Update()
	{
        _anim.SetFloat ("Health", _bossHealth);
		_anim.SetBool ("FacingLeft", facingLeft);

        if (state == BossAction.move) {
            // Spider is active. Make it attack.
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0) {
                attackCooldown = ATTACK_COOLDOWN;
                setBossAction(BossAction.attack);
            }

            // Or spray.
            sprayCooldown -= Time.deltaTime;
            if (sprayCooldown <= 0) {
                sprayCooldown = SPRAY_COOLDOWN;
                setBossAction(BossAction.special);
            }
        }

		base.Update ();
	}

	public override void setBossAction (BossAction newState)
	{
		switch (newState) {
		    case BossAction.attack:            
			    _anim.SetTrigger ("Attack");
			    break;
            case BossAction.special:
                _anim.SetTrigger("Spray");
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

        // Spider just takes damage from one source. Make it show.
	    _anim.SetTrigger ("IsHit");
	}
}
