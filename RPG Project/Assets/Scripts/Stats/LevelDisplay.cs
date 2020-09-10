using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;

namespace RPG.Stats{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats = null;
        void Awake(){
            baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        }

        void Update(){
            gameObject.GetComponent<Text>().text = baseStats.GetLevel().ToString();
        }
    }

}
