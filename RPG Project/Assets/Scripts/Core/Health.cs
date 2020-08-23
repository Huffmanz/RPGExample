using RPG.Saving;
using UnityEngine;
namespace RPG.Core{
    public class Health : MonoBehaviour, ISaveable {
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
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float) state;
            if(healthPoints == 0 ) Die();
        }
    }
}