using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraMoveTimeShift : MonoBehaviour
{
public float SlowDownPercentage = 0.5f;
public GameObject RealCamera;
public GameObject CameraHolder;
public Vector3 RC_Position;
public Quaternion RC_Rotation;
public Vector3 RC_Position1;
public Quaternion RC_Rotation1;
    public float speed = 5f;
    Quaternion RotDiff;
    Quaternion CamHolRot;
    Quaternion RotStep1;
    Quaternion RotStep2;
    Quaternion QuatNull = new Quaternion(0f, 0f, 0f, 1f);


    // Start is called before the first frame update
    void Start()
    {

        //InputTracking.GetLocalPosition(XR.XRNode.CenterEye); //RealCamera.transform.position; // UnityEngine.XR.XRNodeState.position;
        // UnityEngine.XR.XRNodeState.rotation;
        //VR.InputTracking.GetLocalPosition(VRNode.CenterEye); 
        //VR.InputTracking.GetLocalRotation(VRNode.CenterEye);
        //  UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        
        
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("1. Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        }

        var generics = new List<UnityEngine.XR.InputDevice>();

        UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.Generic, generics);
           foreach (var device in generics)
           {
               Debug.Log(string.Format("2. Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
           }

      if(generics.Count == 1)
        {
            UnityEngine.XR.InputDevice device = generics[0];
            Vector3 pos;
          //   = device.TryGetFeatureValue(CommonUsages.devicePosition, out pos);
            //; /; UnityEngine.XR.InputTracking.GetLocalPosition(device);
            Debug.Log("one VR-display detected");
        }
      else if (generics.Count >1)
        {
            Debug.Log("more than one VR-display");
        }

        //  RC_Position = device.TryGetFeatureValue(CommonUsages.devicePosition, out pos); //RealCamera.transform.position; 
        RC_Position = RealCamera.transform.position;
        RC_Rotation = RealCamera.transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CamHolRot = CameraHolder.transform.rotation;

        // asetamme edellisen cyclen arvot muuttujiin 1 jonka jäleen haemme uudeet päivitet arvot.
        RC_Position1 = RC_Position;
        RC_Rotation1 = RC_Rotation;
        RC_Position = RealCamera.transform.position;
        RC_Rotation = RealCamera.transform.rotation;

        //laskemmme erotuksen edellisen ja nykyisen cyclen rotaatioarvoille.
        RotDiff = RC_Rotation * Quaternion.Inverse(RC_Rotation1);

        //kerromme erotuksen prosenttimuuttujalla ja asetamme sen camholderin rotaatioksi.
        RotStep2 = RotDiff * Quaternion.SlerpUnclamped(QuatNull, RotDiff, SlowDownPercentage);
        CameraHolder.transform.rotation = CamHolRot * RotStep2;
        //CameraHolder.transform.rotation = CamHolRot * Quaternion.Inverse(RotStep2);

        // miksi tää ei pysähdy koskaan?





        // CameraHolder.transform.rotation = Quaternion.RotateTowards(CamHolRot, (CamHolRot * RotDiff), speed * Time.deltaTime);

        //CameraHolder.GetComponent.transform.rotation = new Quaternion(RC_Rotation);
        //CameraHolder.GetComponent(transform.rotation) = 

        //((RC_Rotation.x + RC_Rotation1.x) * SlowDownPercentage)
        //CameraHolder.transform.rotation = new Quaternion(RC_Rotation.x, RC_Rotation.y, RC_Rotation.z, RC_Rotation.w) ;
        /*  CameraHolder.transform.rotation = new Quaternion(
               ((RC_Rotation.x + RC_Rotation1.x) * SlowDownPercentage),
               ((RC_Rotation.y + RC_Rotation1.y) * SlowDownPercentage),
               ((RC_Rotation.z + RC_Rotation1.z) * SlowDownPercentage),
               ((RC_Rotation.w + RC_Rotation1.w) * SlowDownPercentage)
               );
               */
    }
}
