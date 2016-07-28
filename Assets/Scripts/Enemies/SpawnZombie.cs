﻿using UnityEngine;
using System.Collections;

public class SpawnZombie : AbstractClass
{
	public GameObject[] prefabs;
	public bool autoSpawn = false;
	public int zedCount = 1;
	public float spawnDelay = 3;

	private ArrayList _enemies;
	private bool[] _isSpawning;
	private bool isCurrentlySpawning = false;
	public bool hasMissingEnemy {
		get {
			if (_enemies == null) {
				return false;
			}
			for (int i = _enemies.Count - 1; i >= 0; i--) {
				if (_enemies [i].Equals (null) && !_isSpawning [i]) {
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
            for (int i = _enemies.Count - 1; i >= 0; i--) {
				if (!_enemies [i].Equals (null) || _isSpawning [i]) {
					return false;
				}
			}
			return true;
		}
	}

	void Start ()
	{
		_enemies = new ArrayList (zedCount);
		_isSpawning = new bool[zedCount];

		// Spawn enemies up to zedCount.
		for (int i = zedCount - 1; i >= 0; i--) {
			_enemies.Add (new Object ());
		}
	}

	void setZedCount (int count)
	{
		zedCount = count;
		_enemies = new ArrayList (zedCount);
		_isSpawning = new bool[zedCount];
        
		// Spawn enemies up to zedCount.
		for (int i = zedCount - 1; i >= 0; i--) {
			_enemies.Add (new Object ());
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
	public GameObject spawnEnemy ()
	{
		for (int i = zedCount - 1; i >= 0; i--) {
			if (_enemies [i].Equals (null) && !_isSpawning [i]) {
                // Enemy died. Spawn one new enemy. Make multiple calls to fill the spawn.
                GameObject newEnemy = GameObject.Instantiate(prefabs[Random.Range(0, prefabs.Length)], transform.position, transform.rotation) as GameObject;
                StartCoroutine(createEnemy(i, newEnemy));
                return newEnemy;
            }
		}

        // Enemy didn't spawn. Meh.
        return null;
	}

    public ArrayList getEnemies () {
        return _enemies;
    }

	private IEnumerator createEnemy (int index, GameObject newEnemy)
	{
		isCurrentlySpawning = true;
		_isSpawning [index] = true;
		yield return new WaitForSeconds (spawnDelay);

        _enemies[index] = newEnemy;
        Debug.Log("New Enemy: " + newEnemy);
        if (newEnemy != null) {
            newEnemy.transform.parent = transform.parent.parent;
        }
		_isSpawning [index] = false;
		isCurrentlySpawning = false;
	}
}
