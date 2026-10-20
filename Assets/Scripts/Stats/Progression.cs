using UnityEngine;
using System.Collections.Generic;
namespace RPG.Stats
{
[CreateAssetMenu(fileName = "Progression", menuName = "Scriptable Objects/Progression")]
public class Progression : ScriptableObject
{
    [SerializeField]private CharacterProgression[] characterClass;
    Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;
        public float GetStats(Stat stat, CharacterClass targetClass, int level)
        {
            BuildLookup();
            float[] levels = lookupTable[targetClass][stat];
            if(levels.Length < level) return 0;
            return levels[level-1];
   
        }
    private void BuildLookup()
    {
        if(lookupTable != null) return;
        lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
        foreach(CharacterProgression progression in characterClass)
        {
            var statLookupTable = new Dictionary<Stat, float[]>();
            foreach(ProgressionStat progressionStat in progression.progressionStat)
            {
                statLookupTable[progressionStat.stat] = progressionStat.levelupgrade;
            }
            lookupTable[progression.characterClass] = statLookupTable;
        }
    }
    public int GetLevelLength(CharacterClass characterclass,Stat stat)
        {
            BuildLookup();
            float[] levels = lookupTable[characterclass][stat];
           return levels.Length;
        }
    [System.Serializable]
    public class CharacterProgression
    { 
     [SerializeField] public CharacterClass characterClass;
     public ProgressionStat[]  progressionStat;
    }
    [System.Serializable]
    public class ProgressionStat
    { 
        [SerializeField] public Stat stat;
        [SerializeField] public float [] levelupgrade;
    }
    
}
}
