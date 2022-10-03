using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class JunkCameraControllerScript : MonoBehaviour
{
    // (1)
    [SerializeField] private CinemachineVirtualCamera sphereCam;
    [SerializeField] private CinemachineVirtualCamera cubeCam;
    [SerializeField] private CinemachineVirtualCamera cylinderCam;
    
    // (2)
    private List<CinemachineVirtualCamera> allMyCameras = new List<CinemachineVirtualCamera>();
    
    // (3)
    private int cameraListIndex = 0;
    
    // (4)
    private Keyboard myKB;


    // Start is called before the first frame update
    void Start()
    {
        
        // (5)
        myKB = Keyboard.current;
        
        // (6)
        allMyCameras.Add(sphereCam);
        allMyCameras.Add(cubeCam);
        allMyCameras.Add(cylinderCam);
        
        // just making sure it works.
        Debug.Log(allMyCameras[0]);
        Debug.Log(allMyCameras[1]);
        Debug.Log(allMyCameras[2]);
    }

    // Update is called once per frame
    void Update()
    {
        // (7)
        if (myKB.spaceKey.wasPressedThisFrame)
        {
            cameraListIndex = (cameraListIndex + 1) % allMyCameras.Count;
            SwitchCamera(allMyCameras[cameraListIndex]);
        }
    }

    // (8)
    void SwitchCamera(CinemachineVirtualCamera activeCamera)
    {

        if (activeCamera.Priority != 10)
        {
            foreach (CinemachineVirtualCamera cammy in allMyCameras)
            {
                cammy.Priority = 0;
            }

            activeCamera.Priority = 10;
            Debug.Log("Current camera: " + activeCamera);
        }
    }
}
