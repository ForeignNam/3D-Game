using UnityEngine;
namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 100)]
        [SerializeField] private int startinglevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression = null;
        private float currentXP = 0;

        private void Update()
        {
            if (gameObject.CompareTag("Player"))
            {
                print(GetLevel());
            }
                
        }
        public float GetStat(Stat stat)
        {
            return progression.GetStats(stat, characterClass, startinglevel);
        }

        public int GetLevel()
        {
            currentXP = GetComponent<Experience>().GetExperience();
            int preuntilmatelyLevel = progression.GetLevelLength(characterClass,Stat.experienceToLevelUp);
            for (int levels =1; levels <= preuntilmatelyLevel; levels ++)
            {
                float xpToLevelUp = progression.GetStats(Stat.experienceToLevelUp, characterClass, levels);
                if(currentXP < xpToLevelUp)
                {
                    return levels;
                }
            }
            return preuntilmatelyLevel + 1;
        }
    }
}
