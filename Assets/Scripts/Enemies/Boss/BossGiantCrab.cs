using UnityEngine;
using System.Collections;

public class BossGiantCrab : AbstractBossControl {
    public static int BOSS_STATE_EASY = 0;
    public static int BOSS_STATE_HARD = 1;
    private static float VOMIT_COOLDOWN = 8f;
    private static float VOMIT_IN_BETWEEN_TIME = 1f;

    public Collider2D lightHit;

    public GameObject bodyPirate;
    public GameObject pirateBomb;
    public GameObject vomitBullet;

    protected BossPirateBombsOnly bossPirate;

    private int vomitCount = 0;
    private bool highGround = false;
    private bool isVulnerable = false;

    public Animator crabHealthBar;

    protected override void Start () {
        base.Start();

        base._bossMaxHealth = base._bossHealth = 60f;
        base._enemMoveSpeed = 1.2f;
        base._enemDamage = 2f;
        base._attackRange = 2f;
        base._vertRange = 0.2f;
        base.isAlive = true;
        base.isMoving = false;
        base._player = GameObject.FindGameObjectWithTag("Player");
        base.bulletSpawn = transform.Find("BulletSpawn");

        bossState = BOSS_STATE_EASY;

        _anim = GetComponent<Animator>();
        BossAbstractBehaviour[] eabs = _anim.GetBehaviours<BossAbstractBehaviour>();
        for (var i = eabs.Length - 1; i >= 0; i--) {
            eabs[i].boss = this;
        }

        meleeCooldown = VOMIT_COOLDOWN;
        facingLeft = true;

        bossPirate = GetComponent<BossPirateBombsOnly>();
    }

    protected override void Update () {
        switch (state) {
            case BossAction.stand:
                if (transform.position.x - _player.transform.position.x < 5) {
                    // Player's too close. Swipe that bad boy.
                    setBossAction(BossAction.attack);
                } else {
                    if (meleeCooldown > 0) {
                        meleeCooldown -= Time.deltaTime;
                        if (meleeCooldown < 0) {
                            setBossAction(BossAction.special);
                        }
                    }
                }
                break;
        }

        //_anim.SetFloat("Health", _bossHealth);

        base.Update();
    }

    public override void setBossAction (BossAction newState) {
        if (state == newState) {
            // Same state. Do nothing.
            return;
        }

        switch (newState) {
            case BossAction.stand:
                isVulnerable = false;
                break;
            case BossAction.attack:
                _anim.SetTrigger("Attack");
                break;
            case BossAction.special:
                _anim.SetTrigger("Vomit");
                break;
            case BossAction.dead:
                break;
        }

        base.setBossAction(newState);
    }

    /** States called by the animator. */
    public override void onAnimationState (string state) {
        Debug.Log("AnimataionState:" + state);

        switch (state) {
            case AbstractEnemyControl.ANIM_SPAWN_END:
                // Boss spawned.
                Debug.Log("Spawn Complete");
                setBossAction(BossAction.stand);
                break;
            case AbstractBossControl.ANIM_ATTACK_END:
                // Melee attack complete.
                setBossAction(BossAction.stand);
                break;
            case AbstractBossControl.ANIM_SPECIAL_START:
                // Vomit attack start. Make vulnerable.
                isVulnerable = true;
                StartCoroutine(VomitRapid());
                break;
            case AbstractBossControl.ANIM_SPECIAL_END:
                // Vomit attack complete.
                isVulnerable = false;
                meleeCooldown = VOMIT_COOLDOWN;
                setBossAction(BossAction.stand);
                break;
            case AbstractBossControl.ANIM_DEATH_END:
                SendMessageUpwards("bossDead", SendMessageOptions.DontRequireReceiver);
                Destroy(gameObject);
                break;
            case AbstractBossControl.ANIM_RETREAT_END:
                // Call land.
                setBossAction(BossAction.reappear);
                break;
            case AbstractBossControl.ANIM_STUN_START:
                setBossAction(BossAction.retreat);
                break;
        }
    }

    protected IEnumerator VomitRapid() {
        vomit();
        yield return new WaitForSeconds(VOMIT_IN_BETWEEN_TIME);
        vomit();
        yield return new WaitForSeconds(VOMIT_IN_BETWEEN_TIME);
        vomit();
    }

    protected void vomit () {
        GameObject go;
        VomitBullet bullet;

        go = Instantiate(vomitBullet);
        bullet = go.GetComponent<VomitBullet>();
        bulletSpawn.position.Set(-Mathf.Abs(bulletSpawn.position.x), bulletSpawn.position.y, bulletSpawn.position.z);
        bullet.direction = Vector2.left;

        // Stick the bullet in the spawner.
        bullet.transform.position = bulletSpawn.position;

        // Put the bullet on the stage.
        bullet.transform.parent = transform.parent;
    }

    public override void damage (int damage, AbstractDamageCollider.DamageType type, int knockback) {
        if (!isVulnerable) {
            // Can't damage him any other time than move and attack.
            return;
        }

        _bossHealth -= damage;
        if (_bossHealth <= 0) {
            // Boos is dead. Or is it?
            SendMessageUpwards("bossDead", SendMessageOptions.DontRequireReceiver);
        } else {
            // Boss is hit. Stun that noise!
            stun(5);
        }
    }

    public override void stun (float timeInSec) {
        base.stun(timeInSec);
        isVulnerable = false;
        _anim.SetTrigger("IsHit");
        crabHealthBar.SetTrigger("PirateIsHit");
        if (bossPirate != null) {
            bossPirate.stun(timeInSec);
        }
    }
}
