using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using RPG.SceneManagement;
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
    [SerializeField] float fadeOutTime = 1f;
    [SerializeField] float fadeInTime = 1f;
    [SerializeField] float fadeWaitTime = 0.5f;

    [SerializeField] DestinationIdentifier destination;
    void OnTriggerEnter(Collider trigger) {
        if(trigger.gameObject.tag == "Player"){
                StartCoroutine(Transition());
            }
    }
    private IEnumerator Transition(){
        DontDestroyOnLoad(gameObject);
        SavingWrapper saveSystem = GameObject.FindObjectOfType<SavingWrapper>();
        saveSystem.Save();
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeOutTime);
        yield return SceneManager.LoadSceneAsync(SceneToLoad);
        saveSystem.Load();
        Portal otherPortal = getOtherPortal();
        UpdatePlayer(otherPortal);
        saveSystem.Save();
        yield return new WaitForSeconds(fadeWaitTime);
        yield return fader.FadeIn(fadeInTime);
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
