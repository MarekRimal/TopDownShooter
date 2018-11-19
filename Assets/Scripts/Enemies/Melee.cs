using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Melee : Enemy {

    // Update is called once per frame
    void Update() {

        if (hasTarget) {

            if (Time.time > nextAttackTime) {

                // I compare squared values because the square root is expensive
                sqrDistanceToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDistanceToTarget < Mathf.Pow(attackDistance + enemyCollisionRadius + targetCollisionRadius, 2)) {

                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }
    }

    IEnumerator Attack() {

        currState = State.Attacking;
        pathfinder.enabled = false;         // Dont move while attacking

        Vector3 originalPos = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPos = target.position - dirToTarget * (enemyCollisionRadius);

        float attackSpeed = 3;  // How fast is the attack - the animation
        float percent = 0;

        skinMaterial.color = attackColor;
        bool hasAppliedDmg = false;

        while (percent <= 1) {

            if (percent >= 0.5f && !hasAppliedDmg) {
                hasAppliedDmg = true;
                if (target != null) {
                    target.GetComponent<IDamagable>().TakeHit(dmg, target.position, dirToTarget);
                }
            }
            percent += Time.deltaTime * attackSpeed;
            float interpolation = 4 * (-Mathf.Pow(percent, 2) + percent);
            transform.position = Vector3.Lerp(originalPos, attackPos, interpolation);

            yield return null;
        }

        skinMaterial.color = origColor;

        GetComponent<Rigidbody>().Sleep();      // Solving bounce effect
        currState = State.Chasing;
        pathfinder.enabled = true;
    }

    // Avoiding updating path each frame - updates path each refreshTime
    protected override IEnumerator UpdatePath() {

        while (hasTarget) {

            if (currState == State.Chasing) {

                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (targetCollisionRadius + enemyCollisionRadius + attackDistance / 2);

                // When far away refresh less and randomize tanrget location
                if (sqrDistanceToTarget >  Mathf.Pow(refreshTresholdDistance, 2)) {
                    pathfinder.SetDestination(RandomizeTargetLocation(2));              // This method is in Enemy class
                    yield return new WaitForSeconds(pathRefreshTime*2);
                }
                else if(sqrDistanceToTarget > Mathf.Pow(randomTresholdDistance, 2)) {
                    pathfinder.SetDestination(RandomizeTargetLocation(2));
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
