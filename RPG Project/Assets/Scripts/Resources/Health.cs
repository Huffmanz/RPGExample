using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using Utils;

namespace RPG.Resources{
    public class Health : MonoBehaviour, ISaveable {
        [SerializeField] float regenerationPercent = .7f;
        LazyValue<float> healthPoints;
        BaseStats baseStats = null;
        public bool isDead = false;

        void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        void Start(){
            baseStats = GetComponent<BaseStats>();
            if (baseStats == null) return;
            baseStats.onLevelUp += SetHealth;
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPoHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return baseStats.GetStat(Stat.Health);
        }

        private void SetHealth()
        {
            float tempHealth = baseStats.GetStat(Stat.Health) * regenerationPercent;
            healthPoints.value = Mathf.Max(tempHealth, healthPoints.value);
        }
        public void TakeDamage(float damage, GameObject instigator){
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            if(healthPoints.value == 0)
            {
                Die();
                RewardExperience(instigator);
            }
        }


        public float HealthPercentage(){
            if(baseStats == null){
                baseStats = GetComponent<BaseStats>();
                //healthPoints = baseStats.GetStat(Stat.Health);
              }
            return (healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health)) * 100;
        }
        private void Die()
        {
            if(!isDead){
                GetComponent<Animator>().SetTrigger("Death");
                isDead = true;
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
        }

        private void RewardExperience(GameObject instigator){
            Experience experience = instigator.GetComponent<Experience>();
            if(experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }
        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float) state;
            if(healthPoints.value == 0 ) Die();
        }
    }
}
