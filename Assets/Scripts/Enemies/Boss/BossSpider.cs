using UnityEngine;
using System.Collections;

public class BossSpider: AbstractBossControl
{
    public Collider2D lightHit;

    public float spawnCeilingY = 0f;
    public float spawnGroundY = 0f;
    public float squishHeight = 0f;
    public float normalHeight = 0f;

    protected static float ATTACK_COOLDOWN = 4f;
    protected static float SPRAY_COOLDOWN = 10f;

    protected float attackCooldown = 0f;
    protected float sprayCooldown = 0f;

    public GameObject acidBullet;
    public GameObject webBullet;

    // Manual Spawn
    protected int msState = -1;
    protected const int MS_STATE_IDLE = -1;
    protected const int MS_STATE_FALL = 0;
    protected const int MS_STATE_SQUISH = 1;
    protected const int MS_STATE_RECOVER = 2;
    protected const int MS_STATE_START = 3;

    protected float ms_timer = 0f;
    protected float ms_fall_time = 0.6f;
    protected float ms_squish_time = 0.2f;

    protected Transform webSpawn;

    protected override void Start ()
	{
        base.Start();

        base._bossMaxHealth = base._bossHealth = 300f;
        _anim.SetFloat("Health", _bossHealth);

        base._enemMoveSpeed = 1f;
		base.enemDamage = 1;
		base._attackRange = 1.2f;
		base._vertRange = 0.2f;
		base.isAlive = true;
		base.isMoving = false;

        bulletSpawn = transform.Find("AcidSpawn");
        webSpawn = transform.Find("WebSpawn");

        _controller = gameObject.GetComponent<MovementController2D> ();
        attackCooldown = ATTACK_COOLDOWN;
        sprayCooldown = SPRAY_COOLDOWN;

        msState = MS_STATE_IDLE;
        transform.position = new Vector3(transform.position.x, spawnCeilingY, transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x, normalHeight, transform.localScale.z);
    }

	protected override void Update()
	{
        _anim.SetFloat ("Health", _bossHealth);
		_anim.SetBool ("FacingLeft", facingLeft);

        if (state == BossAction.stand) {
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
        } else if (state == BossAction.reappear) {
            switch (msState) {
                case MS_STATE_FALL:
                    ms_timer += Time.deltaTime;
                    if (ms_timer >= ms_fall_time) {
                        transform.position = new Vector3(
                            transform.position.x,
                            spawnGroundY,
                            transform.position.z
                        );
                        ms_timer = 0;
                        msState = MS_STATE_SQUISH;
                    } else {
                        transform.position = new Vector3(
                            transform.position.x,
                            spawnCeilingY + ((spawnGroundY - spawnCeilingY) * (ms_timer / ms_fall_time)),
                            transform.position.z
                        );
                    }
                    break;
                case MS_STATE_SQUISH:
                    ms_timer += Time.deltaTime;
                    if (ms_timer >= ms_squish_time) {
                        transform.localScale = new Vector3(
                            transform.localScale.x,
                            squishHeight,
                            transform.localScale.z
                        );
                        ms_timer = 0;
                        msState = MS_STATE_RECOVER;
                    } else {
                        transform.localScale = new Vector3(
                            transform.localScale.x,
                            normalHeight + ((squishHeight - normalHeight) * (ms_timer / ms_squish_time)),
                            transform.localScale.z
                        );
                    }
                    break;
                case MS_STATE_RECOVER:
                    ms_timer += Time.deltaTime;
                    if (ms_timer >= ms_squish_time) {
                        // Manual spawn complete.
                        transform.localScale = new Vector3(
                            transform.localScale.x,
                            normalHeight,
                            transform.localScale.z
                        );
                        ms_timer = 0;
                        msState = MS_STATE_START;
                        setBossAction(BossAction.stand);
                    } else {
                        transform.localScale = new Vector3(
                            transform.localScale.x,
                            squishHeight + ((normalHeight - squishHeight) * (ms_timer / ms_squish_time)),
                            transform.localScale.z
                        );
                    }
                    break;
            }
        }

		base.Update ();
	}

    protected override void MoveToPlayer () {
        // Spider doesn't move. This doesn't do squat.
    }

    protected override void CheckToAttack()
    {
        // I'll check to attack myself. Don't you worry about me.
    }

    public override void setBossAction (BossAction newState)
	{
		switch (newState) {
		    case BossAction.attack:            
			    _anim.SetTrigger ("Attack");
                StartCoroutine(rapidFireWeb());
                break;
            case BossAction.special:
                _anim.SetTrigger("Spray");
                StartCoroutine(rapidFireAcid());
                break;
		    case BossAction.dying:
			    _anim.SetBool ("IsMoving", false);

                // No death animation; gets cutscene interrupted.
                SendMessageUpwards("bossDead", null, SendMessageOptions.DontRequireReceiver);
			    break;
            case BossAction.reappear:
                transform.position = new Vector3(transform.position.x, spawnCeilingY, transform.position.z);
                transform.localScale = new Vector3(transform.localScale.x, normalHeight, transform.localScale.z);
                msState = MS_STATE_FALL;
                break;
		}

		base.setBossAction (newState);
	}

    protected void fireAcid()
    {
        GameObject go;
        AcidBullet bullet;

        go = Instantiate(acidBullet);
        bullet = go.GetComponent<AcidBullet>();
        bullet.direction = Vector2.right;
        bullet.transform.localScale = new Vector3(-1, 1, 1);

        // Stick the bullet in the spawner.
        go.transform.position = bulletSpawn.position;

        // Put the bullet on the stage.
        go.transform.parent = transform.parent;
    }
    protected IEnumerator rapidFireAcid () {
        for (int i = 15; i >= 0; i--) {
            fireAcid();
            yield return new WaitForSeconds(.15f);
        }
    }

    protected void fireWeb () {
        GameObject go;
        WebBullet bullet;

        go = Instantiate(webBullet);
        bullet = go.GetComponent<WebBullet>();
        bullet.direction = Vector2.right;
        bullet.transform.localScale = new Vector3(-1, 1, 1);

        // Stick the bullet in the spawner.
        bullet.setSpawnAndTarget(webSpawn.position, _player.transform.position);

        // Put the bullet on the stage.
        bullet.transform.parent = transform.parent;
    }
    protected IEnumerator rapidFireWeb () {
        for (int i = 3; i >= 0; i--) {
            fireWeb();
            yield return new WaitForSeconds(0.3f);
        }
    }

    /* public void ThrowBomb()
     {
         // Create a bomb and make it fly.
         GameObject go = Instantiate(pirateBomb);
         PirateBomb bomb = go.GetComponent<PirateBomb>();

         // Position the spawner and the direction.
         if (facingLeft)
         {
             bombSpawn.position.Set(-Mathf.Abs(bombSpawn.position.x), bombSpawn.position.y, bombSpawn.position.z);
             bomb.direction = Vector2.left;
         }
         else {
             bombSpawn.position.Set(Mathf.Abs(bombSpawn.position.x), bombSpawn.position.y, bombSpawn.position.z);
             bomb.direction = Vector2.right;
         }

         // Setup the bomb's spawn and target. It will animate itself from spawn to the target by means of physics!
         bomb.setSpawnAndTarget(bombSpawn.position, new Vector2(_player.transform.position.x, _player.transform.position.y));

         // Put the bomb on the stage.
         bomb.transform.parent = transform.parent;
     }*/

    public override void onAnimationState (string animState)
	{
		switch (animState) {
		    case AbstractBossControl.ANIM_ATTACK_END:
			    setBossAction (BossAction.stand);
			    break;
		    case AbstractBossControl.ANIM_STUN_END:
			    setBossAction (BossAction.stand);
			    break;
		}
	}

	public override void damage (int damage, AbstractDamageCollider.DamageType type, int knockback)
	{
        // Spider only takes one kind of damage. Hacking override.
        damage = 100;
		base.damage (damage, type, knockback);

        if (isInvincible)
        {
            // Can't hurt this boy.
            return;
        }

        // Spider just takes damage from one source. Make it show.
	    _anim.SetTrigger ("IsHit");
	}
    public void manualSpawn () {
        // Spawns in the spider manually by making it drop from the ceiling.
        setBossAction(BossAction.reappear);
    }
}
