using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {

        [SerializeField] private DamageText damageTextPrefab;
    
        void Start()
        {
           // SpawnDamageText(10f);
        }
        public void SpawnDamageText(float damageAmount)
        {
        DamageText damageTextInstance = Instantiate<DamageText>(damageTextPrefab, transform);
            damageTextInstance.SetDamageText(damageAmount);
        }        
    }
}
