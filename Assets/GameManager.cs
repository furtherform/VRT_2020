using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ********************* 
    // THIS DRIVES THE VR ADJUSTMENTS
    // *********************



    public GameObject CameraReference; // 2nd camera that feeds the rotation and position changes in VR.
    public GameObject CameraGame; // The camera used to render the scene. Hiearchy under the Holder and HolderRoot.
    public GameObject CameraHolder;
    public GameObject CameraHolderRoot;


 
    public GameObject ConBoxL;
    public GameObject ConBoxR;
    public GameObject ControllerReferenceL;
    public GameObject ControllerReferenceR;

    // adjustments
    public float controllerGain = 1.0f; 
    public float hmdRotGain = 1.0f;
    public float hmdPosGain = 1.0f;

    // Premade mode setup for demo. cold skip these with an ENUM
    public bool NormalMode;
    public bool SlowMode;
    public bool FastMode;

// game logic variables
   public GameObject startBox;
   public bool adaptationStarted = false;

public    Vector3 ConLPosNow;
    Vector3 ConLPosOld;
public    Vector3 ConRPosNow;
    Vector3 ConRPosOld;

    Vector3 HmdPosNow;
    Vector3 HmdPosOld;
    Quaternion HmdRotNow;
    Quaternion HmdRotOld;
    Vector3 CamPosNow;
    Vector3 CamPosOld;

    // public Vector3 DistanceError; 
  


    void Start()
    {
        // Setting the defaults.
        HmdPosNow = CameraReference.transform.position;
        HmdPosOld = CameraReference.transform.position;

        HmdRotNow = CameraReference.transform.rotation;
        HmdRotOld = CameraReference.transform.rotation;

        CamPosNow = CameraGame.transform.position;
        CamPosOld = CameraGame.transform.position;

        ConLPosNow = ControllerReferenceL.transform.position;
        ConLPosOld = ControllerReferenceL.transform.position;
        ConRPosNow = ControllerReferenceR.transform.position;
        ConRPosOld = ControllerReferenceR.transform.position;
        ConBoxL.transform.position = ControllerReferenceL.transform.position;
        ConBoxR.transform.position = ControllerReferenceL.transform.position;

        // setting up the camera holder and root positions so that root is placed at camera position.
        CameraHolder.transform.position += CameraHolder.transform.position - CameraGame.transform.position;
        CameraHolderRoot.transform.position -= CameraHolder.transform.position - CameraGame.transform.position;
    
       

    }

    void Update()
    {

        // **************************
        // VIRTUAL REALITY ADJUSTMENT
        // **************************


        // BUG: the position of camera and holder Root wont stay exactly same for some reason, mainly relationg to rotations.
        // Hopefully neglible. Way better than before. hould be fine when using small values values of rotgain. -0,4>rotgain<-0,4
        // DistanceError = CameraHolderRoot.transform.position - CameraGame.transform.position;

        // CAMERA MOVEMENT
        // adjusting the holder and holder root so that holderRoot is kept on same spot with gameCamera aand adding the adjustment value.
        // thus allowing rotating the camera by rotating the holderRoot

        HmdPosOld = HmdPosNow;
        HmdPosNow = CameraReference.transform.position;
        Vector3 HmdDelta = HmdPosNow - HmdPosOld;
        CameraHolder.transform.position -= HmdDelta;
        CameraHolderRoot.transform.position += HmdDelta;
        CameraHolderRoot.transform.position += hmdPosGain * HmdDelta;
   
        // CAMERA ROTATION
        // this does the rotation of the holder and holder root to adjust rightin regard of the hmd rotation.
        // Slerp means multiplying the rotationdifference by the rotgain variable. min and max are -1 to 1.
        // Practical limits: 
        // 0.5 doubles the rotation, so 90 degree turn sees 180 degree turn. Error prone. Use smaller.
        // -0.5 halves the rotation, so to see 90 degree turn, user has to rotate 180 degrees. Feels odd.

        HmdRotOld = HmdRotNow;
        HmdRotNow = CameraReference.transform.rotation;
        Quaternion rotationDifference = HmdRotNow * Quaternion.Inverse(HmdRotOld);
        Quaternion quatNull = new Quaternion(0f, 0f, 0f, 1f);
        Quaternion rotationChange = Quaternion.SlerpUnclamped(quatNull, rotationDifference, hmdRotGain);
        CameraHolderRoot.gameObject.transform.rotation = CameraHolderRoot.transform.rotation * rotationChange;





        // ********************* 
        // CONTROLLER ADJUSTMENT
        // *********************


        ConLPosOld = ConLPosNow;
        ConLPosNow = ControllerReferenceL.transform.position;
        ConBoxL.transform.position += (ConLPosNow - ConLPosOld) * controllerGain;

        ConRPosOld = ConRPosNow;
        ConRPosNow = ControllerReferenceR.transform.position;
        ConBoxR.transform.position += (ConRPosNow - ConRPosOld) * controllerGain;



        // *********
        // GAMELOGIC
        // *********


        // Not neccessary just yet

        // Setting the default parameters. Ugly. Do ifelse/list instead so many can't be on all the time.
        if (NormalMode)
        {
            controllerGain = 1.0f; // 0.5 is slow, 1.5 is fast;
            hmdRotGain = 0.0f;
            hmdPosGain = 0.0f;
        }

        if (SlowMode)
        {
            controllerGain = 0.5f; // 0.5 is slow, 1.5 is fast;
            hmdRotGain = -0.4f;
            hmdPosGain = 0.0f;
        }

        if (FastMode)
        {
            controllerGain = 1.5f; // 0.5 is slow, 1.5 is fast;
            hmdRotGain = 0.4f;
            hmdPosGain = 0.0f;
        }


        if (!adaptationStarted)
        {
            StartCubeTest cs = startBox.GetComponent<StartCubeTest>();
            adaptationStarted = cs.adaptationStarted;
            ConBoxL.transform.position = ControllerReferenceL.transform.position;
            ConBoxR.transform.position = ControllerReferenceR.transform.position;
        }

    }
}
