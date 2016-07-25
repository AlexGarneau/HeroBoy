using UnityEngine;
using System.Collections;

public class BalloonDog : AbstractEnemyControl {
    public Collider2D lightHit;
    public GameObject healItem;

    protected bool highGround;

    protected override void Start () {
        base.Start();
        base._enemHealth = 1f;
        base._enemMoveSpeed = 1.5f;
        base.enemDamage = 2;
        base._attackRange = 1f;
        base._vertRange = 0.2f;
        base.isAlive = true;
        base.isMoving = false;

        _controller = GetComponent<MovementController2D>();

        _damageColliders = GetComponentsInChildren<EnemyDamageCollider>();
        if (_damageColliders != null && _damageColliders.Length > 0) {
            // Sets the damage of damage colliders. TODO: Independent damage set to different colliders, if multple exist.
            for (int i = _damageColliders.Length - 1; i >= 0; i--) {
                _damageColliders[i].gameObject.SetActive(false);
            }
        }

        EnemyAbstractBehaviour[] eabs = _anim.GetBehaviours<EnemyAbstractBehaviour>();
        for (var i = eabs.Length - 1; i >= 0; i--) {
            eabs[i].enemy = this;
        }

        // Set the first state.
        setState(EnemyStates.spawn);
    }

    public void setDirection(bool left) {
        facingLeft = left;
        _anim.SetBool("FacingLeft", left);
    }

    protected override void Update () {
        switch (state) {
            case EnemyStates.move:
                break;
            case EnemyStates.attack:
                break;
            case EnemyStates.dead:
                //DeathTimerDestroy ();
                break;
        }

        _anim.SetFloat("Health", _enemHealth);
        _anim.SetBool("FacingLeft", facingLeft);
        _anim.SetInteger("PlayerHealth", _playerControl.playerHealth);

        base.Update();
    }

    protected override void setState (EnemyStates newState) {
        switch (newState) {
            case EnemyStates.move:
                _anim.SetBool("IsMoving", true);
                break;
            case EnemyStates.attack:
                _anim.SetBool("IsMoving", false);
                _anim.SetTrigger("Attack");
                break;
            case EnemyStates.dead:
                _anim.SetBool("IsMoving", false);
                GetComponent<BoxCollider2D>().enabled = false;
                break;
        }

        base.setState(newState);
    }

    public override void onAnimationState (string animState) {
        switch (animState) {
            case AbstractEnemyControl.ANIM_SPAWN_END:
                setState(EnemyStates.move);
                break;
            case AbstractEnemyControl.ANIM_ATTACK_END:
                setState(EnemyStates.move);
                break;
            case AbstractEnemyControl.ANIM_DEATH_END:
                Destroy(gameObject);
                break;
        }
    }

    public override void damage (int damage, AbstractDamageCollider.DamageType type, int knockback) {
        base.damage(damage, type, knockback);
    }
}
