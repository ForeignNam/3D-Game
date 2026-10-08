using UnityEngine;
using System.Collections.Generic;
namespace RPG.Combat
{
public class Pickup : MonoBehaviour
{
   [SerializeField] private Weapon weapon = null;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Fighter>().EquipWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
}