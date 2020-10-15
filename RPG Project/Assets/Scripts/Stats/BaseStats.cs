using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using System;
using Utils;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;
        [SerializeField] bool shouldUseModifier = false;

        public event Action onLevelUp;

        private LazyValue<int> currentLevel;
        Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            currentLevel.ForceInit();
           
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }
        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpEffect, transform);
        }

        public float GetStat(Stat stat){

           return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1+(GetPercentageModifier(stat)/100));
        }



        public int GetLevel()
        {
            return currentLevel.value;
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

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            float statModifier = 0;
            if (!shouldUseModifier) return statModifier;
            foreach(IModifierProvider provider in gameObject.GetComponents<IModifierProvider>())
            {

                foreach(float modifier in provider.GetAdditiveModifiers(stat))
                {
                    statModifier += modifier;
                }
            }
            return statModifier;

        }
        private float GetPercentageModifier(Stat stat)
        {
            float statModifier = 0;
            if (!shouldUseModifier) return statModifier;
            foreach (IModifierProvider provider in gameObject.GetComponents<IModifierProvider>())
            {

                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    statModifier += modifier;
                }
            }
            return statModifier;
        }
    }


}
