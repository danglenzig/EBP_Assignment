using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionManagerScript : MonoBehaviour
{
    private GameObject objectUnderControl = null;
    private CinemachineVirtualCamera currentFPSCamera = null;
    private CinemachineVirtualCamera current3PCamera = null;
    private Keyboard myKB;
    private Mouse myMouse;
    private Rigidbody myRB;
    private Vector3 jumpVector;
    private GameObject projectileObject = null;

    private bool isGrounded = true;

    public float rotateSpeed = 1;
    public float verticalJumpStrength = 10;
    public float horizontalJumpStrength = 10;
    public float attackModeRotateSpeed = .5f;
    public float projectileSpeed = 0.1f;
    
    private GameObject bluePlayerObject;
    private GameObject greenPlayerObject;
    private GameObject redPlayerObject;
    private GameObject pinkPlayerObject;

    [SerializeField] private CinemachineVirtualCamera blue3PCam;
    private CinemachineVirtualCamera blueFPSCam;
    [SerializeField] private CinemachineVirtualCamera green3PCam;
    private CinemachineVirtualCamera greenFPSCam;
    [SerializeField] private CinemachineVirtualCamera red3PCam;
    private CinemachineVirtualCamera redFPSCam;
    [SerializeField] private CinemachineVirtualCamera pink3PCam;
    private CinemachineVirtualCamera pinkFPSCam;
    [SerializeField] private CinemachineVirtualCamera projectileCam;
    private List<CinemachineVirtualCamera> allMyCinemachineVirtualCameras = new List<CinemachineVirtualCamera>();


    //private List<GameObject> allMyPlayerObjects;

    // Start is called before the first frame update
    void Start()
    {
        myKB = Keyboard.current;
        myMouse = Mouse.current;

        bluePlayerObject = SceneDirectorScript.bluePlayerObject;
        greenPlayerObject = SceneDirectorScript.greenPlayerObject;
        redPlayerObject = SceneDirectorScript.redPlayerObject;
        pinkPlayerObject = SceneDirectorScript.pinkPlayerObject;
        projectileObject = SceneDirectorScript.projectileObject;

        blueFPSCam = bluePlayerObject.GetComponentInChildren<CinemachineVirtualCamera>();
        greenFPSCam = greenPlayerObject.GetComponentInChildren<CinemachineVirtualCamera>();
        redFPSCam = redPlayerObject.GetComponentInChildren<CinemachineVirtualCamera>();
        pinkFPSCam = pinkPlayerObject.GetComponentInChildren<CinemachineVirtualCamera>(); 
            
        allMyCinemachineVirtualCameras.Add(blue3PCam);
        allMyCinemachineVirtualCameras.Add(blueFPSCam);
        allMyCinemachineVirtualCameras.Add(green3PCam);
        allMyCinemachineVirtualCameras.Add(greenFPSCam);
        allMyCinemachineVirtualCameras.Add(red3PCam);
        allMyCinemachineVirtualCameras.Add(redFPSCam);
        allMyCinemachineVirtualCameras.Add(pink3PCam);
        allMyCinemachineVirtualCameras.Add(pinkFPSCam);
        allMyCinemachineVirtualCameras.Add(projectileCam);
        
        ResetAllCameras();
    }

    void ResetAllCameras()
    {
        foreach (var cam in allMyCinemachineVirtualCameras)
        {
            if (cam.Priority != 0)
            {
                cam.Priority = 0;
            }
        }
    }

    void ActivateCamera(CinemachineVirtualCamera cam)
    {
        if (cam.Priority != 10)
        {
            cam.Priority = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (myKB.shiftKey.isPressed && myKB.oKey.wasPressedThisFrame)
        {
            foreach (var cam in allMyCinemachineVirtualCameras)
            {
                //Debug.Log(cam.name + " Priority: " + cam.Priority);
            }
            Debug.Log("Player " + objectUnderControl.name);
            Debug.Log("3P Cam: "+current3PCamera.name + " priority = " + current3PCamera.Priority);
            Debug.Log("FPS Cam:  " + currentFPSCamera + " priority = " + current3PCamera.Priority);
            //Debug.Log("3P Mode: " + SceneDirectorScript.thirdPersonModeOn);
            //Debug.Log("FPS Mode " + SceneDirectorScript.fPSModeOn);
            //Debug.Log("Projectile mode " + SceneDirectorScript.projectileModeOn);
            //Debug.Log("###################################################");
        }
        
        if (SceneDirectorScript.currentPlayerNumber == 0)
        {
            if (objectUnderControl != bluePlayerObject)
            {
                objectUnderControl = bluePlayerObject;
            }
                
            if (current3PCamera != blue3PCam)
            {
                current3PCamera = blue3PCam;
            }
            if (currentFPSCamera != blueFPSCam)
            {
                currentFPSCamera = blueFPSCam;
            }
        }
        
        if (SceneDirectorScript.currentPlayerNumber == 1)
        {
            if (objectUnderControl != greenPlayerObject)
            {
                objectUnderControl = greenPlayerObject;
            }
            if (current3PCamera != green3PCam)
            {
                current3PCamera = green3PCam;
            }
            if (currentFPSCamera != greenFPSCam)
            {
                currentFPSCamera = greenFPSCam;
            }
            
        }
        if (SceneDirectorScript.currentPlayerNumber == 2)
        {
            if (objectUnderControl != redPlayerObject)
            {
                objectUnderControl = redPlayerObject;
            }
            if (current3PCamera != red3PCam)
            {
                current3PCamera = red3PCam;
            }
            if (currentFPSCamera != redFPSCam)
            {
                currentFPSCamera = redFPSCam;
            }
        }
        if (SceneDirectorScript.currentPlayerNumber == 3)
        {
            if (objectUnderControl != pinkPlayerObject)
            {
                objectUnderControl = pinkPlayerObject;
            }
            if (current3PCamera != pink3PCam)
            {
                current3PCamera = pink3PCam;
            }
            if (currentFPSCamera != pinkFPSCam)
            {
                currentFPSCamera = pinkFPSCam;
            }
        }
        
        if (myRB != objectUnderControl.GetComponent<Rigidbody>())
        {
            myRB = objectUnderControl.GetComponent<Rigidbody>();
        }

        if (SceneDirectorScript.thirdPersonModeOn)
        {
            ResetAllCameras();
            ActivateCamera(current3PCamera);
        }

        if (SceneDirectorScript.fPSModeOn)
        {
            ResetAllCameras();
            ActivateCamera(currentFPSCamera);
        }

        if (SceneDirectorScript.projectileModeOn)
        {
            ResetAllCameras();
            ActivateCamera(projectileCam);
        }
        
        /////////////////////////////////
        // controls for each game mode //
        /////////////////////////////////
        /*

        */
        

        if (myKB.shiftKey.isPressed && myKB.pKey.wasPressedThisFrame)
        {
            if (SceneDirectorScript.thirdPersonModeOn)
            {
                SceneDirectorScript.thirdPersonModeOn = false;
                SceneDirectorScript.fPSModeOn = true;
                SceneDirectorScript.projectileModeOn = false;
            } else if (SceneDirectorScript.fPSModeOn)
            {
                SceneDirectorScript.thirdPersonModeOn = true;
                SceneDirectorScript.fPSModeOn = false;
                SceneDirectorScript.projectileModeOn = false;
                SceneDirectorScript.currentPlayerNumber = (SceneDirectorScript.currentPlayerNumber + 1) %
                                                          SceneDirectorScript.numberOfPlayers;
            }
        }

        if (myKB.spaceKey.wasPressedThisFrame)
        {
            if (SceneDirectorScript.thirdPersonModeOn)
            {
                // jump
            }

            if (SceneDirectorScript.fPSModeOn)
            {
                InputSystem.DisableDevice(myKB);
                projectileObject.SetActive(true);
                projectileObject.transform.position = objectUnderControl.transform.position;
                StartCoroutine(FireProjectile(objectUnderControl));
            }
        }
    }

    private IEnumerator FireProjectile(GameObject sourceObject)
    {
        // activate, position and move the projectile
        yield return new WaitForSecondsRealtime(4f);
        InputSystem.EnableDevice(myKB);
        SceneDirectorScript.currentPlayerNumber =
            (SceneDirectorScript.currentPlayerNumber + 1) % SceneDirectorScript.numberOfPlayers;
        SceneDirectorScript.fPSModeOn = false;
        SceneDirectorScript.projectileModeOn = false;
        SceneDirectorScript.thirdPersonModeOn = true;
    }
}
