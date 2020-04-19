using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position =  target.position;
    }
}
