using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat{
        
    public class Projectile : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] float speed = 1f;
        [SerializeField] bool homing = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifetime = 10f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] UnityEvent ProjectileHit;
        Health target = null;
        GameObject instigator = null;
        float damage = 0;
        void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if(target==null || target.isDead) return;
            if(homing && target.isDead){
                transform.LookAt(GetAimLocation());
            }
            
        }

        private void OnTriggerEnter(Collider enemy) {
            if(enemy.GetComponent<Health>() == target){
                if(target.isDead) return;
                enemy.GetComponent<Health>().TakeDamage(damage, instigator);
                if(hitEffect != null) Instantiate(hitEffect, GetAimLocation(),transform.rotation);
                foreach(GameObject destory in destroyOnHit){
                    Destroy(destory);
                }
                Destroy(gameObject, lifeAfterImpact);
                ProjectileHit.Invoke();
            }
        }
        public void SetTarget(Health _target, float _damage, GameObject _instigator){
            target = _target;
            damage = _damage;
            instigator = _instigator;
            Destroy(gameObject, maxLifetime);
        }

        Vector3 GetAimLocation(){
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            if(targetCollider == null) return target.transform.position;
            return target.transform.position + (Vector3.up * (targetCollider.height / 2));
        }
    }

}