using UnityEngine;
using System.Collections;

public class BossPirateNew : AbstractBossControl
{
    /*  NEW PIRATE BOSS
            REMAINS ON UPPER DECK
            THROWS BOMBS
            ONLY DAMAGED BY A SUPER MERMAID CANNON SITTING IN THE MIDDLE, SEPARATE FROM MAX'S MERMAID CANNON
            ADDS ATTACK AND ARE BEATEN BY THE PLAYER, CHARGING THE MERMAID CANNON.
            ALSO SOMETIMES RAINS CHARGE BAR CHARGERS
            WHEN THE PLAYER FIRES THE MERMAID CANNON, IT DOES A SET AMOUNT OF DAMAGE AND SETS THE NEXT STATE IN THE BOSS
            ALSO LAUNCHES A BIRD EVERY STAGE.
                STATE 1     THREE SPAWN POINTS, BOMBS ARE THROWN SLOW
                STATE 2     FOUR SPAWN POINTS, BOMBS ARE THROWN SLIGHTLY FASTER
                STATE 3     FOUR SPAWN POINTS, BOMBS ARE THROWN FAST
        */

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

	private bool highGround = true;
	private Transform bombSpawn;
	private float bombDelay = 0;
	private float bombDelayMax = 2f;

	protected override void Start ()
	{
		base.Start ();

		base._bossMaxHealth = base._bossHealth = 50f;
		base._enemMoveSpeed = 1.2f;
		base.enemDamage = 2f;
		base._attackRange = 2f;
		base._vertRange = 0.2f;
		base.isAlive = true;
		base.isMoving = false;
		base._player = GameObject.FindGameObjectWithTag ("Player");
        bombSpawn = transform.Find ("BombSpawn");

        _anim.SetBool("HighGround", true);
        bossState = BOSS_STATE_1;
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

		_anim.SetFloat ("Health", _bossHealth);

		base.Update ();
	}

	public override void setBossAction (BossAction newState)
	{
        if (state == newState) {
            // Same state. Do nothing.
            return;
        }

        //Debug.Log("Set Boss Action: " + newState);
        _anim.SetBool("IsMoving", false);

        switch (newState) {
		case BossAction.attack:
			_anim.SetTrigger ("Bombthrow");
			break;
		case BossAction.dead:
			break;
		}

		base.setBossAction (newState);
	}

	/** States called by the animator. */
	public override void onAnimationState (string animState)
	{
        switch (animState)
        {
            case AbstractEnemyControl.ANIM_SPAWN_END:
                // Boss spawned.
                highGround = true;
                _anim.SetBool("HighGround", highGround);
                setBossAction(BossAction.stand);
                break;
            case AbstractBossControl.ANIM_ATTACK_END:
                bombDelay = bombDelayMax;
                setBossAction(BossAction.stand);
                break;
            case AbstractBossControl.ANIM_SPECIAL_END:
                setBossAction(BossAction.move);
                break;
            case AbstractBossControl.ANIM_DEATH_END:
                SendMessageUpwards("bossDead", SendMessageOptions.DontRequireReceiver);
                Destroy(gameObject);
                break;
            case AbstractBossControl.ANIM_STUN_START:
                setBossAction(BossAction.retreat);
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
}
