using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        // Update is called once per frame
        void LateUpdate()
        {
            transform.position =  target.position;
        }
    }

}
