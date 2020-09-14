using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources{
    public class Health : MonoBehaviour, ISaveable {
        float healthPoints = -1f;
        BaseStats baseStats = null;
        public bool isDead = false;

        void Start(){
            baseStats = GetComponent<BaseStats>();
            if (healthPoints < 0) { 
                healthPoints = baseStats.GetStat(Stat.Health);
            }
        }
        public void TakeDamage(float damage, GameObject instigator){
            healthPoints = Mathf.Max(healthPoints-damage, 0);
            if(healthPoints == 0)
            {
                Die();
                RewardExperience(instigator);
            }
        }


        public float HealthPercentage(){
            if(baseStats == null){
                baseStats = GetComponent<BaseStats>();
                healthPoints = baseStats.GetStat(Stat.Health);
              }
            return (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health)) * 100;
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
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float) state;
            if(healthPoints == 0 ) Die();
        }
    }
}
