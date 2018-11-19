using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles player control

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    Rigidbody playerRigibody;

    Vector3 playerVelocity;

	// Use this for initialization
	void Start () {

        playerRigibody = GetComponent<Rigidbody>();
        playerRigibody.velocity = Vector3.zero;
	}

    // For correct movement speed
    private void FixedUpdate() {

        if (playerRigibody.gameObject.transform.position.y > 2) {
            playerRigibody.gameObject.transform.position = new Vector3(playerRigibody.gameObject.transform.position.x, 1, playerRigibody.gameObject.transform.position.z) ;
        }

        playerRigibody.gameObject.transform.position += playerVelocity * Time.deltaTime;
        //playerRigibody.MovePosition(playerRigibody.position + playerVelocity * Time.deltaTime);
    }

    // Moving
    public void Move(Vector3 velocity) {

        playerVelocity = velocity;
    }

    // Rotating
    public void LookAt(Vector3 lookPoint) {

        Vector3 heightCorrectPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);   // For not tilting down
        transform.LookAt(heightCorrectPoint);
    }

}
