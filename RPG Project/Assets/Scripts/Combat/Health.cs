using UnityEngine;
namespace RPG.Combat{
    public class Health : MonoBehaviour {
        [SerializeField] float healthPoints = 100f;
        public bool isDead = false;
        public void TakeDamage(float damage){
            healthPoints = Mathf.Max(healthPoints-damage, 0);
            if(healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if(!isDead){
                GetComponent<Animator>().SetTrigger("Death");
                isDead = true;
            }
        }
    }
}