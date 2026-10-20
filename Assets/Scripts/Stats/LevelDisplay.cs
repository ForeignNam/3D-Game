using TMPro;
using UnityEngine;
namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        private BaseStats stats;
        private TMP_Text displayText;
        private void Awake()
        {
            stats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
            displayText = GetComponent<TMP_Text>();
        }
        
        // Update is called once per frame
        void Update()
        {
            displayText.text = 
      stats.GetLevel()
      .ToString();
        }
    }
}