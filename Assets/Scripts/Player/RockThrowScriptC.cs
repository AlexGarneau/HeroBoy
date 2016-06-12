using UnityEngine;
using System.Collections;

public class RockThrowScriptC : MonoBehaviour
{
    PlayerControl _player;
	Animator _anim;
	float speed = 6f;
	float _timer = 3f;
	bool facingLeft;
	Vector2 rockDirection;

	void Start ()
	{
        GameObject[] list = GameObject.FindGameObjectsWithTag("Player");
        for (var i = list.Length - 1; i >= 0; i--)
        {
            PlayerControl player = list[i].GetComponent<PlayerControl>();
            if (player != null)
            {
                _player = player;
                break;
            }
        }
        _anim = GetComponent<Animator> ();
		if (_player.throwRight) {
			facingLeft = false;
			rockDirection = new Vector2 (1, 0);
		} else {
			facingLeft = true;
			rockDirection = new Vector2 (-1, 0);
		}
		_anim.SetBool ("FacingLeft", facingLeft);
	}

	
	// Update is called once per frame
	void Update ()
	{
		transform.Translate (rockDirection * speed * Time.deltaTime);
		_timer -= Time.deltaTime;
		if (_timer < 0) {
			Destroy (this.gameObject);
		}
	}
}
