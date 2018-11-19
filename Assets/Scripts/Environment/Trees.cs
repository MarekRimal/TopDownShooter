using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour {

    public float treeRadius = 0.2f;

	// Use this for initialization
	void Start () {

        foreach (Transform tree in transform) {
            CapsuleCollider col = tree.gameObject.GetComponent<CapsuleCollider>();
            col.center = Vector3.zero;
            col.radius = treeRadius;
            col.height = 100f;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
