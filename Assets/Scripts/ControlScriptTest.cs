using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScriptTest : MonoBehaviour
{
    public GameObject ControllerReference;
    public bool StartAdaptation = false;
    public float MovementGain = 1.0f;
    Vector3 PosOld;
    Vector3 PosNow;

    void Start()
    {

        PosNow = this.transform.position;
        PosOld = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        PosOld = PosNow;
        PosNow = ControllerReference.transform.position;
        Vector3 PosDif = PosNow - PosOld;
        this.transform.position += PosDif*MovementGain;
    }
}
