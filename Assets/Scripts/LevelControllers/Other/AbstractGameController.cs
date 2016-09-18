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

    public Vector3 circleCentre;
    public float circleRadius;

    public float rectTopWidth;
    public float rectBottomWidth;
    public float rectLeft;
    public float rectBottom;
    public float rectHeight;
    
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
        LevelBoundary.topWidth = rectTopWidth; // Use the enemies positions during game play to get these coordinates.
        LevelBoundary.bottomWidth = rectBottomWidth; // Widths are right-corner minus left corner.
        LevelBoundary.left = rectLeft; // Left is the left corner of the larger width (in this case, bottom)
        LevelBoundary.bottom = rectBottom; // Bottom is the lowest point in the boundary.
        LevelBoundary.height = rectHeight; // Height is top minus bottom.

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
        if (enemiesPacing == null) { return; }

        AbstractEnemyControl enemy;
        for (int i = enemiesPacing.Count - 1; i >= 0; i--)
        {
            // Keep it clean.
            enemy = enemiesPacing[i] as AbstractEnemyControl;
            if (enemy == null || enemy.Equals(null) || enemy.ToString() == "null")
            {
                enemiesPacing.RemoveAt(i);
            }
        }
        for (int i = enemiesAttacking.Count - 1; i >= 0; i--)
        {
            // Keep it clean.
            enemy = enemiesAttacking[i] as AbstractEnemyControl;
            if (enemy == null || enemy.Equals(null) || enemy.ToString() == "null")
            {
                enemiesAttacking.RemoveAt(i);
            }
        }

        if (enemiesAttacking.Count < numEnemiesAttackAtOnce) {
            enemy = getClosestPacingEnemyToPlayer();
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
        float lowestDistance = float.MaxValue;
        float testDistance;
        AbstractEnemyControl enemy = enemiesPacing[enemiesPacing.Count - 1] as AbstractEnemyControl;
        AbstractEnemyControl enemyPace;
        for (int i = enemiesPacing.Count - 1; i >= 0; i--) {
            enemyPace = enemiesPacing[i] as AbstractEnemyControl;
            if (enemyPace == null || enemyPace.Equals(null) || enemyPace.ToString() == "null")
            {
                enemiesPacing.RemoveAt(i);
                continue;
            }

            testDistance = Vector3.Distance(enemyPace.transform.position, player.transform.position);
            if (testDistance < lowestDistance) {
                lowestDistance = testDistance;
                enemy = enemyPace;
            }
        }
        return enemy;
    }
	
	public virtual void enemyDied (AbstractEnemyControl enemy)
	{
        Debug.Log("Remove Enemy: " + enemy);

        // Only works if this is a registered enemy.
        if (enemiesPacing.Contains(enemy) || enemiesAttacking.Contains(enemy))
        {
            // Increase charge bar.
            if (chargeBar != null && player.earnKills)
            {
                chargeBar.IncreaseChargePercentage(20);
            }
            killCount++;
            currentEnemyCount--;

            // Clean up enemies.
            enemiesPacing.Remove(enemy);
            enemiesAttacking.Remove(enemy);
        }
    }

	public void SavePlayerStats ()
	{
		if (GlobalControl.instance != null) {
		    GlobalControl.instance.playerHP = player.playerHealth;
			GlobalControl.instance.playerCP = chargeBar.chargePercentage;
		}
	}

    public virtual IEnumerator phaseEnemyAway(AbstractEnemyControl illusion)
    {
        float l = 1;
        SpriteRenderer[] sprites = illusion.GetComponentsInChildren<SpriteRenderer>();
        SpriteRenderer sprite;
        for (float i = 0; i < l; i += .03f)
        {
            for (int j = sprites.Length - 1; j >= 0; j--)
            {
                sprite = sprites[j];
                if (sprite == null) { continue; }
                sprite.transform.Translate(Random.Range(-i, i), Random.Range(-i, i), 0);
                sprite.GetComponent<Renderer>().material.color = new Color(0, 0, 0, l - i);
            }
            yield return new WaitForSeconds(.01f);
        }
        Destroy(illusion.gameObject);
    }
}