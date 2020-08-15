using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{

    enum DestinationIdentifier{
        A, B, C, D, E
    }
    [SerializeField] int SceneToLoad=-1;
    [SerializeField] Transform SpawnPoint;

    [SerializeField] DestinationIdentifier destination;
    void OnTriggerEnter(Collider trigger) {
        if(trigger.gameObject.tag == "Player"){
                StartCoroutine(Transition());
            }
    }
    private IEnumerator Transition(){
        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(SceneToLoad);
        Portal otherPortal = getOtherPortal();
        UpdatePlayer(otherPortal);
        Destroy(gameObject);
    }

    private void UpdatePlayer(Portal otherPortal)
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<NavMeshAgent>().Warp(otherPortal.SpawnPoint.position);
        player.transform.rotation = otherPortal.SpawnPoint.rotation;
    }

    private Portal getOtherPortal()
    {
        foreach(Portal p in FindObjectsOfType<Portal>()){
            if(p != this && p.destination == this.destination){
                return p;
            }
        }
        return null;
    }
}
