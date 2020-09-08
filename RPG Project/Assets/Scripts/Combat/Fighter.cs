using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Combat{

    public class Fighter : MonoBehaviour, IAction, ISaveable{
    
        Health target;
        [SerializeField] float timeBetweenAttacks = 1f;

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        Weapon currentWeapon = null;

        
        float timeSinceLastAttack = Mathf.Infinity;

        public void Start() {
            if(defaultWeapon != null){
                EquipWeapon(defaultWeapon);
            }
            
        }
        public void Update(){
            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return;
            if(target.isDead) return;
            if(Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.GetRange())
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
            if(currentWeapon.HasProjectile()){
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target.GetComponent<RPG.Resources.Health>(), gameObject);
            }else{
                target.TakeDamage(currentWeapon.GetDamage(), gameObject);
            }
        }

        //Animation event
        void Shoot(){
            Hit();
        }   

        public void EquipWeapon(Weapon weapon){
            currentWeapon = weapon;
            if(weapon==null) return;    
            weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
        }

        public Health GetTarget(){
            return target;
        }
        public object CaptureState()
        {
            if(currentWeapon == null) return defaultWeapon.name;
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            Weapon weapon = UnityEngine.Resources.Load<Weapon>((string) state);
            EquipWeapon(weapon);
        }
    }
}