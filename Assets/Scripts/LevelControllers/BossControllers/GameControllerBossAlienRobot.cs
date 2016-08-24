using UnityEngine;
using System.Collections;

public class GameControllerBossAlienRobot : AbstractGameController
{
	public AbstractBossControl boss;
	protected bool isSpawningAds = true;

	public override void Start ()
	{
		base.Start ();
		nextLevel = 25;
		for (var i = spawns.Length - 1; i >= 0; i--) {
			spawns [i].spawnDelay = 2;
		}

        LevelBoundary.type = LevelBoundary.TYPE_RECTANGLE;
		LevelBoundary.left = -5.68f;
		LevelBoundary.bottom = -4.15f;
		LevelBoundary.height = 4.65f;
		LevelBoundary.bottomWidth = 15f;
        LevelBoundary.topWidth = 10f;
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
