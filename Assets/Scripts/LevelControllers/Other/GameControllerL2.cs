using UnityEngine;
using System.Collections;

public class GameControllerL2 : AbstractGameController
{
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		nextLevel = 3;
		enemyCount = 6;
		// TODO: Get references to all the SpawnZombie objects currently in the level.
	}
}
