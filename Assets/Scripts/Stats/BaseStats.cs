using UnityEngine;
namespace RPG.Stats
{
public class BaseStats : MonoBehaviour
{
    [Range(1,100)]
    [SerializeField] private int startinglevel = 1;
    [SerializeField] private CharacterClass characterClass;
    [SerializeField] private Progression progression = null;
   
    public float GetHealth()
    {
        return progression.GetHealth(characterClass,startinglevel);
    }
    
}
}
