using UnityEngine;
using TMPro;
using RPG.Core;
namespace RPG.Attributes
{
public class ExperienceDisplay : MonoBehaviour
{
        private TMP_Text experienceText;
        private Health playerHealth;
        private Experience playerExperience ; 
        private void Awake()
        {
            experienceText = GetComponent<TMP_Text>();
            playerExperience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        } 
        private void Update()
        {
            experienceText.text = Mathf.CeilToInt(playerExperience.GetExperience()).ToString();
        }
}
}
