using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using System;

namespace RPG.Stats{
    public class Experience : MonoBehaviour, ISaveable {
        [SerializeField] float experiencePoints = 0;

        //public delegate void ExperienceGainedDelegate();
        public event Action onExperienceGained;

        public void GainExperience(float experience){
            experiencePoints+= experience;
            onExperienceGained();
        }
        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float) state;
        }

        public float GetExperience(){
            return experiencePoints;
        }
    }
}
