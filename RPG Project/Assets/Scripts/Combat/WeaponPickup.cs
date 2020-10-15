using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, IRayCastable
{
    // Start is called before the first frame update
    [SerializeField] WeaponConfig weapon = null;
    [SerializeField] float respawnTime = 5f;
    [SerializeField] float healthToRestore = 0;
    void OnTriggerEnter(Collider trigger) {
        if(trigger.gameObject.tag == "Player")
        {
            Pickup(trigger.gameObject);
        }

    }

    private void Pickup(GameObject subject)
    {
        if(weapon != null)
        {
            subject.GetComponent<Fighter>().EquipWeapon(weapon);
        }
        if(healthToRestore > 0)
        {
            subject.GetComponent<Health>().Heal(healthToRestore);
        }
        StartCoroutine(HideForSeconds(respawnTime));
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

    public bool HandleRaycast(PlayerController callingController)
    {
        if (Input.GetMouseButton(0))
        {
            Pickup(callingController.gameObject);
        }
        return true;
    }

    public CursorType GetCursorType()
    {
        return CursorType.Pickup;
    }
}
