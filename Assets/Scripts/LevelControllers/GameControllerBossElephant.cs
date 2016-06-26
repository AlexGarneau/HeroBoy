using UnityEngine;
using System.Collections;

public class GameControllerBossElephant : AbstractGameController
{
	public AbstractBossControl boss;
	protected bool isSpawningAds = false;

	public override void Start ()
	{
		base.Start ();
		nextLevel = 25;
		for (var i = spawns.Length - 1; i >= 0; i--) {
			spawns [i].spawnDelay = 2;
		}

		LevelBoundary.left = -13.91f;
		LevelBoundary.bottom = -2.1f;
		LevelBoundary.height = 6f;
		LevelBoundary.bottomWidth = LevelBoundary.topWidth = 13f;
	}

	void Update ()
	{
        base.Update();
	}

	public void spawnAds (int adCount)
	{
		// Boss requested the spawning of the ads. Comply.
		isSpawningAds = true;
		enemyCount = adCount;
	}

	public void bossDying ()
	{
		// Boss is dying. Long drawn-out animation or whatever. Clear out all the ads and hazards so the player can't get hit during his moment of victory.
	}

	public void bossDead ()
	{
		Application.LoadLevel (nextLevel);
	}
}
