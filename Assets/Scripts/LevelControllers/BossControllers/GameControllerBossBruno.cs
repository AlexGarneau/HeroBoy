using UnityEngine;
using System.Collections;

public class GameControllerBossBruno : AbstractGameController {
    public AbstractEnemyControl boss;
    protected bool isSpawningAds = false;
    public BossBruno bruno;

    public override void Start () {
        base.Start();
        nextLevel = 32;

        LevelBoundary.left = -7f;
        LevelBoundary.bottom = -2.1f;
        LevelBoundary.height = 6f;
        LevelBoundary.bottomWidth = LevelBoundary.topWidth = 14f;
    }

    void Update () {
        base.Update();
    }

    public void bossDead () {
        Application.LoadLevel(nextLevel);
    }
}
