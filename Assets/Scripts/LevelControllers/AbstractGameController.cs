using UnityEngine;
using System.Collections;

public class AbstractGameController : MonoBehaviour
{
	public SpawnZombie[] spawns;
	public int enemyCount = 6;
	public int killCount = 0;
    public int currentEnemyCount = 0;
    public int numEnemiesAttackAtOnce = 1;
    protected ArrayList enemiesPacing;
    protected ArrayList enemiesAttacking;

	protected int nextLevel;

	protected PlayerControl player;
    protected ChargeBarScript chargeBar;

    protected float timer = 3f;

    protected Animator _anim;
    protected bool levelComplete;
    
	// Use this for initialization
	public virtual void Start ()
	{
		_anim = GetComponent<Animator> ();
		levelComplete = false;

        GameObject[] list = GameObject.FindGameObjectsWithTag("Player");
        for (var i = list.Length - 1; i >= 0; i--)
        {
            PlayerControl p = list[i].GetComponent<PlayerControl>();
            if (p != null)
            {
                player = p;
                break;
            }
        }
        chargeBar = GetComponentInChildren<ChargeBarScript> ();
		// TODO: Get references to all the SpawnZombie objects currently in the level.

		// Set the level boundaries. These form a trapezoid, which can be used to keep the enemies within.
		LevelBoundary.topWidth = 5.893934f - -3.215067f; // Use the enemies positions during game play to get these coordinates.
		LevelBoundary.bottomWidth = 8.103892f - -5.385192f; // Widths are right-corner minus left corner.
		LevelBoundary.left = -5.385192f; // Left is the left corner of the larger width (in this case, bottom)
		LevelBoundary.bottom = -3.9f; // Bottom is the lowest point in the boundary.
		LevelBoundary.height = 0.64f - -3.9f; // Height is top minus bottom.

        enemiesAttacking = new ArrayList();
	}
	
	// Update is called once per frame
	public virtual void Update ()
	{
		if (enemyCount > 0) {
			for (var i = spawns.Length - 1; i >= 0; i--) {
				if (spawns [i].hasMissingEnemy) {
					GameObject newEnemy = spawns [i].spawnEnemy ();
                    enemiesPacing.Add(newEnemy.GetComponent<AbstractEnemyControl>());
					enemyCount--;
                    currentEnemyCount++;
					if (enemyCount <= 0) {
						return;
					}
				}
			}
		} else {
			bool hasEnemy = false;
			for (var i = spawns.Length - 1; i >= 0; i--) {
				if (!spawns [i].hasNoEnemy) {
					hasEnemy = true;
				}
			}
			if (!hasEnemy) {
				_anim.SetTrigger ("Complete");
				timer -= 1 * Time.deltaTime;
				if (timer <= 0) {
					SavePlayerStats ();
					timer = 0;
					Resources.UnloadUnusedAssets ();
					Application.LoadLevel (nextLevel);
				}
			}
		}

        UpdateEnemies();
    }

    public virtual void UpdateEnemies() {
        if (enemiesAttacking.Count < numEnemiesAttackAtOnce) {
            AbstractEnemyControl enemy = getClosestPacingEnemyToPlayer();
            // Add closest enemy to the attack state.
            enemiesAttacking.Add(enemy);
            enemiesPacing.Remove(enemy);
            // TODO: Change enemy state to move.
        }
    }

    public virtual void EnemyGotStunned(AbstractEnemyControl enemy) {
        // Enemy got stunned. If he was active, return him to the pacing array.
        if (enemiesAttacking.Contains(enemy)) {
            enemiesPacing.Add(enemy);
            enemiesAttacking.Remove(enemy);
            // TODO: Change enemy state to pacing.
        }
    }

    public virtual AbstractEnemyControl getClosestPacingEnemyToPlayer () {
        object enemy = enemiesPacing[enemiesPacing.Count - 1];
        float lowestDistance = float.MaxValue;
        float testDistance;
        for (int i = enemiesPacing.Count - 1; i >= 0; i--) {
            testDistance = Vector3.Distance((enemiesPacing[i] as GameObject).transform.position, player.transform.position);
            if (testDistance < lowestDistance) {
                lowestDistance = testDistance;
                enemy = enemiesPacing[i];
            }
        }
        return (enemy as AbstractEnemyControl);
    }
	
	public virtual void enemyDied ()
	{
        // Increase charge bar.
        if (chargeBar != null) {
			chargeBar.IncreaseChargePercentage (20);
		}
		killCount++;
        currentEnemyCount--;
		
        // Clean up enemies.
        for (int i = enemiesPacing.Count - 1; i >= 0; i--) {
            if (enemiesPacing[i] == null) {
                enemiesPacing.RemoveAt(i);
            }
        }
        for (int i = enemiesAttacking.Count - 1; i >= 0; i--) {
            if (enemiesAttacking[i] == null) {
                enemiesAttacking.RemoveAt(i);
            }
        }
    }

	public void SavePlayerStats ()
	{
		if (GlobalControl.instance != null) {
		    GlobalControl.instance.playerHP = player.playerHealth;
			GlobalControl.instance.playerCP = chargeBar.chargePercentage;
		}
	}
}