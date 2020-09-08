using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources{
    public class HealthDisplay : MonoBehaviour
    {
        Health health = null;
        void Awake(){
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }

        void Update(){
            gameObject.GetComponent<Text>().text = String.Format("{0:0}%",health.HealthPercentage());
        }
    }

}