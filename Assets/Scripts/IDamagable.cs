using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface wchich will implement every damagable unit
// Ensures entity to have TakeHit() and TakeDmg() methods
// For example some destroyable objects can implements this

public interface IDamagable {

    // For some fancy stuff like spawning particle system on hit/death
    void TakeHit(float dmg, Vector3 hitPoint, Vector3 hitDirection);
}
