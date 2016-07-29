using UnityEngine;
using System.Collections;

public class AbstractGameController : MonoBehaviour
{
	public SpawnZombie[] spawns;
	public int enemyCount = 6;
	public int killCount = 0;
    public int numEnemiesAttackAtOnce = 1;
    protected int currentEnemyCount = 0;
    protected ArrayList enemiesPacing;
    protected ArrayList enemiesAttacking;

	public int nextLevel;

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

        enemiesPacing = new ArrayList();
        enemiesAttacking = new ArrayList();
	}
	
	// Update is called once per frame
	public virtual void Update ()
	{
		if (enemyCount > 0) {
			for (var i = spawns.Length - 1; i >= 0; i--) {
				if (spawns [i].hasMissingEnemy) {
					GameObject newEnemy = spawns [i].spawnEnemy ();
                    AbstractEnemyControl newControl = newEnemy.GetComponent<AbstractEnemyControl>();
                    enemiesPacing.Add(newControl);
                    newControl.setBaseState(Random.value >= 0.5f ? AbstractEnemyControl.EnemyStates.paceBack : AbstractEnemyControl.EnemyStates.paceForth);
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
        for (int i = enemiesPacing.Count - 1; i >= 0; i--)
        {
            // Keep it clean.
            if (enemiesPacing[i] == null)
            {
                enemiesPacing.RemoveAt(i);
            }
        }
        for (int i = enemiesAttacking.Count - 1; i >= 0; i--)
        {
            // Keep it clean.
            if (enemiesAttacking[i] == null)
            {
                enemiesAttacking.RemoveAt(i);
            }
        }

        if (enemiesAttacking.Count < numEnemiesAttackAtOnce) {
            AbstractEnemyControl enemy = getClosestPacingEnemyToPlayer();
            if (enemy == null) {
                // Never mind. No more enemies.
                return;
            }
            // Add closest enemy to the attack state.
            enemiesAttacking.Add(enemy);
            enemiesPacing.Remove(enemy);
            enemy.setEnemyState(AbstractEnemyControl.EnemyStates.move);
            enemy.setBaseState(AbstractEnemyControl.EnemyStates.move);
        }
    }

    public virtual void EnemyGotStunned(AbstractEnemyControl enemy) {
        // Enemy got stunned. If he was active, return him to the pacing array. If he's still the closest, he'll get converted back in a heartbeat.
        if (enemiesAttacking.Contains(enemy)) {
            enemiesPacing.Add(enemy);
            enemiesAttacking.Remove(enemy);
            AbstractEnemyControl.EnemyStates state = Random.value >= 0.5f ? AbstractEnemyControl.EnemyStates.paceBack : AbstractEnemyControl.EnemyStates.paceForth;
            enemy.setEnemyState(state);
            enemy.setBaseState(state);
        }
    }

    public virtual AbstractEnemyControl getClosestPacingEnemyToPlayer () {
        if (enemiesPacing.Count <= 0) {
            // No enemies!
            return null;
        }
        object enemy = enemiesPacing[enemiesPacing.Count - 1];
        float lowestDistance = float.MaxValue;
        float testDistance;
        for (int i = enemiesPacing.Count - 1; i >= 0; i--) {
            testDistance = Vector3.Distance((enemiesPacing[i] as AbstractEnemyControl).transform.position, player.transform.position);
            if (testDistance < lowestDistance) {
                lowestDistance = testDistance;
                enemy = enemiesPacing[i];
            }
        }
        return (enemy as AbstractEnemyControl);
    }
	
	public virtual void enemyDied (AbstractEnemyControl enemy)
	{
        Debug.Log("Remove Enemy: " + enemy);

        // Increase charge bar.
        if (chargeBar != null && player.earnKills) {
			chargeBar.IncreaseChargePercentage (20);
		}
		killCount++;
        currentEnemyCount--;
		
        // Clean up enemies.
        enemiesPacing.Remove(enemy);
        enemiesAttacking.Remove(enemy);
    }

	public void SavePlayerStats ()
	{
		if (GlobalControl.instance != null) {
		    GlobalControl.instance.playerHP = player.playerHealth;
			GlobalControl.instance.playerCP = chargeBar.chargePercentage;
		}
	}
}