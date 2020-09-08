using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core{
    public class DestoryAfterEffect : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(!GetComponent<ParticleSystem>().IsAlive()) Destroy(gameObject);
        }
    }

}