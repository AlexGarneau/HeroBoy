using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverSpawnStairsEnter : MonoBehaviour
{
	PlayerControl _player;
	
	float timer;
	float timerReset = 5f;

	void Start ()
	{
		_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControl> ();
		timer = timerReset;
	}

	void Update ()
	{
		if (_player.playerHealth <= 0) {
			ActivateScreen ();
		}
	}

	void ActivateScreen ()
	{
		if (timer > 0) {
			timer -= 1 * Time.deltaTime;
			if (timer <= 0) {
				Application.LoadLevel (3);
				timer = 0;
			}
		}
	}
}
