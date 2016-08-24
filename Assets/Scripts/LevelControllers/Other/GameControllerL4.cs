using UnityEngine;
using System.Collections;

public class GameControllerL4 : AbstractGameController
{
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		nextLevel = 0;
		enemyCount = 12;
		// TODO: Get references to all the SpawnZombie objects currently in the level.
	}
}
