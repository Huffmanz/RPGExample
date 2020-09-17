﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using System;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;

        public event Action onLevelUp;

        private int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if(experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpEffect, transform);
        }

        public float GetStat(Stat stat){
           return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if(currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        public int CalculateLevel(){
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
