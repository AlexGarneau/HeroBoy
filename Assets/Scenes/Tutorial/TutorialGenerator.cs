using UnityEngine;
using System.Collections;

public class TutorialGenerator : AbstractEnemyControl {

    public float secondsBetweenShots = 2f;

    public GameObject dodgeball;

    public float arcAngle = 10f;

    protected float timer = 0f;

	// Use this for initialization
	void Start () {
        base.Start();

        base._enemHealth = 1;
        timer = secondsBetweenShots;
	}
	
	// Update is called once per frame
	void Update () {
	    if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Shoot();
                timer = secondsBetweenShots;
            }
        }
	}

    void Shoot ()
    {
        GameObject go;
        Dodgeball bullet;

        float angle = arcAngle * Mathf.PI / 180f;
        float range = Random.Range(-angle, angle);

        go = Instantiate(dodgeball);
        bullet = go.GetComponent<Dodgeball>();
        bullet.direction = new Vector2(-Mathf.Cos(range), Mathf.Sin(range));

        // Stick the bullet in the spawner.
        go.transform.position = bulletSpawn.position;

        // Put the bullet on the stage.
        go.transform.SetParent(transform.parent, true);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("What hit gener? " + col);
        PlayerDamageCollider pc = col.GetComponent<PlayerDamageCollider>();
        if (pc != null)
        {
            _anim.SetTrigger("IsHit");
            timer = -1;
        }
    }
}
