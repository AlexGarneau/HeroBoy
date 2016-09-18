using UnityEngine;
using System.Collections;

public class TutorialTetherball : MonoBehaviour {

    Animator _anim;

    public enum DamageType
    {
        light,
        medium,
        heavy
    };
    public DamageType type;

	// Use this for initialization
	void Start () {
        _anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D (Collider2D col)
    {
        PlayerDamageCollider pc = col.GetComponent<PlayerDamageCollider>();
        if (pc != null)
        {
            Debug.Log("What hit me? " + pc.type);
            if (pc.type == PlayerDamageCollider.DamageType.light)
            {
                _anim.SetTrigger("IsHit");
            } else if (pc.type == PlayerDamageCollider.DamageType.medium)
            {
                _anim.SetTrigger("IsHit");
            }
            else if (pc.type == PlayerDamageCollider.DamageType.heavy)
            {
                _anim.SetTrigger("IsHit");
            }
        }
    }
}
