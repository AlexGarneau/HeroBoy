using UnityEngine;
using System.Collections;

public class Illusion : AbstractEnemyControl
{
    public Collider2D lightHit;

    public GameObject formBandit;
    public GameObject formClownClaw;
    public GameObject formClownHammer;
    public GameObject formGhoul;
    public GameObject formGoblin;
    public GameObject formPirate;
    public GameObject formRARClaw;
    public GameObject formSkeleton;
    public GameObject formZombie;

    public GameObject bulletWater;
    public GameObject bulletPirate;
    public GameObject bulletDart;

    protected GameObject[] forms;
    protected GameObject form;
    protected int formIndex;
    protected GameObject bullet;

    protected int _prevPlayerHealth = 100;
    protected float shiftTimer = 4000;
    protected float shiftTimerMax = 4000;

    protected override void Start ()
	{
        base.Start();
        base._enemHealth = 50f;
		base._enemMoveSpeed = 2.5f;
		base.enemDamage = 10;
		base._attackRange = 1.6f;
		base._vertRange = 0.2f;
		base.isAlive = true;
		base.isMoving = false;
        base.hasGun = false;

        forms = new GameObject[] {
            formBandit, formClownClaw, formClownHammer, formGhoul, formGoblin, formPirate, formRARClaw, formSkeleton, formZombie
        };

        _controller = gameObject.GetComponent<MovementController2D> ();

        _damageColliders = gameObject.GetComponentsInChildren<EnemyDamageCollider> (true);
		if (_damageColliders != null && _damageColliders.Length > 0) {
			// Sets the damage of damage colliders. TODO: Independent damage set to different colliders, if multiple exist.
			for (int i = _damageColliders.Length - 1; i >= 0; i--) {
				_damageColliders [i].gameObject.SetActive (false);
			}
		}

        setForm(Random.Range(0, forms.Length));
    }

	protected override void Update()
	{
		switch (state) {
		case EnemyStates.dead:
            
            break;
		}

        if (shiftTimer > 0) {
            shiftTimer -= Time.deltaTime;
            if (shiftTimer <= 0) {
                shiftTimer = shiftTimerMax;
                changeForm();
            }
        }

        int playerHealth = _playerControl.playerHealth;
        if (playerHealth != _prevPlayerHealth) {
            if (_prevPlayerHealth >= 80 && playerHealth < 80) {
                shiftTimerMax = 60;
                shiftTimer = shiftTimerMax;
            } else if (_prevPlayerHealth >= 60 && playerHealth < 60) {
                shiftTimerMax = 30;
                shiftTimer = shiftTimerMax;
            } else if(_prevPlayerHealth >= 40 && playerHealth < 40) {
                shiftTimerMax = 15;
                shiftTimer = shiftTimerMax;
            } else if (_prevPlayerHealth >= 20 && playerHealth < 20) {
                shiftTimerMax = 5;
                shiftTimer = shiftTimerMax;
            }
            _prevPlayerHealth = playerHealth;
        }

		base.Update ();
	}

	protected override void setState (EnemyStates newState)
	{
        if (state == EnemyStates.dead)
        {
            // What? I'm fleeing for my life! Shut up!
            return;
        }

		switch (newState) {
            case EnemyStates.move:
                _anim.SetBool ("IsMoving", true);
                break;
            case EnemyStates.attack:
                _anim.SetBool ("IsMoving", false);
                if (form == formPirate) {
                    _anim.SetTrigger(Random.value > 0.5 ? "Attack1" : "Attack2");
                } else {
                    _anim.SetTrigger("Attack");
                }
                break;
            case EnemyStates.shoot:
                _anim.SetTrigger("Fire");
                break;
            case EnemyStates.dead:
                _anim.SetBool ("IsMoving", false);
                GetComponent<BoxCollider2D>().enabled = false;
                break;
		}

		base.setState (newState);
	}
    
    public override void onAnimationState (string animState)
	{
		switch (animState) {
		    case AbstractEnemyControl.ANIM_ATTACK_END:
			    setState (baseState);
			    break;
		    case AbstractEnemyControl.ANIM_INJURED_END:
			    setState (baseState);
			    break;
            case AbstractEnemyControl.ANIM_SHOOT_START:
                Shoot();
                break;
            case AbstractEnemyControl.ANIM_DEATH_START:
                setState(EnemyStates.dead);
                SendMessageUpwards ("enemyDied", this, SendMessageOptions.DontRequireReceiver);
                break;
            case AbstractEnemyControl.ANIM_DEATH_END:
			    break;
		}

        base.onAnimationState(animState);
	}

	public override void damage (int damage, AbstractDamageCollider.DamageType type, int knockback)
	{
		base.damage (damage, type, knockback);

        if (invincible)
        {
            // Can't hurt this boy.
            return;
        }

		if (facingLeft == true) {
			xForce = damage * .005f;
		} else {
			xForce = -damage * .005f;
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

    protected override void Shoot () {
        GameObject go;
        AbstractBullet ab;
        if (facingLeft) {
            go = Instantiate(bullet);
            ab = go.GetComponent<AbstractBullet>();
            bulletSpawn.position.Set(-Mathf.Abs(bulletSpawn.position.x), bulletSpawn.position.y, bulletSpawn.position.z);
            ab.direction = Vector2.left;
        } else {
            go = Instantiate(bullet);
            ab = go.GetComponent<AbstractBullet>();
            bulletSpawn.position.Set(Mathf.Abs(bulletSpawn.position.x), bulletSpawn.position.y, bulletSpawn.position.z);
            ab.direction = Vector2.right;
            bullet.transform.localScale = new Vector3(-1, 1, 1);
        }

        // Stick the bullet in the spawner.
        ab.transform.position = bulletSpawn.position;

        // Put the bullet on the stage.
        ab.transform.parent = transform.parent;
    }

    void changeForm () {
        for (int i = forms.Length - 1; i >= 0; i--) {
            forms[i].SetActive(false);
        }

        StartCoroutine(shuffleForm());
    }

    IEnumerator shuffleForm() {
        for (int i = 20; i >= 0; i--) {
            int tempIndex = Random.Range(0, forms.Length);
            forms[tempIndex].SetActive(true);
            _anim = form.GetComponent<Animator>();
            _anim.SetBool("IsMoving", false);
            _anim.SetBool("FacingLeft", facingLeft);

            yield return new WaitForSeconds(0.05f);
            forms[tempIndex].SetActive(false);
        }
        setForm(Random.Range(0, forms.Length));
    }

    void setForm (int index) {
        formIndex = index;
        form = forms[index];
        form.SetActive(true);

        hasAttack = true;
        hasGun = false;
        if (form == formClownClaw || form == formClownHammer) {
            bullet = bulletWater;
            hasGun = true;
            bulletSpawn = form.transform.Find("BulletSpawn");
        } else if (form == formGoblin) {
            bullet = bulletDart;
            hasGun = true;
            hasAttack = false;
            bulletSpawn = form.transform.Find("BulletSpawn");
        } else if (form == formPirate) {
            hasGun = true;
            bullet = bulletPirate;
            bulletSpawn = form.transform.Find("BulletSpawn");
        }

        // Set the animator.
        _anim = form.GetComponent<Animator>();
        EnemyAbstractBehaviour[] eabs = _anim.GetBehaviours<EnemyAbstractBehaviour>();
        for (var i = eabs.Length - 1; i >= 0; i--) {
            eabs[i].enemy = this;
        }
    }
}
