using UnityEngine;
using System.Collections;

public class BossPirate : AbstractBossControl
{
	private static int BOSS_STATE_1 = 0;
	private static int BOSS_STATE_2 = 1;
	private static int BOSS_STATE_3 = 2;
	private static int BOSS_STATE_FINAL = 3;
    private static float MELEE_COOLDOWN = 1f;

	public Collider2D lightHit;

	public PirateParrot parrot75;
	public PirateParrot parrot50;
	public PirateParrot parrot25;
	public GameObject pirateBomb;

	private bool highGround = false;
	private Transform bombSpawn;
	private float bombDelay = 0;
	private float bombDelayMax = 2f;

	protected override void Start ()
	{
		base._bossMaxHealth = base._bossHealth = 50f;
		base._enemMoveSpeed = 1.2f;
		base._enemDamage = 2f;
		base._attackRange = 2f;
		base._vertRange = 0.2f;
		base.isAlive = true;
		base.isMoving = false;
		base._player = GameObject.FindGameObjectWithTag ("Player");
        bombSpawn = transform.Find ("BombSpawn");

		bossState = BOSS_STATE_1;

		base.Start ();
	}

	protected override void Update ()
	{
		switch (state) {
		case BossAction.move:
			break;
		case BossAction.attack:
			break;
		case BossAction.stand:
			// Just keep lobbing those bombs. You want a delay timer between throws, you know what to do. Ref: Pirate Gun delay.
			bombDelay -= Time.deltaTime;
			if (bombDelay <= 0) {
				setBossAction (BossAction.attack);
			}
			break;
		case BossAction.dead:
			//DeathTimerDestroy ();
			break;
		}

        if (meleeCooldown > 0)
        {
            meleeCooldown -= Time.deltaTime;
            if (meleeCooldown < 0)
            {
                meleeCooldown = 0;
            }
        }
		_anim.SetFloat ("Health", _bossHealth);

		base.Update ();
	}

	public override void setBossAction (BossAction newState)
	{
        if (state == newState) {
            // Same state. Do nothing.
            return;
        }

        Debug.Log("Set Boss Action: " + newState);
        _anim.SetBool("IsMoving", false);

        switch (newState) {
		case BossAction.move:
			_anim.SetBool ("IsMoving", true);
			break;
		case BossAction.attack:
			if (highGround) {
				// On High Ground. Throw bomb.
				_anim.SetTrigger ("Bombthrow");
			} else if (meleeCooldown <= 0) {
                // On low ground. Use melee.
                _anim.SetTrigger("Attack");
			}
			
			break;
		case BossAction.retreat:
			// Retreat is when the boss leaves the stage. Let his lackeys do some death.
			_anim.SetTrigger ("Drawsword");
            break;
		case BossAction.reappear:
			// Boss is in the air.
			if (highGround) {
				// Position boss on the player's area.
				highGround = false;
				_anim.SetBool ("HighGround", highGround);
				transform.position = new Vector3 (-6.67f, 1.23f, transform.position.z);
			} else {
				// Position boss on his stand.
				highGround = true;
				_anim.SetBool ("HighGround", highGround);
				transform.position = new Vector3 (6.29f, 1.23f, transform.position.z);
			}
            break;
		case BossAction.dead:
			break;
		}

		base.setBossAction (newState);
	}

	/** States called by the animator. */
	public override void onAnimationState (string state)
	{
		Debug.Log ("AnimataionState:" + state);

		switch (state) {
		case AbstractEnemyControl.ANIM_SPAWN_END:
			// Boss spawned.
			highGround = true;
            _anim.SetBool("HighGround", highGround);
            setBossAction (BossAction.retreat);
			break;
		case AbstractBossControl.ANIM_ATTACK_END:
			if (highGround) {
				bombDelay = bombDelayMax;
				setBossAction (BossAction.stand);
			} else {
                meleeCooldown = MELEE_COOLDOWN;
				setBossAction (BossAction.move);
			}
			break;
		case AbstractBossControl.ANIM_SPECIAL_END:
			setBossAction (BossAction.move);
			break;
		case AbstractBossControl.ANIM_DEATH_END:
			SendMessageUpwards ("bossDead", SendMessageOptions.DontRequireReceiver);
			Destroy (gameObject);
			break;
		case AbstractBossControl.ANIM_RETREAT_END:
			// Call land.
			setBossAction (BossAction.reappear);
			break;
		case AbstractBossControl.ANIM_STUN_START:
            setBossAction (BossAction.retreat);
			break;
		case AbstractBossControl.ANIM_REAPPEAR_END:
			// Retreat finished.
			if (highGround) {
				// Boss is on the platform. Spawn ads.
				int adCount;
				if (bossState == BOSS_STATE_3) {
					adCount = 3;
				} else if (bossState == BOSS_STATE_2) {
					adCount = 2;
				} else if (bossState == BOSS_STATE_1) {
					adCount = 1;
				} else {
					adCount = 5;
				}
				// Spawn them ads, yo.
				SendMessageUpwards ("spawnAds", adCount, SendMessageOptions.DontRequireReceiver);

				// Boss doesn't move from this spot.
				setBossAction (BossAction.stand);
			} else {
                // Boss is off the platform. Make him move, but not attack immediately.
                meleeCooldown = MELEE_COOLDOWN;
                setBossAction (BossAction.move);
			}
			break;
		}
	}

	public void ThrowBomb ()
	{
		// Create a bomb and make it fly.
		GameObject go = Instantiate (pirateBomb);
		PirateBomb bomb = go.GetComponent<PirateBomb> ();

		// Position the spawner and the direction.
		if (facingLeft) {
			bombSpawn.position.Set (-Mathf.Abs (bombSpawn.position.x), bombSpawn.position.y, bombSpawn.position.z);
			bomb.direction = Vector2.left;
		} else {
			bombSpawn.position.Set (Mathf.Abs (bombSpawn.position.x), bombSpawn.position.y, bombSpawn.position.z);
			bomb.direction = Vector2.right;
		}

		// Setup the bomb's spawn and target. It will animate itself from spawn to the target by means of physics!
		bomb.setSpawnAndTarget (bombSpawn.position, new Vector2 (_player.transform.position.x, _player.transform.position.y));
		
		// Put the bomb on the stage.
		bomb.transform.parent = transform.parent;
	}

	public override void damage (int damage, AbstractDamageCollider.DamageType type, int knockback)
	{
		if (state != BossAction.attack && state != BossAction.move) {
			// Can't damage him any other time than move and attack.
			return;
		}

		_bossHealth -= damage;
		if (_bossHealth <= 0) {
			// Boos is dead. Or is it?
			if (bossState == BOSS_STATE_FINAL) {
				SendMessageUpwards ("bossDead", SendMessageOptions.DontRequireReceiver);
			} else {
				stun (1f);
			}
		}
	}

	public override void stun (float timeInSec)
	{
		base.stun (timeInSec);
		_anim.SetTrigger ("Stagger");
	}

	public override void adsKilled ()
	{
		PirateParrot parrot;
		if (bossState == BOSS_STATE_3) {
			parrot = parrot25;
			bossState = BOSS_STATE_FINAL;
			base._bossHealth = base._bossMaxHealth / 2;
		} else if (bossState == BOSS_STATE_2) {
			parrot = parrot50;
			bossState = BOSS_STATE_3;
			base._bossHealth = base._bossMaxHealth;
		} else if (bossState == BOSS_STATE_1) {
			parrot = parrot75;
			bossState = BOSS_STATE_2;
			base._bossHealth = base._bossMaxHealth;
		} else {
			return;
		}
		parrot.flyAtTarget (_player.gameObject);

		setBossAction (BossAction.retreat);
	}
}
