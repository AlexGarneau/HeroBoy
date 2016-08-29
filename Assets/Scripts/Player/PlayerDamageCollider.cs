using UnityEngine;
using System.Collections;

public class PlayerDamageCollider : AbstractDamageCollider
{
    public GameObject hitSplat;

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
            GameObject splat = Instantiate(hitSplat) as GameObject;
            splat.transform.parent = enemy.transform;
            splat.transform.position = enemy.transform.position;
            splat.transform.Translate(0, 1, -1);
		} else {
			AbstractBossControl boss = other.GetComponent <AbstractBossControl> ();
			if (boss) {
				// Hit a boss! Do death!
				boss.damage (damage, type, knockback);
                GameObject splat = Instantiate(hitSplat) as GameObject;
                splat.transform.parent = boss.transform;
                splat.transform.position = boss.transform.position;
                splat.transform.Translate(0, 1, -1);
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

