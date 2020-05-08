using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderCollision : MonoBehaviour
{
    //collision is detected here. Must have this script on this gameobject. Sucks but easier this way.

    public bool Collision = false;
    public GameObject collider;

 

    void OnTriggerEnter(Collider collision)
    {
    //       Debug.Log("collided");
        if (collision.gameObject.tag == "handController")
        {
            Collision = true;
            collider = collision.gameObject;
           // Debug.Log("collidedStart");


        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "handController")
        {
            Collision = false;
            //Debug.Log("collidedEnd");


        }
    }
}
