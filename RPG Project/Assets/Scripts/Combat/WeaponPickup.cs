using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    // Start is called before the first frame update
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5f;
       void OnTriggerEnter(Collider trigger) {
        if(trigger.gameObject.tag == "Player"){
                GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnTime));
        }
        
    }
    private IEnumerator HideForSeconds(float seconds){
        Show(false);
        yield return new WaitForSeconds(seconds);
        Show(true);
    }

    private void Show(bool shouldShow){
        GetComponent<Collider>().enabled = shouldShow;
        foreach(Transform child in transform){
            child.gameObject.SetActive(shouldShow);
        }
    }
}
