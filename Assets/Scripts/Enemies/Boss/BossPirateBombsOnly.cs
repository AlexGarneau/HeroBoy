using UnityEngine;
using System.Collections;

public class BossPirateBombsOnly : AbstractBossControl
{
    /*  Pirate Boss that does nothing but throw bombs. Meant to be a separate entity to the real boss. */

	public GameObject pirateBomb;

    private Transform bombSpawn;
    private float bombDelay = 0;
    private float bombAmmo = 0;
    private float bombDelayMax = 4f;

    protected override void Start ()
	{
		base.Start ();

		base._player = GameObject.FindGameObjectWithTag ("Player");
        bombSpawn = transform.Find ("BombSpawn");
	}

	protected override void Update ()
	{
		switch (state) {
		    case BossAction.move:
			    break;
		    case BossAction.attack:
			    break;
		    case BossAction.stand:
                // Just keep lobbing those bombs.
                bombDelay -= Time.deltaTime;
                if (bombDelay <= 0) {
                    bombAmmo = 3;
                    _anim.SetTrigger("Bombthrow");
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
            case AbstractBossControl.ANIM_ATTACK_END:
                if (bombAmmo > 0) {
                    // Throw more bombs!
                    bombAmmo--;
                    _anim.SetTrigger("Bombthrow");
                } else {
                    bombDelay = bombDelayMax;
                    setBossAction(BossAction.stand);
                }
                break;
            case AbstractBossControl.ANIM_STUN_END:
                setBossAction(BossAction.stand);
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

	public override void stun (float timeInSec)
	{
		_anim.SetTrigger ("Stagger");
	}
}
