using UnityEngine;
using System.Collections.Generic;
using System.Collections;
namespace RPG.Combat
{
public class Pickup : MonoBehaviour
{
   [SerializeField] private Weapon weapon = null;
   [SerializeField] private float timetohide = 5f;
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Fighter>().EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(timetohide));
        }
    }
    private IEnumerator HideForSeconds(float timetohide)
        {
            ShowWeapon(false);
            yield return new WaitForSeconds(timetohide);
            ShowWeapon(true);   
        }
    private void ShowWeapon(bool shouldshow)
        {
	        SphereCollider spherecollider = GetComponent<SphereCollider>();
            spherecollider.enabled = shouldshow;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(shouldshow);               
            }               
        }
        
}
}
