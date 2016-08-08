using UnityEngine;
using System.Collections;

public class GameControllerBossSpiderKnight : AbstractGameController {
    public AbstractBossControl boss;
    protected bool isSpawningAds = false;
    public BossKnight knight;
    public BossSpider spider;

    protected int spawnIndex = 0;
    protected bool knightIsDead = false;

    public override void Start () {
        base.Start();
        nextLevel = 32;

        LevelBoundary.left = -1.5f;
        LevelBoundary.bottom = -5f;
        LevelBoundary.height = 5.5f;
        LevelBoundary.bottomWidth = 9f;
        LevelBoundary.topWidth = 6f;

        // Leave the spider and ads alone for now.
        enemyCount = 0;
        currentEnemyCount = 0;
        spider.setBossAction(AbstractBossControl.BossAction.stand);
    }

    void Update () {
        if (knightIsDead && currentEnemyCount < enemyCount) {
            // Spawn enemies one at a time, cycling through the index.
            int i = spawnIndex;
            spawnIndex = (spawnIndex + 1) % spawns.Length;

            // Only spawn if the spawner doesn't have the max.
            if (spawns[i].hasMissingEnemy) {
                currentEnemyCount++;
                spawns[i].spawnEnemy();
            }
        }
    }

    public void knightDead() {
        // Part one complete. Knight is down. Start up the spider.
        spider.manualSpawn();
        knightIsDead = true;
        enemyCount = 3;
    }

    public void bossDead () {
        Application.LoadLevel(nextLevel);
    }
}
