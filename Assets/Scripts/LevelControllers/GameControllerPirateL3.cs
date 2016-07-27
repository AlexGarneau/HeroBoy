using UnityEngine;
using System.Collections;

public class GameControllerPirateL3 : AbstractGameController
{
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		enemyCount = 6;
		// TODO: Get references to all the SpawnZombie objects currently in the level.

		LevelBoundary.topWidth = 8f - -8f; // Use the enemies positions during game play to get these coordinates.
		LevelBoundary.bottomWidth = 8f - -8f; // Widths are right-corner minus left corner.
		LevelBoundary.left = -8f; // Left is the left corner of the larger width (in this case, bottom)
		LevelBoundary.bottom = -4f; // Bottom is the lowest point in the boundary.
		LevelBoundary.height = 0.7f - -3.7f; // Height is top minus bottom.
	}
}