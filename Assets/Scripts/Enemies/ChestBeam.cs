using UnityEngine;
using System.Collections;

public class ChestBeam : AbstractBullet
{
    protected Animator _anim;

	public override void Start ()
	{
		base.Start ();
        _anim = GetComponent<Animator>();
        if (_anim != null) {
            BulletExplodeBehaviour[] eabs = _anim.GetBehaviours<BulletExplodeBehaviour>();
            for (var i = eabs.Length - 1; i >= 0; i--) {
                eabs[i].bullet = this;
            }
        }

        damage = 10;
	}

    public override void explosionEnd () {
        destroy();
    }

    public override void OnTriggerEnter2D (Collider2D collider)
	{
		Debug.Log ("What Did I Hit? " + collider.tag);
		PlayerControl pc = collider.GetComponent<PlayerControl> ();
		if (pc) {
			pc.damage (damage, AbstractDamageCollider.DamageType.light, knockback);
            explode();
        }
	}
}


