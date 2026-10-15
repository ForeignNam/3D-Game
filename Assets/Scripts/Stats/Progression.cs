using UnityEngine;
namespace RPG.Stats
{
[CreateAssetMenu(fileName = "Progression", menuName = "Scriptable Objects/Progression")]
public class Progression : ScriptableObject
{
    [SerializeField]private CharacterProgression[] characterClass;

    public float GetStats(Stat stat, CharacterClass targetClass, int level)
    {
        foreach(CharacterProgression progression in characterClass)
        {
            if(progression.characterClass == targetClass)
            {
                foreach(ProgressionStat progressionStat in progression.progressionStat)
                {
                   if(progressionStat.stat != stat) continue;
                   if(progressionStat.levelupgrade.Length < level) continue;
                   return progressionStat.levelupgrade[level-1]; 
                }
            }
        }
        return 0;
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
