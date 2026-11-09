using UnityEngine;
using UnityEngine.UI;
namespace RPG.UI.DamageText
{
public class DamageText : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI damageText;
   public void DestroyDamageText()
   {
       Destroy(gameObject);
   }

   public void SetDamageText(float damageAmount)
   {
       damageText.text = damageAmount.ToString();
   }
}
}