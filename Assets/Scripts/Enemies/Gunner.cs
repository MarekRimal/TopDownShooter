using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Enemy {

    public GunHolder gunHolder;
    public Transform gunSpot;
    Gun gun;

    public LayerMask obstacleMask;

    protected override void Start() {
        base.Start();

        gun = Instantiate(gunHolder.GetGunByName("SMG"), gunSpot.position, gunSpot.rotation) as Gun;
        gun.Equip(gunSpot);
    }

    void Update() {

        if (hasTarget) {

            if (Time.time > nextAttackTime) {

                // I compare squared values because the square root is expensive
                sqrDistanceToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDistanceToTarget < Mathf.Pow(attackDistance, 2)) {

                    // If there is no obstacles between Enemy and Player
                    if (!Physics.Linecast(transform.position, player.transform.position, obstacleMask)) {

                        Vector3 dirToTarget = (player.transform.position - transform.position).normalized;
                        gun.OnTriggerHoldEnemy(dirToTarget);
                        nextAttackTime = Time.time + timeBetweenAttacks;
                    }
                }
            }
        }
    }

    // Avoiding updating path each frame - updates path each refreshTime
    protected override IEnumerator UpdatePath() {

        while (hasTarget) {

            if (currState == State.Chasing) {

                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (targetCollisionRadius + enemyCollisionRadius + attackDistance / 2);

                // When far away refresh less and randomize tanrget location
                if (sqrDistanceToTarget > Mathf.Pow(refreshTresholdDistance, 2)) {
                    pathfinder.SetDestination(RandomizeTargetLocation(1));              // This method is in Enemy class
                    yield return new WaitForSeconds(pathRefreshTime * 2);
                }
                else if (sqrDistanceToTarget > Mathf.Pow(randomTresholdDistance, 2)) {
                    pathfinder.SetDestination(RandomizeTargetLocation(1));
                    yield return new WaitForSeconds(pathRefreshTime);
                }
                else {
                    pathfinder.SetDestination(targetPosition);
                    yield return new WaitForSeconds(pathRefreshTime);
                }
            }
            else {
                yield return new WaitForSeconds(pathRefreshTime);
            }
        }
    }
}
