using UnityEngine;
using System.Collections;

public class GameControllerL1 : AbstractGameController
{
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		nextLevel = 2;
		enemyCount = 6;
		// TODO: Get references to all the SpawnZombie objects currently in the level.


	}
}