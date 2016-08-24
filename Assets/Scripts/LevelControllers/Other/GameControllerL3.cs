using UnityEngine;
using System.Collections;

public class GameControllerL3 : AbstractGameController
{
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		nextLevel = 4;
		enemyCount = 9;
		// TODO: Get references to all the SpawnZombie objects currently in the level.
	}
}
