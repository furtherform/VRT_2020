using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskController_Slider : MonoBehaviour
{
    public bool Movable;
    public bool CountingTime = false;
    public float SlideLimitL;
    public float SlideLimitR;

    public GameObject Slider;
    public GameObject SliderLight;
    public Material LightCount;
    public Material LightOff;
    public Material LightOn;
    public Material LightRecording;


    public GameObject SliderCollider;
    float ColliderPosOld = .0f;
    float ColliderPosNew = .0f;
    public bool moveL = true;
    public bool moveR = true;
    public bool ResetTest = false;
   
    public enum MoveOptions {noMove, firstMove, moving }
    public MoveOptions moveState = MoveOptions.noMove;


    // Start is called before the first frame update
    void Start()
    {
        Movable = false;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        /*
        eli kun collisio huomataan, ja no new collision, merkitään että törmäsi ekan kerran. jei,
        sit seuraavalla kerralla collisio on päällä, ja on  merkitty et tää on jo uusi, ei tehdä mitään
        Ja kun törmäys loppuu, merkitään et noNewCollision.
        Ei toi toimi. Mitä haluan et yhden kerran huomataan et on nyt törmättiin, sit ajetaan if-skripti. 
        ja sen jälkeen vaikka törmäys jatkuu, sitä ei tehdä. et tarvitaanko kolme statea, newmove, waiting, no-collision.
        tää pitäisi kyl olla tehtävissä kahdellakin muuttujalta. Hermo sentään miksi en aivota tätä nyt. Tauko. Liikuntaa. Huomiseen tjsp.

        */

        if ((Slider.GetComponent<SliderCollision>().Collision == true) && (moveState == MoveOptions.noMove)) {
            moveState = MoveOptions.firstMove;
        }




        if (Slider.GetComponent<SliderCollision>().Collision == true) { Movable = true; } else { Movable = false; moveState = MoveOptions.noMove; }

        if (Movable)
        /* 
        && 
       

        Miksi tää ei toimi? heti kun hetiän ton ineen homma menee rikki?
        //*/
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) ||
                 OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) ||
                 OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) ||
                 OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
            {

                if (ResetTest)
                {
                    ColliderPosNew = Slider.GetComponent<SliderCollision>().collider.gameObject.transform.position.x;
                    ResetTest = false;
                }


                if (moveState == MoveOptions.firstMove)
                {
                    ColliderPosNew = Slider.GetComponent<SliderCollision>().collider.gameObject.transform.position.x;
                    moveState = MoveOptions.moving;
                }


                ColliderPosOld = ColliderPosNew;
                ColliderPosNew = Slider.GetComponent<SliderCollision>().collider.gameObject.transform.position.x;

                if (Slider.transform.position.x > SlideLimitL) { moveL = false; } else moveL = true;
                if (Slider.transform.position.x < SlideLimitR) { moveR = false; } else moveR = true;

                if (!moveL)
                {
                    if (ColliderPosNew - ColliderPosOld < .0f)
                    {
                        Vector3 SlideDelta = new Vector3(ColliderPosNew - ColliderPosOld, 0.0f, 0.0f);
                        Slider.transform.position += SlideDelta;
                    }
                }

                if (!moveR)
                {

                    if (ColliderPosNew - ColliderPosOld > .0f)
                    {
                        Vector3 SlideDelta = new Vector3(ColliderPosNew - ColliderPosOld, 0.0f, 0.0f);
                        Slider.transform.position += SlideDelta;
                    }
                }

                if (moveL && moveR)
                {
                    Vector3 SlideDelta = new Vector3(ColliderPosNew - ColliderPosOld, 0.0f, 0.0f);
                    Slider.transform.position += SlideDelta;
                }
            }
        }


        //SLIDER COLOR CONTROLS


        if (CountingTime)

        {
            SliderLight.GetComponent<MeshRenderer>().material = LightCount;
        } else {
            if (Movable) //(Slider.GetComponent<SliderCollision>().Collision == true)
            {
                SliderLight.GetComponent<MeshRenderer>().material = LightOn;

            } else { SliderLight.GetComponent<MeshRenderer>().material = LightOff; }
        }
        

    }



    

}
