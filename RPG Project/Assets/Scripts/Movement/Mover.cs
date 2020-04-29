using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement{
    public class Mover : MonoBehaviour, IAction
    {
        NavMeshAgent navMeshAgent;

        void Start(){
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        void Update()
        {
            // if(Input.GetMouseButton(0)){
            //     MoveToCursor();
            // }
            UpdateAnimator();
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = destination;
            //GetComponent<ActionScheduler>().StartAction(this);
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

        public void StartMoveAction(Vector3 destination){
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }
    }

    }