using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        NavMeshAgent navMeshAgent;
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float maxNavMeshPathLength = 40f;

        void Start(){
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        void Update()
        {
            // if(Input.GetMouseButton(0)){
            //     MoveToCursor();
            // }
            navMeshAgent.enabled = !GetComponent<Health>().isDead;
            UpdateAnimator();
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath navMeshPath = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(gameObject.transform.position, destination, NavMesh.AllAreas, navMeshPath);
            if (!hasPath) return false;
            if (navMeshPath.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(navMeshPath) > maxNavMeshPathLength) return false;
            return true;
        }

        private float GetPathLength(NavMeshPath navMeshPath)
        {
            float distance = 0;
            if (navMeshPath.corners.Length < 2) return 0;
            for (int i = 1; i < navMeshPath.corners.Length; i++)
            {
                distance += Vector3.Distance(navMeshPath.corners[i - 1], navMeshPath.corners[i]);
            }
            return distance;
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.destination = destination;
        }
        public void Cancel(){
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator(){
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }

        public void StartMoveAction(Vector3 destination, float speedFraction){
            GetComponent<ActionScheduler>().StartAction(this );
            MoveTo(destination,speedFraction);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            transform.position = ((SerializableVector3) state).ToVector();
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            gameObject.GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }

    }