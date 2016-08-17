using UnityEngine;
using System.Collections;

public class BossAlienRobot : AbstractBossControl
{
    protected bool _useMissiles = true;

    public GameObject missile;
    public Transform missileSpawn;
    public GameObject laser;
    public Transform laserSpawn;

    protected float stateTimer;
    protected float stateTimerMax = 1; // 15
    protected BoxCollider2D box;

    protected override void Start ()
	{
        base.Start();
        base._bossHealth = 500f;
		base._enemMoveSpeed = 1f;
		base.enemDamage = 30;
		base._attackRange = 3f;
		base._vertRange = 0.2f;
		base.isAlive = true;
		base.isMoving = false;

        base._attackCooldownTime = 5f;
        base._attackCooldown = _attackCooldownTime;
        base._specialCooldownTime = 1f;
        base._specialCooldown = _specialCooldownTime;

        _controller = gameObject.GetComponent<MovementController2D> ();
        _vel = Vector3.zero;
        box = GetComponent<BoxCollider2D>();

        stateTimer = stateTimerMax;

        _damageColliders = gameObject.GetComponentsInChildren<EnemyDamageCollider> (true);
		if (_damageColliders != null && _damageColliders.Length > 0) {
			// Sets the damage of damage colliders. TODO: Independent damage set to different colliders, if multiple exist.
			for (int i = _damageColliders.Length - 1; i >= 0; i--) {
				_damageColliders [i].gameObject.SetActive (false);
			}
		}

        BossAbstractBehaviour[] eabs = _anim.GetBehaviours<BossAbstractBehaviour>();
        for (var i = eabs.Length - 1; i >= 0; i--)
        {
            eabs[i].boss = this;
        }
	}

	protected override void Update()
	{
        if (state == BossAction.stand || state == BossAction.move) {
            if (stateTimer > 0) {
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0) {
                    stateTimer = stateTimerMax;
                    setBossAction(state == BossAction.stand ? BossAction.move : BossAction.stand);
                }
            }
        }

		switch (state) {
            case BossAction.stand:
                Debug.Log("Special Cooldown: " + _specialCooldown);
                if (_specialCooldown <= 0) {
                    _specialCooldown = _specialCooldownTime;
                    setBossAction(BossAction.special);
                }
                return;
            case BossAction.move:
                CheckToAttack();
                break;
	        case BossAction.attack:
		        break;
	        case BossAction.dead:
		        break;
		}

		base.Update ();
	}

	public override void setBossAction (BossAction newState)
	{
		switch (newState) {
		    case BossAction.move:
			    _anim.SetBool ("IsMoving", true);
                box.enabled = false;
			    break;
            case BossAction.stand:
                _anim.SetBool ("IsMoving", false);
                box.enabled = true;
                break;
		    case BossAction.attack:
			    _anim.SetBool ("IsMoving", true);
                _anim.SetTrigger("Attack");
                break;
            case BossAction.special:
                _anim.SetBool("IsMoving", false);
                StartCoroutine(FireMissiles());
                break;
            case BossAction.dead:
			    _anim.SetBool ("IsMoving", false);
                SendMessageUpwards("bossDead", null, SendMessageOptions.DontRequireReceiver);
                break;
		}

		base.setBossAction (newState);
	}

    protected override void MoveToPlayer()
    {
        float hD = _player.transform.position.x - this.transform.position.x;
        float vD = _player.transform.position.y - this.transform.position.y;
        float normHD, normVD;

        // Y-positioning - move enemy to the player's level at all times.
        if (_player.transform.position.y - _vertRange > this.transform.position.y) {
            normVD = 1;
        } else if (_player.transform.position.y + _vertRange < this.transform.position.y) {
            normVD = -1;
        } else {
            normVD = 0;
        }

        // X-positioning - move enemy to its closest horizontal range. If too close to the player, back away.
        if (hD > _attackRange) {
            // Enemy is to the left of player. Move right.
            facingLeft = false;
            normHD = 1;
        } else if (hD < -_attackRange) {
            // Enemy is to the right of player. Move left.
            facingLeft = true;
            normHD = -1;
        } else if (hD < _attackRange - 0.1f && hD >= 0) {
            // Enemy is to the left of player, but too close. Move left.
            facingLeft = false;
            normHD = -1;
        } else if (hD > -_attackRange + 0.1f && hD < 0) {
            // Enemy is to the right of player, but too close. Move right.
            facingLeft = true;
            normHD = 1;
        } else {
            normHD = 0;
        }

        float targetVelX = normHD * _enemMoveSpeed;
        float targetVelY = normVD * _enemMoveSpeed;
        _vel.x = Mathf.SmoothDamp(_vel.x, targetVelX, ref velocityXSmoothing, .1f);
        _vel.y = Mathf.SmoothDamp(_vel.y, targetVelY, ref velocityYSmoothing, .1f);
        _controller.Move(_vel * Time.deltaTime);
    }

    public override void onAnimationState (string animState)
	{
		switch (animState) {
            case AbstractEnemyControl.ANIM_SPAWN_END:
                setBossAction(BossAction.move);
                break;
		    case AbstractBossControl.ANIM_ATTACK_START:
			    break;
		    case AbstractBossControl.ANIM_ATTACK_END:
                isInvincible = false;
                setBossAction(BossAction.move);
			    break;
		    case AbstractBossControl.ANIM_SPECIAL_END:
                isInvincible = false;
                setBossAction(BossAction.stand);
                break;
		    case AbstractBossControl.ANIM_DEATH_END:
                SendMessageUpwards("bossDead", SendMessageOptions.DontRequireReceiver);
			    Destroy (gameObject);
			    break;
        }
    }

    protected IEnumerator FireMissiles () {
        _anim.SetTrigger("Rocket");
        yield return new WaitForSeconds(1f);
        if (this._bossHealth < this._bossMaxHealth / 2) {
            // Low health. Fire missiles.
            for (int i = 10; i > 0; i--) {
                ShootMissile();
                yield return new WaitForSeconds(0.5f);
            }
        } else {
            // Good health. Fire lasers.
            for (int i = 5; i > 0; i--) {
                ShootLaser();
                yield return new WaitForSeconds(.1f);
            }
        }
    }

    protected void ShootLaser () {
        GameObject go;
        AbstractBullet bullet;
        go = Instantiate(laser);
        bullet = go.GetComponent<AbstractBullet>();

        if (facingLeft) {
            laserSpawn.position.Set(-Mathf.Abs(laserSpawn.position.x), laserSpawn.position.y, laserSpawn.position.z);
            bullet.direction = Vector2.left;
        } else {
            laserSpawn.position.Set(Mathf.Abs(laserSpawn.position.x), laserSpawn.position.y, laserSpawn.position.z);
            bullet.direction = Vector2.right;
        }

        bullet.setSpawnAndTarget(laserSpawn, _player.transform);

        // Put the bullet on the stage.
        bullet.transform.parent = transform.parent;
    }

    protected void ShootMissile () {
        GameObject go;
        AbstractBullet bullet;
        go = Instantiate(missile);
        bullet = go.GetComponent<AbstractBullet>();

        if (facingLeft) {
            missileSpawn.position.Set(-Mathf.Abs(missileSpawn.position.x), missileSpawn.position.y, missileSpawn.position.z);
            bullet.direction = Vector2.left;
        } else {
            missileSpawn.position.Set(Mathf.Abs(missileSpawn.position.x), missileSpawn.position.y, missileSpawn.position.z);
            bullet.direction = Vector2.right;
        }

        bullet.setTarget(_player.transform);

        // Stick the bullet in the spawner.
        bullet.transform.position = missileSpawn.position;

        // Put the bullet on the stage.
        bullet.transform.parent = transform.parent;
    }

    public override void damage (int damage, AbstractDamageCollider.DamageType type, int knockback)
	{
		base.damage (damage, type, knockback);

        if (isInvincible)
        {
            // Can't hurt this boy.
            return;
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
}
