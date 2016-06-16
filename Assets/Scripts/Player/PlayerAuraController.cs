using UnityEngine;
using System.Collections;

public class PlayerAuraController : MonoBehaviour {

    PlayerControl _player;
    Animator _anim;

	// Use this for initialization
	void Start () {
        _player = GetComponentInParent<PlayerControl>();
        _anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        _anim.SetBool("Red", _player.tempInvuln);
        _anim.SetInteger("PlayerHealth", _player.playerHealth);
        //_anim.SetBool("White", );
    }
}
