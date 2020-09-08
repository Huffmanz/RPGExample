using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Resources;

namespace RPG.Combat{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Health health = null;

        void Update(){
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().GetTarget();
            if(health == null){
                gameObject.GetComponent<Text>().text = "N/A";
            }else{
                gameObject.GetComponent<Text>().text = String.Format("{0:0}%",health.HealthPercentage());
            }
        }
    }

}