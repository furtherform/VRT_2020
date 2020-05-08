using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public GameObject HandTrackReference;
    //public GameObject ControllerHolder;
    Vector3 positionNow;
    Vector3 positionPrevious;
    public float movementGain = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        positionPrevious = HandTrackReference.transform.position;
        positionNow = HandTrackReference.transform.position;
        this.transform.position = HandTrackReference.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        positionPrevious = positionNow;
        positionNow = HandTrackReference.transform.position;// - HandTrackaReference.transform.up * 3);
        Vector3 positionDifference = positionNow - positionPrevious;
        this.transform.position += positionDifference * movementGain;
        //this.transform.position = HandTrackReference.transform.position; 
    }
}
