using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using Utils;

namespace RPG.Combat{

    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider{

        Health target;
        [SerializeField] float timeBetweenAttacks = 1f;

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        WeaponConfig currentWeaponConfig = null;
        LazyValue<Weapon> currentWeapon;


        float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            return EquipWeapon(defaultWeapon);
        }

        public void Start() {
            EquipWeapon(currentWeaponConfig);
            currentWeapon.ForceInit();
            if(defaultWeapon != null){
                EquipWeapon(defaultWeapon);
            }

        }
        public void Update(){
            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return;
            if(target.isDead) return;
            if(IsInRange(target.transform))
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

        private bool IsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, target.transform.position) <= currentWeaponConfig.GetRange();
        }

        public bool CanAttack(GameObject combatTarget){
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) &&
                !IsInRange(combatTarget.transform)) return false;
            if(combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.isDead;

        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetDamage();
            }

        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
        }
        //Animation event
        void Hit(){

            if(target==null) return;
            if (currentWeapon.value == null) return;
            currentWeapon.value.OnHit();
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            Debug.Log(gameObject.name + " Deals " + damage + " to " + target.name);
            if (currentWeaponConfig.HasProjectile()){
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target.GetComponent<RPG.Attributes.Health>(), gameObject, damage);
            }else{
                target.TakeDamage(damage, gameObject);
            }
        }

        //Animation event
        void Shoot(){
            Hit();
        }

        public Weapon EquipWeapon(WeaponConfig weapon){ 
	         
           if(weapon==null) return new Weapon();
            currentWeaponConfig = weapon;
           currentWeapon.value = currentWeaponConfig.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
           return currentWeapon.value;
        }

        public Health GetTarget(){
            return target;
        }
        public object CaptureState()
        {
            if(currentWeaponConfig == null) return defaultWeapon.name;
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>((string) state);
            currentWeapon.value = EquipWeapon(weapon);
        }


    }
}
