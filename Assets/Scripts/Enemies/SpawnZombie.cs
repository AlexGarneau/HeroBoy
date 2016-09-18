using UnityEngine;
using System.Collections;

public class SpawnZombie : AbstractClass
{
	public GameObject[] prefabs;
	public bool autoSpawn = false;
	public int zedCount = 1;
	public float spawnDelay = 3;

	private GameObject[] _enemies;
	private bool[] _isSpawning;
	private bool isCurrentlySpawning = false;
	public bool hasMissingEnemy {
		get {
			if (_enemies == null) {
				return false;
			}
			for (int i = _enemies.Length - 1; i >= 0; i--) {
				if (_checkIfNull(_enemies[i]) && !_isSpawning [i]) {
                    return true;
				}
			}
			return false;
		}
	}
	public bool hasNoEnemy {
		get {
            if (_enemies == null)
            {
                return true;
            }
            for (int i = _enemies.Length - 1; i >= 0; i--) {
				if (!_checkIfNull(_enemies[i]) || _isSpawning [i]) {
					return false;
				}
			}
			return true;
		}
	}

	void Start ()
	{
		_enemies = new GameObject[zedCount];
		_isSpawning = new bool[zedCount];

		// Spawn enemies up to zedCount.
		for (int i = zedCount - 1; i >= 0; i--) {
			_enemies[i] = null;
		}
	}

	void setZedCount (int count)
	{
		zedCount = count;
		_enemies = new GameObject[zedCount];
		_isSpawning = new bool[zedCount];
        
		// Spawn enemies up to zedCount.
		for (int i = zedCount - 1; i >= 0; i--) {
			_enemies[i] = null;
		}
	}

	void Update ()
	{
        // Used for unlimited automatic zombie spawning.
        if (autoSpawn && !isCurrentlySpawning) {
			spawnEnemy ();
		}
	}

    /** Spawns a single enemy. */
    public GameObject spawnEnemy()
    {
        GameObject enemy;
        for (int i = zedCount - 1; i >= 0; i--) {
            enemy = _enemies[i];
            if (_checkIfNull(_enemies[i]) && !_isSpawning [i]) {
                // Enemy died. Spawn one new enemy. Make multiple calls to fill the spawn.
                GameObject newEnemy = GameObject.Instantiate(prefabs[Random.Range(0, prefabs.Length)], transform.position, transform.rotation) as GameObject;
                _enemies[i] = newEnemy;
                StartCoroutine(createEnemy(i, newEnemy));
                return newEnemy;
            }
		}

        // Enemy didn't spawn. Meh.
        return null;
	}

    public GameObject[] getEnemies () {
        return _enemies;
    }

	private IEnumerator createEnemy (int index, GameObject newEnemy)
	{
		isCurrentlySpawning = true;
		_isSpawning [index] = true;
		yield return new WaitForSeconds (spawnDelay);

        if (newEnemy != null) {
            newEnemy.transform.parent = transform.parent.parent;
        } else {
            _enemies[index] = null;
        }
		_isSpawning [index] = false;
		isCurrentlySpawning = false;
	}

    private bool _checkIfNull(GameObject obj)
    {
        return object.Equals(obj, null) || obj == null || obj.ToString() == "null";
    }
}
