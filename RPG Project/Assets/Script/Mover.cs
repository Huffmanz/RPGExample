using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{

    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            MoveToCursor();
        }
        
    }
    void MoveToCursor(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray,out hit);
        if(hasHit){
           NavMeshAgent agent = GetComponent<NavMeshAgent>();
           agent.destination = hit.point;
           
        }
        

    }


}
