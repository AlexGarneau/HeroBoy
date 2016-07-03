using UnityEngine;
using System.Collections;

public class EnemyDamageCollider : AbstractDamageCollider
{
	void Start ()
	{

    }

    /*
    // NOT NEEDED - Player Control handles all this.
	void OnTriggerEnter2D (Collider2D other)
	{
        PlayerControl player = other.GetComponent <PlayerControl> ();
		if (player && other.GetComponent<PlayerDamageCollider>() != null) {
            player.damage(damage, AbstractDamageCollider.DamageType.light, knockback);
        }
    }

    void OnTriggerStay2D (Collider2D other)
    {
        PlayerControl player = other.GetComponent<PlayerControl>();
        if (player && other.GetComponent<PlayerDamageCollider>() != null)
        {
            // Keep calling damage. If player's invulnerable, won't hurt him. Doesn't matter.
            player.damage(damage, AbstractDamageCollider.DamageType.light, knockback);
        }
    }
    //*/
}

