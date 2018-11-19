using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class representing the player
// Taking input but player is affected through PlayerController

[RequireComponent(typeof(PlayerController))]					
public class Player : MonoBehaviour, IDamagable {

    // Gun stuff
    public GunHolder gunHolder;
    public Transform gunSpot;
    Gun equippedGun;

    [Header("Characteristics")]
    public float maxHealth = 100f;
    public float moveSpeed = 5f;
    public float pickUpRadius = 5f;

    public ParticleSystem deathEffect;

    PlayerController playerController;

    Camera viewCamera;
    public float camHeight = 12f;

    float currHealth;   

    public event System.Action OnPlayerDeath;
    public event System.Action OnRestartGame;

    // This should be done before gun controller Start()
    private void Awake() {

        playerController = GetComponent<PlayerController>();
        viewCamera = Camera.main;
    }

    // Use this for initialization
    public void Start () {

        currHealth = maxHealth;
        transform.position = new Vector3(transform.position.x ,1f , transform.position.z);

        // Starting gun
        if (equippedGun != null) {
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate(gunHolder.GetGunByName("Pistol"), gunSpot.position, gunSpot.rotation) as Gun;
        equippedGun.Equip(gunSpot);
        if (equippedGun == null) {
            Debug.LogError("Pistol not found in gunHolder");
        }
    }
	
	// Update is called once per frame
	protected virtual void Update () {

        GetInput();
        MoveCamera();
	}

    // Handles input through controller scripts
    void GetInput() {

        // Movement
        Vector3 velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = velocity.normalized * moveSpeed;
        playerController.Move(moveVelocity);

        // Turning
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.up * gunSpot.transform.position.y);       
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance)) {    // Returns true if the ray intersects with the plane and also assigns
                                                            // value to the rayDistance - the out keyword

            Vector3 intersection = ray.GetPoint(rayDistance);   // This is the itercestion point
                                                                //Debug.DrawLine(ray.origin, intersection, Color.red);
            playerController.LookAt(intersection);
            equippedGun.Aim(intersection);
        }

        // Gun pick up
        if (Input.GetKeyDown(KeyCode.F)) {
            PickUpGun();
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            DropCurrentGun();
        }

        // Shooting
        if (equippedGun != null) {
            if (Input.GetMouseButton(0)) {
                equippedGun.OnTriggerHold();
            }
            else if (Input.GetMouseButtonUp(0)) {
                equippedGun.OnTriggerRelease();
            }
            //if (equippedGun.name == "C4(Clone)") {
            //    if (Input.GetMouseButtonUp(0)) {
            //        print("Want to detonate");
            //        equippedGun.Detonade();
            //    }
            //}
        }

        // Cam rise up
        if (Input.GetKeyDown(KeyCode.O)) {
            camHeight += 3f;
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            camHeight -= 3f;
        }

        // Exit game 
        if (Input.GetKey(KeyCode.R)) {
            FindObjectOfType<SceneManager>().ReloadLevel();
            //Application.LoadLevel(Application.loadedLevel);
        }

        // Exit game 
        if (Input.GetKey(KeyCode.Escape)) {
            FindObjectOfType<SceneManager>().LoadLevel("Menu");
        }

        // WTFFFFFFFF DISABLING COLLIDER DOSENT WORKKKK
        if (Input.GetKeyDown(KeyCode.Space)) {
            equippedGun.GetComponent<SphereCollider>().enabled = !equippedGun.GetComponent<SphereCollider>().enabled;
        }

    }

    void PickUpGun() {

        Gun gunToEquip = null;
        Collider[] gunsInRadius = Physics.OverlapSphere(transform.position, pickUpRadius);
        float closest = pickUpRadius;

        // Find the closest Gun
        foreach (Collider gunCol in gunsInRadius) {

            // Check whether the object is a gun
            if (gunCol.GetComponent<Gun>() == null 
                        || gunCol.GetComponent<Gun>() == equippedGun) {   // We are interested only in guns (other)

                continue;
            }

            float distance = Vector3.Distance(transform.position, gunCol.gameObject.transform.position);
            if (distance < closest) {
                gunToEquip = gunCol.GetComponent<Gun>();
            }
        }

        // Equip the closest Gun and drop the old one
        if (gunToEquip != null) {

            if (equippedGun != null) {
                DropCurrentGun();
            }
            PickUpNewGun(gunToEquip);
        }
    }

    void DropCurrentGun() {
        transform.Find("GunSpot").gameObject.transform.DetachChildren();
        equippedGun.Drop();
        equippedGun = null;
    }

    void PickUpNewGun(Gun gunToEquip) {
        gunToEquip.Equip(gunSpot);
        equippedGun = gunToEquip;
    }

    public void TakeHit(float dmg, Vector3 hitPoint, Vector3 hitDirection) {

        currHealth -= dmg;
        if (currHealth <= 0) {
            Die();
        }
    }

    void Die() {

        // Death effect
        deathEffect.startColor = GetComponent<Renderer>().material.color;
        deathEffect.GetComponent<Renderer>().material = GetComponent<Renderer>().material;
        Instantiate(deathEffect.gameObject, transform.position, Quaternion.FromToRotation(Vector3.zero, Vector3.up));

        OnPlayerDeath();
        Destroy(gameObject);
    }

    void MoveCamera() {

        viewCamera.transform.position = transform.position + new Vector3(0f, camHeight, -4f);
    }

    public float getCurrHealth() {
        return currHealth;
    }
}
