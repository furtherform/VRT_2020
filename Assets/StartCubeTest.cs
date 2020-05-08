using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCubeTest : MonoBehaviour
{
    public bool adaptationStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter (Collider collision)
    {
        if (collision.gameObject.tag == "handController")
        {
            adaptationStarted = true;
            //Debug.Log("tormattu");
        }
    }
}
