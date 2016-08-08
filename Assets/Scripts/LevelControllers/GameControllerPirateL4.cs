using UnityEngine;
using System.Collections;

public class GameControllerPirateL4 : AbstractGameController
{
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		enemyCount = 8;
		// TODO: Get references to all the SpawnZombie objects currently in the level.

		LevelBoundary.topWidth = 7.35f - -5.4f; // Use the enemies positions during game play to get these coordinates.
		LevelBoundary.bottomWidth = 8.25f - -7.65f; // Widths are right-corner minus left corner.
		LevelBoundary.left = -8f; // Left is the left corner of the larger width (in this case, bottom)
		LevelBoundary.bottom = -6f; // Bottom is the lowest point in the boundary.
		LevelBoundary.height = -0.8f - -6f; // Height is top minus bottom.
	}
}