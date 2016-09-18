using UnityEngine;
using System.Collections;

public class GameControllerForestFinale : AbstractGameController
{
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		enemyCount = 6;
        // TODO: Get references to all the SpawnZombie objects currently in the level.

        LevelBoundary.type = LevelBoundary.TYPE_CIRCLE;
		LevelBoundary.circleCenter = new Vector2(-11f, -9.4f); // Use the enemies positions during game play to get these coordinates.
        LevelBoundary.circleRadius = 11f;
	}

    public override void enemyDied(AbstractEnemyControl enemy)
    {
        Debug.Log("Remove Enemy: " + enemy);

        // Increase charge bar.
        if (enemiesPacing.Contains(enemy) || enemiesAttacking.Contains(enemy))
        {
            if (chargeBar != null && player.earnKills)
            {
                chargeBar.IncreaseChargePercentage(20);
            }
            killCount++;
            currentEnemyCount--;

            // Clean up enemies.
            enemiesPacing.Remove(enemy);
            enemiesAttacking.Remove(enemy);

            StartCoroutine(phaseEnemyAway(enemy));
        }
    }
}