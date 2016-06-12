using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
	PlayerControl _player;
	Animator _anim;

	// Use this for initialization
	void Start ()
	{
        GameObject[] list = GameObject.FindGameObjectsWithTag("Player");
        for (var i = list.Length-1; i >= 0; i--)
        {
            PlayerControl player = list[i].GetComponent<PlayerControl>();
            if (player != null)
            {
                _player = player;
                break;
            }
        }
		_anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		_anim.SetInteger ("Health", _player.playerHealth);
	}
}
