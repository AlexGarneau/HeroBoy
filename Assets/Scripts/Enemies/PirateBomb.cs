using UnityEngine;
using System.Collections;

public class PirateBomb : AbstractBullet
{
	private float explosionDelay = 2f;
	private PlayerControl _player = null;
	private float fizzDelay = 0f;
	private float fizzDelayMax = 1f;
	private bool fizzStateA = true;
	private Animator _anim;

	protected enum BombState
	{
		fly,
		delay,
		explode}
	;

	protected BombState state;

	protected Vector2 spawnLocation;
	protected Vector2 targetLocation;
	protected float arcHeight = 0f;
	protected float lifeMax = 0f;
    
	public override void Start ()
	{
		base.Start ();

		damage = 40;
		lifeMax = life = 2.5f;
		explosionTime = 1f;
		_anim = GetComponent<Animator> ();

		source = gameObject.GetComponent<AudioSource> ();
	}

	public void setSpawnAndTarget (Vector2 spawn, Vector2 target)
	{
		spawnLocation = spawn;
		targetLocation = target;
		arcHeight = ((Mathf.Abs (spawn.y - target.y)) + 2);
		transform.position = new Vector3 (spawn.x, spawn.y, transform.position.z);

		Debug.Log (spawn + " --- " + target + " --- " + arcHeight + " --- ");
	}

	public override void Update ()
	{
		base.Update ();

		// Yes. Alright, state time.
		switch (state) {
		case BombState.fly:
			// Bomb is flying through the air like a pancake on a British airline.
			// Move the bomb along a straight line.
			Vector2 pos = Vector2.Lerp (spawnLocation, targetLocation, 1 - (life / lifeMax));

			// But wait, Splice! I thought you said there's an arc!
			float arcRatio = Mathf.Sin ((life / lifeMax) * Mathf.PI);

			// Ok, so that means ratio is 0 at 0 health, 1 at half health, and 0 again at full health...
			// Ooh! You multiplied that with the max arc height! Apply it to the y position!
			transform.position = new Vector3 (pos.x, pos.y + (arcRatio * arcHeight), transform.position.z);

			// Swap the texture for fizz.
			/*
			fizzDelay -= Time.deltaTime;
			if (fizzDelay <= 0) {
				fizzStateA = !fizzStateA;
				int imageID;
				if (fizzStateA) {
					imageID = AbstractAssetController.BOMB_A;
				} else {
					imageID = AbstractAssetController.BOMB_B;
				}
				AbstractAssetController.applyImage (renderer, imageID);
				fizzDelay = fizzDelayMax;
			}
			*/

			life -= Time.deltaTime;
			if (life <= 0) {
				// Doesn't explode immediately...
				life = explosionDelay;
				state = BombState.delay;
			}
			break;
		case BombState.delay:
			// Seconds ticking until explosion.
			/*
			fizzDelay -= Time.deltaTime;
			if (fizzDelay <= 0) {
				fizzStateA = !fizzStateA;
				int imageID;
				if (fizzStateA) {
					imageID = AbstractAssetController.BOMB_A;
				} else {
					imageID = AbstractAssetController.BOMB_B;
				}
				AbstractAssetController.applyImage (renderer, imageID);
				fizzDelay = fizzDelayMax;
			}
			*/

			life -= Time.deltaTime;
			if (life <= 0) {
				// Explosion!
				life = explosionTime;
				state = BombState.explode;

				// Play explosion sound.
				source.clip = explosionClip;
				source.Play ();

				explode ();
			}
			break;
		case BombState.explode:
			_anim.SetTrigger ("Explode");
			if (_player != null) {
				// Hit the player!
				_player.damage (damage, AbstractDamageCollider.DamageType.heavy, knockback);
				_player = null;
			}
			break;
		}
	}

	public override void OnTriggerEnter2D (Collider2D collider)
	{
		Debug.Log ("What Did I Hit? " + collider.tag);
		PlayerControl pc = collider.GetComponent<PlayerControl> ();
		if (pc) {
			_player = pc;
		}
	}

	public void OnTriggerExit2D (Collider2D collider)
	{
		PlayerControl pc = collider.GetComponent<PlayerControl> ();
		if (pc) {
			_player = null;
		}
	}
}


