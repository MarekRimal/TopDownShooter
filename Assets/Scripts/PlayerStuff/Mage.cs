using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour, ICharacter {

    [Header("Spells")]
    public string firstSpellName;
    public string secondSpellName;

    public ParticleSystem pulseEffect;
    public ParticleSystem blinkEffect;
    public float pulseDmg;
    public float pulseKnockBack;
    public float pulseDuration;
    public float pulseRange;
    public float pulseCd;
    public float blinkCd;

    float nextPulseTime;
    float nextBlinkTime;

    void Start() {

        nextPulseTime = Time.time;
        nextBlinkTime = Time.time;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.E)) {
            if (Time.time > nextPulseTime) {
                Pulse();
                nextPulseTime = Time.time + pulseCd;
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            if (Time.time > nextBlinkTime) {
                Blink();
                nextBlinkTime = Time.time + blinkCd;
            }
        }
    }

    void Pulse() {

        Instantiate(pulseEffect, transform.position, Quaternion.LookRotation(Vector3.up, Vector3.right));

        // Get all enemies in range
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, pulseRange);

        // Deal dmg and knock back all enemies in the pulseAngle
        // NOTE: If the knock back wont work than you can implement like a coroutine which will make them run away
        // That could be also solution to the Cannon problem
        foreach (Collider enemy in enemiesInRange) {

            // We are only interensted in enemies
            if (enemy.gameObject.tag != "Enemy") {
                continue;
            }

            Vector3 vecToEnemy = (enemy.transform.position - transform.position).normalized;


            enemy.gameObject.GetComponent<IDamagable>().TakeHit(pulseDmg, enemy.transform.position, vecToEnemy);
            enemy.GetComponent<Rigidbody>().AddForce(vecToEnemy * pulseKnockBack, ForceMode.Impulse);
            StartCoroutine(enemy.GetComponent<Enemy>().StopAfterTime(pulseDuration));
        }
    }

    // Teleports player to mouse position
    void Blink() {

        Instantiate(blinkEffect, transform.position, Quaternion.LookRotation(Vector3.up, Vector3.right));

        Vector3 posToBlink;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, 1f);
        float distance;

        if (groundPlane.Raycast(ray, out distance)) {
            posToBlink = new Vector3(ray.GetPoint(distance).x, 1f, ray.GetPoint(distance).z);
            transform.position = posToBlink;
            Instantiate(blinkEffect, transform.position, Quaternion.LookRotation(Vector3.up, Vector3.right));
        }
    }

    // Returns remaining cooldowns
    public float GetFisrtSpellCD() {
        return Mathf.Max(nextPulseTime - Time.time, 0);
    }

    public float GetSecondSpellCD() {
        return Mathf.Max(nextBlinkTime - Time.time, 0);
    }

    // Returns spell names
    public string GetFisrtSpellName() {
        return firstSpellName;
    }

    public string GetSecondSpellName() {
        return secondSpellName;
    }
}
