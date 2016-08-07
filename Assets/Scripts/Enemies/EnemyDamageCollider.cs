using UnityEngine;
using System.Collections;

public class EnemyDamageCollider : AbstractDamageCollider
{
	Animator _pAnim;

	void Start ()
	{
		_pAnim = GameObject.FindGameObjectWithTag ("Player").GetComponent<Animator> ();
    }

	void OnTriggerEnter2D (Collider2D other)
	{
		PlayerControl player = other.GetComponent <PlayerControl> ();
		if (player && other.GetComponent<PlayerDamageCollider>() != null) {
            if(player.tempInvuln != true)
            {
			    // Hit a player! Do death!
			    _pAnim.SetTrigger ("IsHit");
			    player.damage (damage, AbstractDamageCollider.DamageType.light, knockback);
            }
        }
    }
}

