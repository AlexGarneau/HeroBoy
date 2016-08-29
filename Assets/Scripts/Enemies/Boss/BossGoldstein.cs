using UnityEngine;
using System.Collections;

public class BossGoldstein : AbstractBossControl
{
    protected bool _useMissiles = true;

    public GameObject meleeBlast;
    public GameObject missile;
    public GameObject chestBeam;

    public Transform bulletSpawnLeft;
    public Transform bulletSpawnRight;
    public Transform bulletSpawnRocketLeft;
    public Transform bulletSpawnRocketRight;
    public Transform bulletSpawnChest;

    protected override void Start ()
	{
        base.Start();
        base._bossHealth = 500f;
		base._enemMoveSpeed = .5f;
		base.enemDamage = 30;
		base._attackRange = 3f;
		base._vertRange = 0.2f;
		base.isAlive = true;
		base.isMoving = false;

        base._specialCooldownTime = 15f;
        base._specialCooldown = _specialCooldownTime;

        _controller = gameObject.GetComponent<MovementController2D> ();
        _vel = Vector3.zero;

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
        _specialCooldown -= Time.deltaTime;
		switch (state) {
	        case BossAction.move:
                CheckToAttack();
                if (_specialCooldown <= 0) {
                    _specialCooldown = _specialCooldownTime;
                    setBossAction(BossAction.special);
                }
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
			    break;
		    case BossAction.attack:
			    _anim.SetBool ("IsMoving", false);
                _anim.SetTrigger("Attack");
                break;
            case BossAction.special:
                _anim.SetBool("IsMoving", false);
                    if (_useMissiles) {
                        StartCoroutine(FireMissiles());
                        _useMissiles = false;
                    } else {
                        StartCoroutine(FireChestLaser());
                        _useMissiles = true;
                    }
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
		    case AbstractBossControl.ANIM_ATTACK_START:
			    break;
		    case AbstractBossControl.ANIM_ATTACK_END:
			    break;
		    case AbstractBossControl.ANIM_SPECIAL_END:
                isInvincible = false;
                break;
		    case AbstractBossControl.ANIM_DEATH_END:
			    Destroy (gameObject);
			    break;
		}

        base.onAnimationState(animState);
    }

    protected IEnumerator FireChestLaser () {
        isInvincible = true;
        _anim.SetTrigger("Cannon");
        yield return new WaitForSeconds(.5f);
        Debug.Log("Start Beam");
        for (int i = 10; i > 0; i--) {
            ShootBeam();
            Debug.Log("--- Beam ");
            yield return new WaitForSeconds(.2f);
        }
        Debug.Log("--- End Beam");
        isInvincible = false;
    }

    protected IEnumerator FireMissiles () {
        _anim.SetTrigger("Rocket");
        yield return new WaitForSeconds(1f);
        ShootMissile(true);
        ShootMissile(false);
        yield return new WaitForSeconds(.2f);
        ShootMissile(true);
        ShootMissile(false);
        yield return new WaitForSeconds(.2f);
        ShootMissile(true);
        ShootMissile(false);
    }

    protected void ShootMissile (bool leftSide) {
        GameObject go;
        AbstractBullet bullet;
        go = Instantiate(missile);
        bullet = go.GetComponent<AbstractBullet>();
        bullet.direction = Vector2.left;

        // Stick the bullet in the spawner.
        if (leftSide) {
            bullet.transform.position = bulletSpawnRocketLeft.position;
        } else {
            bullet.transform.position = bulletSpawnRocketRight.position;
        }

        bullet.setTarget(_player.transform);

        // Put the bullet on the stage.
        bullet.transform.parent = transform.parent;
    }

    protected void ShootBeam () {
        GameObject go;
        AbstractBullet bullet;
        go = Instantiate(chestBeam);
        bullet = go.GetComponent<AbstractBullet>();

        // Stick the beam atop a random location.
        bullet.transform.position = new Vector3(LevelBoundary.left + (LevelBoundary.bottomWidth * Random.value), LevelBoundary.bottom + (LevelBoundary.height * Random.value));

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
	}
}
