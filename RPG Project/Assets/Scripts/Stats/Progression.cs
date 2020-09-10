using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Stats{
    [CreateAssetMenu(fileName="Progression", menuName="Stats/Progression", order=0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookup = null;
        public float GetStat(Stat stat, CharacterClass characterClass, int level){
            BuildLookup();
            //make sure class exists
            if(!lookup.ContainsKey(characterClass)) return 0;
            //make sure stat exists
            if(!(lookup[characterClass].ContainsKey(stat))) return 0;
            //check length to make sure it has enough items
            if(lookup[characterClass][stat].Length < level) return 0;
            return lookup[characterClass][stat][level-1];
        }

        public int GetLevels(CharacterClass characterClass, Stat stat){
          BuildLookup();
          return lookup[characterClass][stat].Length;
        }

        private void BuildLookup()
        {
            if(lookup != null ) return;
            lookup = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach(ProgressionCharacterClass character in characterClasses){
                Dictionary<Stat, float[]>  characterProgress = new Dictionary<Stat, float[]>();
                foreach(ProgressionStat progressionStat in character.stats){
                   characterProgress.Add(progressionStat.stat, progressionStat.levels);
                }
                lookup.Add(character.characterClass, characterProgress);
            }
        }

        [System.Serializable]
    class ProgressionCharacterClass{
        public CharacterClass characterClass;
        public ProgressionStat[] stats;

    }

    [System.Serializable]
    class ProgressionStat{
        public Stat stat;
        public float[] levels;


    }
    }

}
