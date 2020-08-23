using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat{

    public class Fighter : MonoBehaviour, IAction{
    
        Health target;
        [SerializeField] float timeBetweenAttacks = 1f;

        [SerializeField] Transform handTransform = null;
        [SerializeField] Weapon weapon = null;

        
        float timeSinceLastAttack = Mathf.Infinity;

        public void Start() {
            SpawnWeapon();
        }
        public void Update(){
            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return;
            if(target.isDead) return;
            if(Vector3.Distance(transform.position, target.transform.position) <= weapon.GetRange())
            {
                GetComponent<Mover>().Cancel(); 
                AttackBehaviour();
            }
            else
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f); 
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }

        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
        }

        public void Attack(GameObject combatTarget){
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
            StopAttack();
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public bool CanAttack(GameObject combatTarget){
            if(combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.isDead;

        }
        //Animation event
        void Hit(){
            if(target==null) return;
            target.TakeDamage(weapon.GetDamage());
        }

        void SpawnWeapon(){
            if (weapon == null) return;
            weapon.Spawn(handTransform, GetComponent<Animator>());
        }
    }
}