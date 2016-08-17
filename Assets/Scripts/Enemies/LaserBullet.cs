using UnityEngine;
using System.Collections;

public class LaserBullet : AbstractBullet
{
    protected Animator _anim;
    protected float angle;

    protected float playerHeightHalf = 1f;
    protected float targetAngle = 0;
    protected Vector3 spawnPosition;
    protected Vector3 targetPosition;
    protected float lifeMax = 0;

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

        damage = 5;
		velocity = 2f;
		knockback = 1;
        life = 0;
        lifeMax = 3f; // Laser takes 3 seconds to hit no matter where player is.

        angle = 90f;
	}

	public override void Update ()
	{
        if (life >= lifeMax) {
            transform.position = targetPosition;
            explode();
        } else {
            life += Time.deltaTime;
            transform.position = Vector3.Lerp(spawnPosition, targetPosition, life / lifeMax);
        }
    }

    public override void setSpawnAndTarget (Transform spawn, Transform target) {
        spawnPosition = new Vector3(spawn.position.x, spawn.position.y, spawn.position.z);
        targetPosition = new Vector3(target.position.x, target.position.y, target.position.z);
    }

    public override void explode () {
        _anim.SetTrigger("Explode");
        GetComponent<CircleCollider2D>().radius = 1.5f;
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


