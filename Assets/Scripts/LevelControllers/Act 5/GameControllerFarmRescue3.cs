using UnityEngine;
using System.Collections;

public class GameControllerFarmRescue3: AbstractGameController
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

        LevelBoundary.type = LevelBoundary.TYPE_RECTANGLE;
        LevelBoundary.bottom = -3.4f;
        LevelBoundary.height = 4;
        LevelBoundary.left = -5000;
        LevelBoundary.topWidth = 10000;
        LevelBoundary.bottomWidth = 10000;
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