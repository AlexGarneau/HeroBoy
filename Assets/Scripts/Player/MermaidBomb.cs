using UnityEngine;
using System.Collections;

public class MermaidBomb : AbstractBullet
{
	private float explosionDelay = 2f;
	private float fizzDelay = 0f;
	private float fizzDelayMax = 1f;
	private bool fizzStateA = true;

	protected enum MermaidState
	{
		fly,
		explode
	}
	;
	protected MermaidState state;

	protected Vector2 spawnLocation;
	protected Vector2 targetLocation;
	protected float lifeMax = 0f;
	protected float rotation = 0f;
	protected float horiV = 0f;
	protected float vertV = 0f;
	protected float vAccel = 0f;
	protected bool canCollide = true;
	
	private AudioSource audio;

	private Rect mermaidA = new Rect (380, 14, 155, 262);
	private Rect mermaidB = new Rect (635, 15, 158, 263);

	public override void Start ()
	{
		base.Start ();

		damage = 40;
		lifeMax = life = Random.Range (0f, 4f) + 4.5f;

		transform.localScale *= .5f;
	}

	public void setSpawnAndTarget (Vector2 spawn, Vector2 target)
	{
		// Instead of lerp, this will have a velocity and acceleration.
		// Because of vAccel, this bomb won't actually reach its target. Just gives it something to shoot for.
		spawnLocation = spawn;
		targetLocation = target;
		transform.position = new Vector3 (spawn.x, spawn.y, transform.position.z);
		horiV = (target.x - spawn.x) / life;
		vertV = (target.y - spawn.y) / life;
		rotation = Random.Range (-20f, 20f);
		
		audio = GetComponent<AudioSource> ();
        AudioClip clip = AbstractAssetController.getSFX(AbstractAssetController.SFX_MERMAID);
        if (clip != null) {
            audio.clip = clip;
        }
		audio.maxDistance = 5;
		audio.volume = .1f;
		state = MermaidState.fly;
	}

	public override void Update ()
	{
		base.Update ();

		// Yes. Alright, state time.
		switch (state) {
		case MermaidState.fly:
			// Using velocity to move this one.
			float time = Time.deltaTime;
			transform.Translate (horiV * time, vertV * time, 0);
			vertV += vAccel * time;

			// Rotate now.
			//transform.Rotate (0f, 0f, rotation);

			life -= time;
			if (life <= 0 || transform.position.y < -9) {
				// Destroy. TODO: Show explosion?
				audio.clip = explosionClip;
				audio.loop = false;
				audio.Play ();
				destroy ();
			}
			break;
		}
	}

	public override void OnTriggerEnter2D (Collider2D collider)
	{
		if (state == MermaidState.fly) {
			Debug.Log ("What Did I Hit? " + collider.tag);
			AbstractClass ac = collider.GetComponent<AbstractClass> ();
			if (ac != null) {
				AbstractEnemyControl ec = collider.GetComponent<AbstractEnemyControl> ();
				if (ec != null) {
					// Give pain and bounce.
					ec.damage (damage, AbstractDamageCollider.DamageType.heavy, knockback);
					//state = MermaidState.explode;
					//explode ();
				}
			}
		}
	}

	protected void playSplat ()
	{
		//audio.Play ();
	}
}


