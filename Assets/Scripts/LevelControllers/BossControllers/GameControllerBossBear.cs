using UnityEngine;
using System.Collections;

public class GameControllerBossBear : AbstractGameController
{
	public AbstractBossControl boss;
	protected bool isSpawningAds = false;
    public BossBear bear;

	public override void Start ()
	{
		base.Start ();
		nextLevel = 25;
		for (var i = spawns.Length - 1; i >= 0; i--) {
			spawns [i].spawnDelay = 2;
		}
	}

    void Update()
    {
        base.Update();
	}

	public void bossDead ()
	{
        Application.LoadLevel(nextLevel);
	}
}
