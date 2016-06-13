using UnityEngine;
using System.Collections;

public class AbstractDamageCollider : AbstractClass
{
	public int damage;
	public int knockback;
	public enum DamageType
	{
		light,
		medium,
		heavy,
        ranged
	}
	;
	public DamageType type;
}

