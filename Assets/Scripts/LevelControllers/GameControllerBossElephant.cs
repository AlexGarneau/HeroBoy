using UnityEngine;
using System.Collections;

public class GameControllerBossElephant : AbstractGameController
{
	public AbstractBossControl boss;
    public Transform bossSpawnCenter;
    public Transform bossSpawnRight;

    protected int spawnIndex = 0;
	protected bool isSpawningAds = false;
    protected float timePassed = 0;
    protected bool bossSpawned = false;
    protected bool dieToWin = false;

    protected float timeToAdvance = 20f;

	public override void Start ()
	{
		base.Start ();
		nextLevel = 25;
		for (var i = spawns.Length - 1; i >= 0; i--) {
			spawns [i].spawnDelay = 1;
		}

        LevelBoundary.type = LevelBoundary.TYPE_CIRCLE;

		LevelBoundary.circleCenter = new Vector2(bossSpawnCenter.transform.position.x, bossSpawnCenter.transform.position.y);
        LevelBoundary.circleRadius = 17f;

        // Always spawning ads.
        enemyCount = 6;
        isSpawningAds = true;
    }

	public override void Update ()
	{
        // Just keep spawning enemies.
        if (currentEnemyCount < enemyCount) {
            // Spawn enemies one at a time, cycling through the index.
            int i = spawnIndex;
            spawnIndex = (spawnIndex + 1) % spawns.Length;
            
            // Only spawn if the spawner doesn't have the max.
            if (spawns[i].hasMissingEnemy) {
                currentEnemyCount++;
                GameObject newEnemy = spawns[i].spawnEnemy();
                AbstractEnemyControl newControl = newEnemy.GetComponent<AbstractEnemyControl>();
                enemiesPacing.Add(newControl);
                newControl.setBaseState(Random.value >= 0.5f ? AbstractEnemyControl.EnemyStates.paceBack : AbstractEnemyControl.EnemyStates.paceForth);

                // Make enemies go crazy upon spawning if dieToWin.
                if (dieToWin) {
                    ArrayList enemies = spawns[i].getEnemies();
                    for (int j = enemies.Count - 1; j >= 0; j--) {
                        if (enemies[j] != null) {
                            Clown clown = (enemies[j] as GameObject).GetComponent<Clown>();
                            clown.setShudder(10);
                            clown.setPhaseChance(.3f);
                            clown.setEnemyDamage(10);
                            clown.setMoveSpeed(2f);
                        }
                    }
                }
            }
        }

        timePassed += Time.deltaTime;

        if (enemyCount >= 10) {
            // Boss time.
            if (!bossSpawned) {
                if (timePassed % timeToAdvance < timePassed) {
                    // Hit a 30-second mark. Spawn the boss.
                    GameObject bossObj = Instantiate(boss.gameObject);

                    // Boss is placed in GameController.
                    bossObj.transform.parent = transform;

                    // Boss has 2 spawn points. Spawn wherever it's farthest from the player.
                    if (Vector3.Distance(player.transform.position, bossSpawnCenter.position) > Vector3.Distance(player.transform.position, bossSpawnRight.position)) {
                        bossObj.transform.position = bossSpawnCenter.transform.position;
                    } else {
                        bossObj.transform.position = bossSpawnRight.transform.position;
                    }

                    timePassed = 0;
                    bossSpawned = true;
                }
            } else {
                // Boss time update.
            }
        } else if (timePassed % timeToAdvance < timePassed) {
            // Hit a 30-second mark. Add one to the adCount.
            timePassed -= timeToAdvance;
            enemyCount++;
        }

        UpdateEnemies();
    }

	public void bossDying ()
	{
        // Boss is dying. In this case, it spawns clowns until the player dies.
        enemyCount += 8;
        dieToWin = true;

        // Make all the clowns go nuts.
        ArrayList enemies;
        Clown clown;
        for (int i = spawns.Length - 1; i >= 0; i--) {
            enemies = spawns[i].getEnemies();
            for (int j = enemies.Count - 1; j >= 0; j--) {
                if (enemies[j] != null) {
                    clown = (enemies[j] as GameObject).GetComponent<Clown>();
                    clown.setShudder(10);
                    clown.setPhaseChance(.3f);
                    clown.setEnemyDamage(10);
                    clown.setMoveSpeed(2f);
                }
            }
        }
	}

    public void playerDied () {
        if (dieToWin) {
            // No, Max. You ARE the boss.
            player.playerHealth = 1000;
            bossDead();
        } else {
            // Didn't make it. Normal player death.
        }
    }

	public void bossDead ()
	{
		Application.LoadLevel (nextLevel);
	}
}
