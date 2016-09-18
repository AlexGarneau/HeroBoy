using UnityEngine;
using System.Collections;

public class GameControllerArena: AbstractGameController
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

        LevelBoundary.type = LevelBoundary.TYPE_CIRCLE;
        LevelBoundary.circleRadius = 11;
    }

	void Update ()
    {
        base.Update();
        if (_player.playerHealth <= 0) {
			ActivateScreen ();
		}
	}

	void ActivateScreen ()
	{
		if (timerActivate > 0) {
			timerActivate -= 1 * Time.deltaTime;
			if (timerActivate <= 0) {
				Application.LoadLevel (2);
				timerActivate = 0;
			}
		}
	}
}