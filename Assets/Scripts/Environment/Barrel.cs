using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamagable {

    public float health = 5f;
    public float dmg = 100f;
    public float explosiveRadius = 8f;
    public float explosiveForce = 10f;
    public ParticleSystem explosionEffect;

    public void TakeHit(float dmg, Vector3 hitPoint, Vector3 hitDirection) {

        health -= dmg;
        transform.localScale.Scale(Vector3.one * 1.2f);
        if (health <= 0) {
            Explode();
        }
    }

    void Explode() {

        Instantiate(explosionEffect, transform.position, Quaternion.LookRotation(Vector3.up, Vector3.forward));

        Collider[] nearbyColiders = Physics.OverlapSphere(transform.position, explosiveRadius);

        foreach (Collider col in nearbyColiders) {

            if (col.gameObject.tag == "Player") {
                continue;
            }

            if (col.gameObject.GetComponent<Rigidbody>() == null) {
                continue;
            }
            col.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosiveForce, transform.position, explosiveRadius);

            if (col.gameObject.GetComponent<IDamagable>() != null) {

                col.gameObject.GetComponent<IDamagable>().TakeHit(dmg, col.transform.position, col.transform.position - transform.position);
            }
        }

        Destroy(gameObject);
    }
}
