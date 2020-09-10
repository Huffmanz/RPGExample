using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;


        public float GetStat(Stat stat){
           return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel(){
          Experience experience = GetComponent<Experience>();
          if(experience == null) return startingLevel;

          float currentXp = experience.GetExperience();
          int penultimateLevel = progression.GetLevels(characterClass, Stat.ExperienceToLevelUp);
          for(int level = 1; level < penultimateLevel ; level++){
            float XPToLevel = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
            if(XPToLevel > currentXp){
              return level;
            }
          }
          return penultimateLevel + 1;
        }
    }


}
