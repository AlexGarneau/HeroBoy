using UnityEngine;
using System.Collections;

public class WebBullet : AbstractBullet
{
    private const float OFFSET_MAX = 1f;
    private const float MAX_LIFE = 3f;
    private const float ARC_HEIGHT = 5f;

    private float offset = 0;
    private Vector3 _spawn;
    private Vector3 _target;

    private enum ShotState {
        flying,
        splat
    }
    private ShotState state;

    private BoxCollider2D box;

	public override void Start ()
	{
		base.Start ();

		damage = 0;
		velocity = 3.5f;
		life = MAX_LIFE;
		knockback = 1;

        box = GetComponent<BoxCollider2D>();
        box.enabled = false;
        offset = Random.Range(-OFFSET_MAX, OFFSET_MAX);
        state = ShotState.flying;
	}

    public void setSpawnAndTarget (Vector3 spawn, Vector3 target) {
        _spawn = spawn;
        _target = target;
        this.transform.position = spawn;
    }

	public override void Update ()
	{
        if (state == ShotState.flying) {
            life -= Time.deltaTime;
            if (life <= 0) {
                life = 0;
                state = ShotState.splat;
                box.enabled = true;
            }
            Vector3 travel = Vector3.Lerp(_spawn, _target, 1 - (life / MAX_LIFE));
            travel.y += Mathf.Sin((life / MAX_LIFE) * Mathf.PI) * ARC_HEIGHT;
            float dx = travel.x - transform.position.x;
            float dy = travel.y - transform.position.y;
            Debug.Log("Angle: " + Mathf.Atan2(dy, dx) * 180 / Mathf.PI);
            transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dy, dx) * 180 / Mathf.PI);
            transform.position = travel;
        } else {
            // Vanish right away. Reason we add this state is so the box collider can be enabled for a frame to hit the player.
            Destroy(gameObject);
        }
	}

	public override void OnTriggerEnter2D (Collider2D collider)
	{
		Debug.Log ("WebBullet: What Did I Hit? " + collider.tag);
		PlayerControl pc = collider.GetComponent<PlayerControl> ();
		if (pc) {
			pc.damage (damage, AbstractDamageCollider.DamageType.stunAll, knockback);
			destroy ();
		}
	}
}


