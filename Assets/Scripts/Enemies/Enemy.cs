using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IDamagable {

    public float maxHealth = 100f;
    protected float currHealth;
    public float speed = 4f;
    public float dmg;
    public float timeBetweenAttacks = 1f;
    protected float nextAttackTime;
    public float attackDistance = 1f;

    public float pathRefreshTime = .25f;            // How often path is refreshed
    public float refreshTresholdDistance = 25f;     // Distance when refreshrate increases
    public float randomTresholdDistance = 3f;       // Distance when randomization stops
    protected float sqrDistanceToTarget = 100f;

    protected Material skinMaterial;
    protected Color origColor;
    public Color attackColor;
    public Color hurtColor;
    public float hitAnimDuration = .25f;

    protected float enemyCollisionRadius;
    protected float targetCollisionRadius;

    protected NavMeshAgent pathfinder;
    protected Transform target;
    protected Player player;
    protected bool hasTarget;

    protected enum State { Idle, Chasing, Attacking };
    protected State currState;

    public ScoreKeeper scoreKeeper;
    protected bool isAlive = true;

    public ParticleSystem deathEffect;

	// Use this for initialization
	protected virtual void Start () {

        // Try find Player
        try{
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            target = player.transform;
            player.OnPlayerDeath += OnTargetDeath;
            hasTarget = true;
        }
        catch(NullReferenceException e) {

            hasTarget = false;
        }

        // Colliders
        enemyCollisionRadius = GetComponent<CapsuleCollider>().radius;
        if (hasTarget) {
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
        }

        // Skin
        skinMaterial = GetComponent<Renderer>().material;
        origColor = skinMaterial.color;

        // Initialize
        nextAttackTime = Time.time;
        currHealth = maxHealth;

        // Navigation
        pathfinder = GetComponent<NavMeshAgent>();
        pathfinder.speed = speed;

        currState = State.Chasing;

        StartCoroutine(UpdatePath());
    }

    public void TakeHit(float dmg, Vector3 hitPoint, Vector3 hitDirection) {

        StartCoroutine(HitAnim());

        currHealth -= dmg;
        if (currHealth <= 0) {

            deathEffect.startColor = origColor;
            skinMaterial.color = origColor;
            deathEffect.GetComponent<Renderer>().material = skinMaterial;
            Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection));

            if (isAlive) {
                Die();
                isAlive = false;
            }
        }
    }

    private void Die() {

        scoreKeeper.IncreaseScore();
        Destroy(gameObject);
    }

    // The color flash when enemy get hit
    IEnumerator HitAnim() {

        int i = 0;
        while (i < 1) {

            GetComponent<MeshRenderer>().materials[0].color = hurtColor;
            i++;
            yield return new WaitForSeconds(hitAnimDuration);
        }

        GetComponent<MeshRenderer>().materials[0].color = origColor;
    }

    void OnTargetDeath() {
        hasTarget = false;
        currState = State.Idle;
    }

    // Avoiding agents to run in one straight line 
    protected Vector3 RandomizeTargetLocation(int distanceDivider) {

        float distance = Vector3.Distance(target.position, transform.position);
        distance /= distanceDivider;

        return new Vector3(player.transform.position.x + UnityEngine.Random.Range(0, distance), 0, player.transform.position.z + UnityEngine.Random.Range(0, distance));
    }

    // Stop enemy rigidBody after some time
    public IEnumerator StopAfterTime(float time) {

        yield return new WaitForSeconds(time);

        try {
            GetComponent<Rigidbody>().velocity = Vector3.one * 0.1f; 
        }
        catch (MissingReferenceException e) {
            // fuk u
        }
    }

    protected abstract IEnumerator UpdatePath();
}
