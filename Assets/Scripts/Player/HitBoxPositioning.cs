using UnityEngine;
using System.Collections;

public class HitBoxPositioning : MonoBehaviour
{
	Vector2 pos1;
	Vector2 pos2;

	PlayerControl _player;

	// Use this for initialization
	void Start ()
	{
		_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControl> ();
		pos1 = new Vector2 (0.8f, 0);
		pos2 = new Vector2 (-0.8f, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_player.facingLeft == true) {
			transform.localPosition = pos2;
		} else if (_player.facingRight == true) {
			transform.localPosition = pos1;
		}
	}
}
