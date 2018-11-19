using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameUI gameUI;
    public ScoreKeeper scoreKeeper;
    public EnemySpawner enemySpawner;
    public GunSpawner gunSpawner;
    public Player playerPrefab;
    public Transform playerSpawn;
    public Wall wall;

    Player player;
    string playerGun;

    bool isPlayerInArena = false;

    void Start() {

        player = FindObjectOfType<Player>();
        player.OnPlayerDeath += OnGameOver;
    }

    void Update() {

        // When to start spawning etc.
        if (player != null && !isPlayerInArena && player.transform.position.x < 38) {
            OnArenaEnter();
            isPlayerInArena = true;
            playerGun = player.GetComponentInChildren<Gun>().name;
            print(player.GetComponentInChildren<Gun>().name);
        }
    }

    // When Playerenter the arena
    public void OnArenaEnter() {

        Debug.Log("Player entered the arena");

        StartCoroutine(wall.CloseStartArea());
        enemySpawner.StartSpawning();
    }

    // Call everything necessary after player dies
    public void OnGameOver() {

        Debug.Log("GameOver");

        scoreKeeper.SubmitNewHighscore(ScoreKeeper.kills, playerGun);
        gameUI.GameOver();
    }
}
