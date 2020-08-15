using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics{
    public class CinematicTrigger : MonoBehaviour
    {
        bool triggered = false;
        private void OnTriggerEnter(Collider trigger) {
            if(!triggered && trigger.gameObject.tag == "Player"){
                triggered = true;
                GetComponent<PlayableDirector>().Play();
            }
            

        }
    }

}
