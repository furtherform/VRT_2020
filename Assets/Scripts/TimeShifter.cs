using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeShifter : MonoBehaviour
{

   
    public GameObject cameraReference;
    public GameObject cameraHolder;
    public float movementGain = 1.0f;
    public float rotationGain = 1.0f;

    Vector3 positionNow;
    Vector3 positionPrevious;
    Quaternion rotationNow;
    Quaternion rotationPrevious;
    Quaternion cameraHolderRotation;

    // Start is called before the first frame update
    void Awake()
    {
        positionPrevious = cameraReference.transform.position;
        positionNow = cameraReference.transform.position;
        rotationPrevious = cameraReference.transform.rotation;
        rotationNow = cameraReference.transform.rotation;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
  

    positionPrevious = positionNow;
    positionNow = (cameraReference.transform.position - cameraReference.transform.up*3); // why up*3? copied from reference, does make it better - why?
    Vector3 positionDifference = positionNow - positionPrevious;
    cameraHolder.transform.position += positionDifference * movementGain;


     

        rotationPrevious = rotationNow;
        rotationNow = cameraReference.transform.rotation;
        Quaternion rotationDifference = rotationNow * Quaternion.Inverse(rotationPrevious);
        Quaternion quatNull = new Quaternion(0f, 0f, 0f, 1f);
        Quaternion rotationChange = Quaternion.SlerpUnclamped(quatNull, rotationDifference, rotationGain);
        cameraHolder.transform.rotation = cameraHolder.transform.rotation * Quaternion.Inverse(rotationChange);
        // lisäsin quaternion.inversen. Nyt toimii. positiiviset 0-1 hidastavat, 1 ei liiku mihinkään. negatiiviset arvot nopeuttavat.
        // hienovaraista, 0,2 on maksimi jossa ei tunnu oudota.
        // nopeutuksesta, luontevaa, -0.4 ok... arvolla -1,6 180 asteen kääntymä kääntää 360 
        // -0,5, 120 astetta vastaa 180 astetta. Mitenkähän tän matikan nyt laskisi.

        // refuarvot nopeutukselle: movement gain 0,4  , rotation gain -0.5.
        // refuarvot hidastukselle: movement gain -0.4 , rotation gain 0,4



    





    }
}
