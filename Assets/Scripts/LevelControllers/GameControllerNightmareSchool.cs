using UnityEngine;
using System.Collections;

public class GameControllerNightmareSchool: AbstractGameController
{
	PlayerControl _player;
	
	float timerActivate;
	float timerReset = 5f;
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControl> ();
		timerActivate = timerReset;

		LevelBoundary.topWidth = 100000;
		LevelBoundary.bottomWidth = 100000;
		LevelBoundary.left = -50000;
		LevelBoundary.bottom = -50000;
		LevelBoundary.height = 100000;
	}

	void Update ()
	{
		if (_player.playerHealth <= 0) {
			ActivateScreen ();
		}
	}

	void ActivateScreen ()
	{
		if (timerActivate > 0) {
			timerActivate -= 1 * Time.deltaTime;
			if (timerActivate <= 0) {
				Application.LoadLevel (27);
				timerActivate = 0;
			}
		}
	}
}