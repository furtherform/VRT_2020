using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest2 : MonoBehaviour
{
    public GameObject Target;
    public GameObject TargetPosition;
    Quaternion TargetRotNow;
    Quaternion TargetRotOld;
    public Vector3 rotateVector;


    // Start is called before the first frame update
    void Start()
    {
        TargetRotNow = Target.transform.rotation;
        TargetRotOld = Target.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        Target.transform.position = TargetPosition.transform.position;
        TargetRotOld = TargetRotNow;
        TargetRotNow = Target.transform.rotation;

        Quaternion rotdif = TargetRotNow * Quaternion.Inverse(TargetRotOld);

        // pitää laskea onko paikallinen rotaatio muuttunut (hmd käännetty)
        // jos on, sit muutetaan sijaintia tällä
        // eli jotenkin pitää saada selville erot local spacen rotaatioista
        // ja worldspace rotaatioista
        // ja tätä käännetään vain jos aiemman local space rotation on muuttunut
        // mut muuten ei.
        // jos hakee sen refun toisesta objektista joka ei siirry tän mukaan?
        // jeah, ton pitäisi toimia. Eli refukamera (poissa hierarkiasta) josta rotaatio-arvot
        // ja sit realcamera hierarkiassa joka sit rotatoidaan ton perusteella.
        // mut sen refutargetin pitää olla kokoajan samassa sijainnissa ton kaa.


         rotateVector = rotdif * Vector3.up;
        float rotDelta = Quaternion.Angle(TargetRotOld, TargetRotNow);
        Vector3 positionDif = this.transform.position - TargetPosition.transform.position;
        //Vector3 OrientDif = rotDelta * Vector3.zer;
        this.transform.RotateAround(TargetPosition.transform.position, positionDif, rotDelta);
       // this.transform.rotation = this.transform.rotation * rotdif;
    



    }
}
