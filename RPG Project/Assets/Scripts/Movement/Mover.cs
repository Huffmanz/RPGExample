using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        NavMeshAgent navMeshAgent;
        [SerializeField] float maxSpeed = 6f;
        
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