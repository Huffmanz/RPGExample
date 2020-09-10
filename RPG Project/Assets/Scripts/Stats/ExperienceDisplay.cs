using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;

namespace RPG.Stats{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience xp = null;
        void Awake(){
            xp = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        }

        void Update(){
            gameObject.GetComponent<Text>().text = xp.GetExperience().ToString();
        }
    }

}
