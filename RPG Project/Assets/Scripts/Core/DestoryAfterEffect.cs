using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core{
    public class DestoryAfterEffect : MonoBehaviour
    {
        [SerializeField] GameObject targetToDestroy = null;

        // Update is called once per frame
        void Update()
        {
            if(targetToDestroy != null)
            {
                Destroy(targetToDestroy);
            }
            else
            {
                if (!GetComponent<ParticleSystem>().IsAlive()) Destroy(gameObject);
            }
        }
    }

}