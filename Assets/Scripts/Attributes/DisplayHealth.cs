using UnityEngine;
using TMPro;
using RPG.Core;

namespace RPG.Attributes
{
    [RequireComponent(typeof(TMP_Text))]
    public class DisplayHealth : MonoBehaviour
    {
         private TMP_Text healthplayerText;
        private Health playerHealth;

        private void Awake()
        {
            healthplayerText = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null || !player.TryGetComponent(out playerHealth))
            {
                Debug.LogError("DisplayHealth không tìm thấy Health trên Player.", this);
                enabled = false;
            }
        }

        private void Update()
        {
            healthplayerText.text = Mathf.CeilToInt(playerHealth.GetHealthPoints()).ToString();
        }
    }
}
