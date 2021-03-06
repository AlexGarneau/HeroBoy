using UnityEngine;
using System.Collections;

public class Dodgeball : AbstractBullet
{
	public override void Start ()
	{
		base.Start ();

		damage = 10;
		velocity = 3.5f;
		life = 4;
		knockback = 1;
	}

	public override void Update ()
	{
        base.Update();
		if (life > 0) {
			transform.Translate (velocity * direction.x * Time.deltaTime, velocity * direction.y * Time.deltaTime, 0);
			life -= Time.deltaTime;
			if (life <= 0) {
				destroy ();
			}
		}
	}

	public override void OnTriggerEnter2D (Collider2D collider)
	{
		Debug.Log ("What Did I Hit? " + collider.tag);
		PlayerControl pc = collider.GetComponent<PlayerControl> ();
		if (pc) {
			pc.damage (damage, AbstractDamageCollider.DamageType.light, knockback);
			destroy ();
		}
	}
}


