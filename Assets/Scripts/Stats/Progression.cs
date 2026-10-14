using UnityEngine;
namespace RPG.Stats
{
[CreateAssetMenu(fileName = "Progression", menuName = "Scriptable Objects/Progression")]
public class Progression : ScriptableObject
{
    [SerializeField]private CharacterProgression[] characterClass;

    public float GetHealth(CharacterClass targetClass, int level)
    {
        foreach(CharacterProgression progression in characterClass)
        {
            if(progression.characterClass == targetClass)
            {
                return progression.health[level-1];
            }
        }
        return 0;
    }
    
    [System.Serializable]
    public class CharacterProgression
    { 
     [SerializeField] public CharacterClass characterClass;
     [SerializeField] public float [] health;
    }
    
}
}
