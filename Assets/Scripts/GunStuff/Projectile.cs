using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    protected LayerMask collisionMask;     // This is like what are we interested in?

    [Header("Cannon stuff")]     // May be better to inherit the Cannon bullet
    public float kickBackForce;
    public float kickAreaReducePercent;
    public float areaDmgPercent;
    public float areaRadius;

    protected float speed = 10f;
    protected float dmg = 1f;
    protected float lifetime = 5f;

    protected float skinWidth = 0.1f;             // To compensate enemy movement toward the bullet

    void Start() {

        transform.SetParent(null);

        // Raycasting shooting didnt notice if the projectile starts inside of some object so we need to check it first
        // This will get us array of all colliders our projectile intersects with
        Collider[] initialCollision = Physics.OverlapSphere(transform.position, 0.1f, collisionMask);
        if (initialCollision.Length > 0) {
            OnHitObject(initialCollision[0], transform.position);
        }
        
    }

    void FixedUpdate () {

        if (gameObject.name != "C4(Clone)") {

            float moveDistance = speed * Time.deltaTime;    // This is the distance which the projectile will make between each frame
            CheckCollisions(moveDistance);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
	}

    // Collision checking
    void CheckCollisions(float moveDistance) {

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // The out var will be assigned after the if statement
        // AKA the hit will get our interesting and important information
        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide)) {

            // If we hit something call OnHitObject
            OnHitObject(hit.collider, hit.point);
        }
    }

    // On hit behaviour
    void OnHitObject(Collider col, Vector3 hitPoint) {

        // Fancy stuff - mostly for Cannon
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, areaRadius, collisionMask);
        foreach (Collider c in nearbyColliders) {

            if (c.gameObject.GetComponent<Rigidbody>() == null || c == col  // The hitted one will be served alone
                            || c.gameObject.tag == "Player" || c.gameObject.tag == "Gun") {   
                continue;
            }
            c.gameObject.GetComponent<Rigidbody>().AddForce((col.transform.position - transform.position).normalized * kickBackForce);

            if (c.gameObject.GetComponent<IDamagable>() != null) {
                c.gameObject.GetComponent<IDamagable>().TakeHit(dmg * areaDmgPercent, col.transform.position, col.transform.position - transform.position);
            }
        }

        if (col.gameObject.GetComponent<Rigidbody>() != null) {
            col.gameObject.GetComponent<Rigidbody>().AddForce((transform.rotation * transform.forward).normalized * kickBackForce);
        }
        if (col.gameObject.GetComponent<IDamagable>() != null) {
            col.gameObject.GetComponent<IDamagable>().TakeHit(dmg, col.transform.position + Vector3.back, col.transform.position - transform.position);
        }

        GameObject.Destroy(gameObject);
    }

    // Only for C4 or Mine
    public void Explode() {

        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, areaRadius, collisionMask);
        foreach (Collider col in nearbyColliders) {

            if (col.gameObject.GetComponent<Rigidbody>() == null
                            || col.gameObject.tag == "Player" || col.gameObject.tag == "Gun") {
                continue;
            }
            col.gameObject.GetComponent<Rigidbody>().AddExplosionForce(kickBackForce, transform.position, areaRadius);
            StartCoroutine(col.gameObject.GetComponent<Enemy>().StopAfterTime(1f));

            if (col.gameObject.GetComponent<IDamagable>() != null) {

                col.gameObject.GetComponent<IDamagable>().TakeHit(dmg, col.transform.position, col.transform.position - transform.position);
            }
        }

        Destroy(gameObject);
    }

    // For Mines
    private void OnTriggerEnter(Collider other) {

        if (speed == 0) {           // Indicates its a Mine
            Explode();
        }
    }

    // --- Setters ----

    public void setSpeed(float _speed) {
        speed = _speed;
    }

    public void setDmg(float _dmg) {
        dmg = _dmg;
    }

    public void setLifeTime(float lifetime) {
        Destroy(gameObject, lifetime);
    }

    public void setLayerMask(LayerMask mask) {
        collisionMask = mask;
    }
}
