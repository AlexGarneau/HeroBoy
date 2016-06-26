using UnityEngine;
using System.Collections;

public class BossElephant : AbstractBossControl
{
	private static int BOSS_STATE_MELEE = 0;
	private static int BOSS_STATE_RANGED = 1;
    private static float MELEE_COOLDOWN = 4f;
    private static float BALLOON_COOLDOWN = .1f;
    private static int BALLON_LIMIT = 5;

    public Collider2D lightHit;

	public GameObject balloonDog;

	private bool highGround = false;
	private Transform balloonSpawn;
	private float bombDelay = 0;
	private float bombDelayMax = 2f;
    private float balloonCooldown = 0;
    private float shudderIntensity = 1f;

    private GameObject[] bodyParts;

	protected override void Start ()
	{
		base._bossMaxHealth = base._bossHealth = 400f;
		base._enemMoveSpeed = 1f;
		base._enemDamage = 5f;
		base._attackRange = 4f;
		base._vertRange = 0.2f;
		base.isAlive = true;
		base.isMoving = false;
		base._player = GameObject.FindGameObjectWithTag ("Player");
        balloonSpawn = transform.Find ("BalloonSpawn");

        bodyParts = transform.Find("Body").gameObject.GetComponentsInChildren<GameObject>(false);

		bossState = BOSS_STATE_MELEE;

		base.Start ();
	}

	protected override void Update ()
	{
		switch (state) {
		case BossAction.move:
			break;
		case BossAction.attack:
			break;
		case BossAction.dead:
			//DeathTimerDestroy ();
			break;
		}

        if (bossState == BOSS_STATE_RANGED) {
            shudder();
        }

        if (meleeCooldown > 0) {
            meleeCooldown -= Time.deltaTime;
            if (meleeCooldown < 0) {
                meleeCooldown = 0;
            }
        }
        if (balloonCooldown > 0) {
            balloonCooldown -= Time.deltaTime;
            if (balloonCooldown < 0) {
                balloonCooldown = 0;
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
			if (bossState == BOSS_STATE_MELEE) {
				// On High Ground. Throw bomb.
				_anim.SetTrigger ("Melee");
			} else if (bossState == BOSS_STATE_RANGED) {
                // On low ground. Use melee.
                _anim.SetTrigger("Balloon");
			}
			
			break;
		case BossAction.dead:
			break;
		}

		base.setBossAction (newState);
	}

    /** States called by the animator. */
    public override void onAnimationState (string state) {
        Debug.Log("AnimataionState:" + state);

        switch (state) {
            case AbstractEnemyControl.ANIM_SPAWN_END:
                // Boss spawned.
                highGround = true;
                _anim.SetBool("HighGround", highGround);
                setBossAction(BossAction.retreat);
                break;
            case AbstractBossControl.ANIM_ATTACK_END:
                if (highGround) {
                    bombDelay = bombDelayMax;
                    setBossAction(BossAction.stand);
                } else {
                    meleeCooldown = MELEE_COOLDOWN;
                    setBossAction(BossAction.move);
                }
                break;
            case AbstractBossControl.ANIM_SPECIAL_END:
                setBossAction(BossAction.move);
                break;
            case AbstractBossControl.ANIM_DEATH_END:
                SendMessageUpwards("bossDead", SendMessageOptions.DontRequireReceiver);
                Destroy(gameObject);
                break;
            case AbstractBossControl.ANIM_STUN_START:
                setBossAction(BossAction.move);
                break;
            case AbstractBossControl.ANIM_STUN_END:
                setBossAction(BossAction.move);
                break;
        }
    }

	public void ThrowBomb ()
	{
		// Create a bomb and make it fly.
		GameObject go = Instantiate (balloonDog);
		BalloonDog balloon = go.GetComponent<BalloonDog> ();

		// Position the spawner and the direction.
		if (facingLeft) {
			balloonSpawn.position.Set (-Mathf.Abs (balloonSpawn.position.x), balloonSpawn.position.y, balloonSpawn.position.z);
		} else {
			balloonSpawn.position.Set (Mathf.Abs (balloonSpawn.position.x), balloonSpawn.position.y, balloonSpawn.position.z);
		}

        // Setup the bomb's spawn and target. It will animate itself from spawn to the target by means of physics!
        go.transform.position = balloonSpawn.position;

        // Put the balloon on the stage. This will also localize its position to the stage.
        balloon.transform.parent = transform.parent;
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
			if (bossState == BOSS_STATE_MELEE) {
                // Nope, boss is serious now.
                bossState = BOSS_STATE_RANGED;
                _anim.SetTrigger("Transform");
			} else {
                // YOU CAN'T KILL THIS ONE TODAY!
				stun (1f);
                _bossHealth = 400f;
			}
		}
	}

	public override void stun (float timeInSec)
	{
		base.stun (timeInSec);
		_anim.SetTrigger ("Stagger");
	}

    private void shudder () {
        for (var i = bodyParts.Length - 1; i >= 0; i--) {
            Vector3 pos = bodyParts[i].transform.position;
            Vector3 rot = bodyParts[i].transform.eulerAngles;
            bodyParts[i].transform.position = new Vector3(
                pos.x + Random.Range(-shudderIntensity, shudderIntensity),
                pos.y + Random.Range(-shudderIntensity, shudderIntensity),
                pos.z
            );
            bodyParts[i].transform.eulerAngles = new Vector3(
                rot.x + Random.Range(-shudderIntensity, shudderIntensity),
                rot.y + Random.Range(-shudderIntensity, shudderIntensity),
                rot.z + Random.Range(-shudderIntensity, shudderIntensity)
            );
        }
    }
}
