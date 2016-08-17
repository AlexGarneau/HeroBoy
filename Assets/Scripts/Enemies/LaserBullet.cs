using UnityEngine;
using System.Collections;

public class LaserBullet : AbstractBullet
{
    protected Animator _anim;
    protected float angle;

    protected float playerHeightHalf = 1f;
    protected float targetAngle = 0;
    protected Vector3 targetPosition;

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

        angle = 90f;
	}

	public override void Update ()
	{
        Debug.Log("update bullet: " + Vector3.Distance(transform.position, targetPosition) + " < " + (velocity * Time.deltaTime));
        if (Vector3.Distance(transform.position, targetPosition) < velocity * Time.deltaTime) {
            transform.position = targetPosition;
            explode();
        } else {
            transform.Translate(velocity * Time.deltaTime * Mathf.Cos(targetAngle), velocity * Time.deltaTime * Mathf.Sin(targetAngle), 0);
        }
    }

    public override void setTarget (Transform target) {
        targetPosition = target.position;
        targetPosition.y += playerHeightHalf;

        float dx = targetPosition.x - transform.position.x;
        float dy = targetPosition.y - transform.position.y;

        targetAngle = Mathf.Atan2(dy, dx);

        // Loop angle over.
        while (targetAngle < 0) { targetAngle += Mathf.PI * 2; }
        while (targetAngle > Mathf.PI * 2) { targetAngle -= Mathf.PI * 2; }
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


