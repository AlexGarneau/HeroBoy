using UnityEngine;
using System.Collections;

public class MissileHoming : AbstractBullet
{
    protected Animator _anim;
    protected float angle;

    protected float playerHeightHalf = 1f;

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
		velocity = 1.5f;
		life = 4;
		knockback = 1;

        angle = 90f;
	}

	public override void Update ()
	{
		if (life > 0) {
            float dx = target.position.x - transform.position.x;
            float dy = target.position.y + playerHeightHalf - transform.position.y;

            float targetAngle = Mathf.Atan2(dy, dx);
            // Loop angle over.
            while (targetAngle < 0) { targetAngle += Mathf.PI * 2; }
            while (targetAngle > Mathf.PI * 2) { targetAngle -= Mathf.PI * 2; }

            float direction = targetAngle > angle ? 1 : -1;
            if (Mathf.Abs(targetAngle - angle) > Mathf.PI) {
                // Go the other way.
                direction *= -1;
            }

            angle += (Mathf.PI / 90f) * direction;

            // Loop angle over.
            while (angle < 0) { angle += Mathf.PI * 2; }
            while (angle > Mathf.PI * 2) { angle -= Mathf.PI * 2; }

            transform.Translate (0, -velocity * Time.deltaTime, 0);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90 + (angle * 180 / Mathf.PI));

            life -= Time.deltaTime;
			if (life <= 0) {
                explode();
            }
		}
    }

    public override void explode () {
        _anim.SetTrigger("Explode");
        GetComponent<CircleCollider2D>().radius = 1.8f;
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


