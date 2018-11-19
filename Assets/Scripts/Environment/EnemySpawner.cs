using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public bool testing = false;

    // Melee
    public float meleeStartingSpawnSpeed = 3f;
    float meleeCurrSpawnSpeed;

    // Gunner
    public float gunnerStartingSpawnSpeed = 5f;
    float gunnerCurrSpawnSpeed;

    public float speedUpInterval = 10f;
    public float speedUpMultiplier = 1.1f;

    public Transform[] spawnSpots;

    [Header("Enemies")]
    public GameObject melee;
    public GameObject gunner;

	// Use this for initialization
	public void Start () {

        meleeCurrSpawnSpeed = meleeStartingSpawnSpeed;
        gunnerCurrSpawnSpeed = gunnerStartingSpawnSpeed;
	}

    // Responsible for spawning enemies 
    IEnumerator Spawning(string enemyToSpawn) {

        int spawnIdx;
        while (true) {

            spawnIdx = Random.Range(0, spawnSpots.Length);

            if (enemyToSpawn == "Melee") {
                Instantiate(melee, spawnSpots[spawnIdx].position, spawnSpots[spawnIdx].rotation);
                yield return new WaitForSeconds(meleeCurrSpawnSpeed);
            }
            else if (enemyToSpawn == "Gunner") {
                Instantiate(gunner, spawnSpots[spawnIdx].position, spawnSpots[spawnIdx].rotation);
                yield return new WaitForSeconds(gunnerCurrSpawnSpeed);
            }
        }
    }

    // Speeds up spawning every interval
    IEnumerator SpeedUpSpawning() {

        while (true) {

            meleeCurrSpawnSpeed /= speedUpMultiplier;
            gunnerCurrSpawnSpeed /= speedUpMultiplier;
            yield return new WaitForSeconds(speedUpInterval);
        }
    }

    public void StartSpawning() {

        Debug.Log("Starting spawning enemies");

        StartCoroutine(Spawning("Melee"));
        StartCoroutine(Spawning("Gunner"));
        StartCoroutine(SpeedUpSpawning());
    }

    public void StopSpawning() {
        StopAllCoroutines();
    }

    public void KillAllEnemies() {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            Destroy(enemy);
        }
    }
}
