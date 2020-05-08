using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest3 : MonoBehaviour
{
    public GameObject Target; // here be the real cam
    public GameObject TargetRef; // object we get VR rotation and position values from.
    public GameObject Holder1;
    public GameObject Holder2;
    Quaternion TargetRefRotNow;
    Quaternion TargetRefRotOld;
    bool HolderTransfer = false;
    

    // Start is called before the first frame update
    void Start()
    {
        TargetRefRotNow = TargetRef.transform.rotation; // defaults at beginning.
        TargetRefRotOld = TargetRef.transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        TargetRefRotOld = TargetRefRotNow;
        TargetRefRotNow = TargetRef.transform.rotation;
        Quaternion rotdif = TargetRefRotNow * Quaternion.Inverse(TargetRefRotOld);

        Vector3 DeltaOriginToCam = Target.transform.position - Holder2.transform.position;
        if (!HolderTransfer)
        {
            Holder2.transform.position -= DeltaOriginToCam;
            Holder1.transform.position += DeltaOriginToCam;
            HolderTransfer = true;
        }

        Holder1.transform.rotation = Holder1.transform.rotation * rotdif;



    }
}
