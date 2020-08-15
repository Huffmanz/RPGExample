using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;
using RPG.Control;
using UnityEngine;
using System;

namespace RPG.Control
{
    
public class AIController : MonoBehaviour
{
    [SerializeField] float chaseDistance = 2.5f;
    [SerializeField] float suspicionTime = 3f; 
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointTolerance = 1f;
    [SerializeField] float dwellTime = 3f;
    
    [Range(0,1)]
    [SerializeField] float patrolSpeedFraction = 0.2f;
    // Update is called once per frame
    GameObject player;
    Fighter fighter;
    Vector3 guardPosition;
    float timeSinceLastSawPlayer = Mathf.Infinity;
    float timeSinceMoved = Mathf.Infinity;
    int currentWaypoint=0;
    void Start() {
   
        player = GameObject.FindWithTag("Player");
        fighter = GetComponent<Fighter>();
        guardPosition = transform.position;
        
    }
    void Update()
        {
            if(GetComponent<Health>().isDead) return;
            if (InRangeOfPlayer() &&  fighter.CanAttack(player.gameObject))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehavior();
            }
            else if(timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceMoved+=Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardPosition;
            if(patrolPath != null){
                if(AtWaypoint()){
                    CycleWaypoint();
                    timeSinceMoved = 0;
                }
                nextPosition = GetCurrentWaypoint();

                }
                if(timeSinceMoved >= dwellTime){
                    GetComponent<Mover>().StartMoveAction(nextPosition, patrolSpeedFraction);
                    timeSinceMoved = 0;
            }
            
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypoint);
        }

        private void CycleWaypoint()
        {
            currentWaypoint = patrolPath.GetNextIndex(currentWaypoint);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            fighter.Attack(player.gameObject);
        }

        private bool InRangeOfPlayer()
        {
            return Vector3.Distance(player.transform.position, gameObject.transform.position) < chaseDistance;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(gameObject.transform.position, chaseDistance);
        }
    }
}
