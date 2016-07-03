using UnityEngine;
using System.Collections;

public class GameControllerBossSpiderKnight : AbstractGameController {
    public AbstractBossControl boss;
    protected bool isSpawningAds = false;
    public BossKnight knight;
    public BossSpider spider;

    protected bool knightIsDead = false;

    public override void Start () {
        base.Start();
        nextLevel = 32;

        LevelBoundary.left = -7f;
        LevelBoundary.bottom = -5f;
        LevelBoundary.height = 10f;
        LevelBoundary.bottomWidth = LevelBoundary.topWidth = 14f;

        // Leave the spider and ads alone for now.
        enemyCount = 0;
        currentEnemyCount = 0;
        spider.setBossAction(AbstractBossControl.BossAction.stand);
    }

    void Update () {
        if (knightIsDead && enemyCount > 0) {
            // Spawn an enemy at a random spawn point.
            SpawnZombie spawn = spawns[Random.Range(0, spawns.Length)];
            enemyCount--;
            currentEnemyCount++;
            spawn.spawnEnemy();
        }
    }

    public void knightDead() {
        // Part one complete. Knight is down. Start up the spider.
        spider.setBossAction(AbstractBossControl.BossAction.move);
        knightIsDead = true;
        enemyCount = 3;
    }

    public void bossDead () {
        Application.LoadLevel(nextLevel);
    }
}
