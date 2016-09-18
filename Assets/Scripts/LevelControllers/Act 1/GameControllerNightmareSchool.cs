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

        LevelBoundary.bottom = -10000;
        LevelBoundary.bottomWidth = 20000;
        LevelBoundary.left = -10000;
        LevelBoundary.topWidth = 20000;
        LevelBoundary.height = 10000;
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
				Application.LoadLevel (29);
				timerActivate = 0;
			}
		}
	}
}