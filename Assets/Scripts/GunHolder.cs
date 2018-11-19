using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class holds all guns
// Also provides a library for gun names and indexes

public class GunHolder : MonoBehaviour {

    public Gun[] allGuns;
    Dictionary<string, int> nameToIdx;

    void Awake() {

        nameToIdx = new Dictionary<string, int>();

        for (int i=0; i<allGuns.Length; i++) {
            nameToIdx.Add(allGuns[i].name, i);
            allGuns[i].GetComponent<SphereCollider>().enabled = true;
        }
    }

    public Gun GetGunByName(string name) {

        if (nameToIdx == null) {
            Awake();    // Lol this is fucking strange
        }

        if (!nameToIdx.ContainsKey(name)) {
            Debug.Log("GunHolder: Not contains gun with specified name");
            return null;
        }

        return allGuns[nameToIdx[name]];
    }

    public Gun GetGunByIdx(int idx) {

        if (idx > allGuns.Length) {
            Debug.Log("GunHolder: Index out of range");
            return null;
        }

        return allGuns[idx];
    }
}
