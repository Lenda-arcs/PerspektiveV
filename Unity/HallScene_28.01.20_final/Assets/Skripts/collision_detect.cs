using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision_detect : MonoBehaviour
{
    public bool obj_collision;

    private void Start()
    {
        obj_collision = false;
        
    }

    void OnCollisionEnter(Collision collision)
    {
        obj_collision = true;
    }

    void OnCollisionExit(Collision other)
    {
        obj_collision = false;
    }


}
