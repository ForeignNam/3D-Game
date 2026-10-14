using UnityEngine;
using TMPro;
using RPG.Core;

namespace RPG.Combat
{
    [RequireComponent(typeof(TMP_Text))]
    public class DisplayEnemyHealth : MonoBehaviour
    {
        private TMP_Text healthEnemyText;
        private Fighter fighter;

        private void Awake()
        {
            healthEnemyText = GetComponent<TMP_Text>();
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }

     

        private void Update()
        {
            Health enemyHealth = fighter.GetTarget();

            // Nếu không có mục tiêu (null), hiển thị N/A và thoát khỏi hàm Update ngay
            if (enemyHealth == null)
            {
                healthEnemyText.text = "N/A";
                return;
            }

            // Nếu có mục tiêu, hiển thị máu của mục tiêu đó
            healthEnemyText.text = Mathf.CeilToInt(enemyHealth.GetHealthPoints()).ToString();
        }
    }
}
