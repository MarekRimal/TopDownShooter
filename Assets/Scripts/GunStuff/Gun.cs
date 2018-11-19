using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public GunHolder gunHolder;

    public enum FireMode { Single, Burst, Auto };

    [Header("Characteristics")]
    public string name;
    public FireMode fireMode;
    public float dmg;
    public float bulletSpeed;
    public float msBetweenBullets;
    public int burstBulletCount;

    [Header("Bullet Stuff")]
    public Transform[] bulletSpawns;                 // So gun can have more barells
    public Projectile bullet;
    public float bulletLifeTime = 5f;
    public LayerMask enemyMask;                      // What enemy projectile should colide with
    public LayerMask playerMask;

    // Helper variables
    float nextShotTime;
    bool triggerReleasedSinceLastShot = false;
    int shotsRemainingInBurst;
    bool isPickedUp = false;
    Collider gunCollider;

    // For C4 and Mines
    //public int maxMines = 5;
    //int minesLeft;
    //Projectile[] c4mines;

    // Use this for initialization
    void Start () {

        // C4 stuff
        //minesLeft = maxMines;
        //c4mines = new Projectile[maxMines];

        //gunCollider = GetComponent<SphereCollider>();

        nextShotTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        if (!isPickedUp) {
            transform.Rotate(Vector3.up, 2f);
        }
    }

    void Shoot() {

        //if (gameObject.name == "C4" && minesLeft <= 0) {
        //    return;
        //}

        // Do shooting only if its time to
        if (Time.time > nextShotTime) {

            // FireMode logic
            if (fireMode == FireMode.Burst) {

                if (shotsRemainingInBurst == 0) {
                    return;
                }
                shotsRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single) {

                if (!triggerReleasedSinceLastShot) {
                    return;
                }
            }

            // Actual shooting
            foreach (Transform barell in bulletSpawns) {

                nextShotTime = Time.time + msBetweenBullets / 1000;

                Projectile projectile = Instantiate(bullet, barell.position, barell.rotation) as Projectile;
                projectile.setSpeed(bulletSpeed);
                projectile.setDmg(dmg);
                projectile.setLifeTime(bulletLifeTime);
                projectile.setLayerMask(playerMask);

                // C4 thing
                //c4mines[maxMines - minesLeft] = projectile;
                //minesLeft--;
            }
        }
    }

    //public void Detonade() {

    //    print("Detonating");

    //    foreach (Projectile mine in c4mines) {
    //        mine.Explode();
    //    }

    //    minesLeft = maxMines;
    //}

    // I dont want enemies to shoot just straight because that is not much accurate
    void EnemyShoot(Vector3 dirToTarget) {

        // Do shooting only if its time to
        if (Time.time > nextShotTime) {

            // FireMode logic
            if (fireMode == FireMode.Burst) {

                if (shotsRemainingInBurst == 0) {
                    return;
                }
                shotsRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single) {

                if (!triggerReleasedSinceLastShot) {
                    return;
                }
            }

            // Actual shooting
            foreach (Transform barell in bulletSpawns) {

                nextShotTime = Time.time + msBetweenBullets / 1000;

                Projectile projectile = Instantiate(bullet, barell.position, barell.rotation) as Projectile;

                projectile.transform.rotation = Quaternion.LookRotation(dirToTarget);   // ONLY DIFERECE -------------------

                projectile.setSpeed(bulletSpeed);
                projectile.setDmg(dmg);
                projectile.setLayerMask(enemyMask);
            }
        }
    }

    public void OnTriggerHold() {
        Shoot();
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerHoldEnemy(Vector3 dirToTarget) {
        EnemyShoot(dirToTarget);
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease() {
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstBulletCount;
    }

    // Instantiate and parent gun GameObject
    public void Equip(Transform gunSpot) {

        isPickedUp = true;
        //gunCollider.enabled = false;

        // Set new transform and parent the gun to gunSpot
        gameObject.transform.position = gunSpot.transform.position;
        gameObject.transform.rotation = gunSpot.transform.rotation;
        gameObject.transform.SetParent(gunSpot);
    }

    public void Drop() {
        isPickedUp = false;
        //gunCollider.enabled = true;
    }

    // Corect the gun direction
    public void Aim(Vector3 aimPoint) {
        transform.LookAt(aimPoint);
    }
}

