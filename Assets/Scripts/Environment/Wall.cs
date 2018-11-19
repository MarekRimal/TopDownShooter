using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public float closeTime = 5f;
    public float closeSpeed = 2f;
    public Transform startPosition;

    public Transform[] gunSpots;

    public GunHolder gunHolder;
    Gun[] guns;

	// Use this for initialization
	public void Start () {

        transform.position = startPosition.position;

        guns = new Gun[gunHolder.allGuns.Length];

        // Instantiate all guns
        for (int i = 0; i < gunHolder.allGuns.Length; i++) {
            if (gunHolder.allGuns[i].name == "Pistol") {
                continue;   // Already in hand
            }
            guns[i] = Instantiate(gunHolder.allGuns[i], gunSpots[i].position, gunSpots[i].rotation);
        }
    }

    // Will close the start area where you can pick your gun
    public IEnumerator CloseStartArea() {

        Debug.Log("Closing Start Area");

        while (transform.localPosition.x > 0) {

            transform.position += Vector3.forward * closeSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
