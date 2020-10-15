using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat{ 
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRayCastable {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        } 

        public bool HandleRaycast(PlayerController controller)
        {
            Fighter fighter = controller.transform.GetComponent<Fighter>();
            if (!fighter.CanAttack(gameObject))
            {
                return false;

            }
            if (Input.GetMouseButtonDown(0))
            {
                fighter.Attack(gameObject);
            }
            return true;
        }

    }
}