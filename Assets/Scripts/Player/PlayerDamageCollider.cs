using UnityEngine;
using System.Collections;

public class PlayerDamageCollider : AbstractDamageCollider
{
	private bool _manualDamage = false;
	public bool manualDamage {
		get {
			return _manualDamage;
		}
		set {
			_manualDamage = value;
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
        AbstractEnemyControl enemy = other.GetComponent <AbstractEnemyControl> ();
		if (enemy) {
			// Hit an enemy! Do death!
			enemy.damage (damage, type, knockback);
		} else {
			AbstractBossControl boss = other.GetComponent <AbstractBossControl> ();
			if (boss) {
				// Hit a boss! Do death!
				boss.damage (damage, type, knockback);
			}
		}
	}

	public void damageOutputPowerKick ()
	{

	}

	public void setDamageKnockback (int newDamage, int newKnockback)
	{
		damage = newDamage;
		knockback = newKnockback;
	}
}

