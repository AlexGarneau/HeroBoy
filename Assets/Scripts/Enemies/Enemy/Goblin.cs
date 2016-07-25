using UnityEngine;
using System.Collections;

public class Goblin : AbstractEnemyControl
{
	protected bool highGround;
	protected float bombRangeMax = 3.5f;
	protected float bombCooldownTime = 3f;
	protected float bombCooldown = 3f;

	public GameObject goblinDart;
	public Collider2D lightHit;

	public GameObject healItem;

	protected override void Start ()
	{
        base.Start();
		base._enemHealth = 40f;
		base._enemMoveSpeed = 3f;
		base.enemDamage = 2;
		base._attackRange = 1.2f;
		base._vertRange = 0.1f;
		base.isAlive = true;
		base.isMoving = false;

        base.bulletSpawn = transform.Find ("BulletSpawn");

		_damageColliders = GetComponentsInChildren<EnemyDamageCollider> ();
		if (_damageColliders != null && _damageColliders.Length > 0) {
			// Sets the damage of damage colliders. TODO: Independent damage set to different colliders, if multple exist.
			for (int i = _damageColliders.Length - 1; i >= 0; i--) {
				_damageColliders [i].gameObject.SetActive (false);
			}
		}

		EnemyAbstractBehaviour[] eabs = _anim.GetBehaviours<EnemyAbstractBehaviour> ();
		for (var i = eabs.Length - 1; i >= 0; i--) {
			eabs [i].enemy = this;
		}
	}

	protected override void setState (EnemyStates newState)
	{
		base.setState (newState);
		switch (newState) {
		    case EnemyStates.move:
                // Reset bomb cooldown.
                bombCooldown = bombCooldownTime;
                _anim.SetBool ("IsMoving", true);
			    break;
		    case EnemyStates.attack:
                if (_playerControl.playerHealth > 0) {
                    // Only attack if player is still alive.
			        _anim.SetTrigger ("Fire");
			        _anim.SetBool ("IsMoving", false);
                } else {
                    // Stay on move.
                    setState(EnemyStates.move);
                }
                break;
            case EnemyStates.shoot:
                if (_playerControl.playerHealth > 0) {
                    // Only attack if player is still alive.
                    _anim.SetTrigger("Fire");
                    _anim.SetBool("IsMoving", false);
                } else {
                    // Stay on move.
                    setState(EnemyStates.move);
                }
                break;
            case EnemyStates.dead:
			    _anim.SetBool ("IsMoving", false);
			    break;
		}
	}

	public override void onAnimationState (string animState)
	{
		switch (animState) {
		    case AbstractEnemyControl.ANIM_SPAWN_END:
                bombCooldown = bombCooldownTime;
			    break;
		    case AbstractEnemyControl.ANIM_ATTACK_START:
			    Shoot ();
			    break;
		    case AbstractEnemyControl.ANIM_ATTACK_END:
			    setState (EnemyStates.move);
			    break;
		    case AbstractEnemyControl.ANIM_INJURED_END:
			    if (_enemHealth > 0) {
				    setState (EnemyStates.move);
			    }
			    break;
            case AbstractEnemyControl.ANIM_SHOOT_START:
                Shoot();
                break;
            case AbstractEnemyControl.ANIM_DEATH_END:
			    randomdrop (healItem);
			    Destroy (gameObject);
			    break;
		}

        base.onAnimationState(animState);
    }

	protected override void MoveToPlayer ()
	{
		//Debug.Log ("This is working: MoveToPlayer.");
		float vD = _player.transform.position.y - this.transform.position.y;
		
		// Y-positioning - move enemy to the player's level at all times.
		if (_player.transform.position.y - _vertRange > this.transform.position.y) {
			transform.Translate (new Vector3 (0, _enemMoveSpeed * Time.deltaTime, 0));
		} else if (_player.transform.position.y + _vertRange < this.transform.position.y) {
			transform.Translate (new Vector3 (0, -_enemMoveSpeed * Time.deltaTime, 0));
		}

		// Pirate is different than zombie. If he's less than half the gun range, he moves in for attack.
		// If more, then he will move back while firing his gun.
		float hD = this.transform.position.x - _player.transform.position.x;

		// At gun range. Back away.
		if (hD > bombRangeMax) {
			// Out of gun range. Move in right.
			facingLeft = true;
			transform.Translate (new Vector3 (Mathf.Max (bombRangeMax - hD, -_enemMoveSpeed * Time.deltaTime), 0, 0));
		} else if (hD < -bombRangeMax) {
			// Out of gun range. Move in left.
			facingLeft = false;
			transform.Translate (new Vector3 (Mathf.Min (-bombRangeMax - hD, _enemMoveSpeed * Time.deltaTime), 0, 0));
		} else if (hD <= bombRangeMax && hD >= 0) {
			// Within gun range. Move back right as you fire.
			facingLeft = true;
			transform.Translate (new Vector3 (Mathf.Min (bombRangeMax - hD, _enemMoveSpeed * Time.deltaTime), 0, 0));
		} else if (hD >= -bombRangeMax && hD < 0) {
			// Within gun range. Move back left as you fire.
			facingLeft = false;
			transform.Translate (new Vector3 (Mathf.Max (-bombRangeMax - hD, -_enemMoveSpeed * Time.deltaTime), 0, 0));
		}
	}

	protected override void Shoot ()
	{
        GameObject go;
        GoblinDart dart;
        if (facingLeft)
        {
            go = Instantiate(goblinDart);
            dart = go.GetComponent<GoblinDart>();
            bulletSpawn.position.Set(-Mathf.Abs(bulletSpawn.position.x), bulletSpawn.position.y, bulletSpawn.position.z);
            dart.direction = Vector2.left;
        }
        else {
            go = Instantiate(goblinDart);
            dart = go.GetComponent<GoblinDart>();
            bulletSpawn.position.Set(Mathf.Abs(bulletSpawn.position.x), bulletSpawn.position.y, bulletSpawn.position.z);
            dart.direction = Vector2.right;
            dart.transform.localScale = new Vector3(-1,1,1);
        }

        // Stick the bullet in the spawner.
        dart.transform.position = bulletSpawn.position;

        // Put the bullet on the stage.
        dart.transform.parent = transform.parent;
    }

	public override void damage (int damage, AbstractDamageCollider.DamageType type, int knockback)
	{
		base.damage (damage, type, knockback);

		if (facingLeft == true) {
			xForce = knockback * .01f;
		} else {
			xForce = -knockback * .01f;
		}

		switch (type) {
		case AbstractDamageCollider.DamageType.light:
			_anim.SetTrigger ("IsHit");
			break;
		case AbstractDamageCollider.DamageType.medium:
			_anim.SetTrigger ("IsHit");
			break;
		case AbstractDamageCollider.DamageType.heavy:
			_anim.SetTrigger ("IsHit");
			break;
		}
	}

	public override void stun (float timeInSec)
	{
		base.stun (timeInSec);
		_anim.SetTrigger ("IsStunned");
	}
}
