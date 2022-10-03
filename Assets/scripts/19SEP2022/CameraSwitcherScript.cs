using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraSwitcherScript : MonoBehaviour
{
    /*

    private CinemachineVirtualCamera currentLiveCamera = null;

    private List<CinemachineVirtualCamera> allMyCameras = new List<CinemachineVirtualCamera>();
    //private List<GameObject> allMyCrosshairs = new List<GameObject>();

    [SerializeField] private GameObject crosshairPanel;
    public float crosshairDelay = .05f;

    [SerializeField] private CinemachineVirtualCamera BluePlayerA_3PCam;
    [SerializeField] private CinemachineVirtualCamera GreenPlayerA_3PCam;
    [SerializeField] private CinemachineVirtualCamera RedPlayerA_3PCam;
    [SerializeField] private CinemachineVirtualCamera PinkPlayerA_3PCam;
    [SerializeField] private CinemachineVirtualCamera projectileCam;

    [SerializeField] private GameObject bluePlayerObject;
    [SerializeField] private GameObject greenPlayerObject;
    [SerializeField] private GameObject redPlayerObject;
    [SerializeField] private GameObject pinkPlayerObject;

    private CinemachineVirtualCamera BluePlayerA_FPSCam = null;
    private CinemachineVirtualCamera GreenPlayerA_FPSCam = null;
    private CinemachineVirtualCamera RedPlayerA_FPSCam = null;
    private CinemachineVirtualCamera PinkPlayerA_FPSCam = null;

    private void Awake()
    {

        BluePlayerA_FPSCam = bluePlayerObject.GetComponentInChildren<CinemachineVirtualCamera>();
        GreenPlayerA_FPSCam = greenPlayerObject.GetComponentInChildren<CinemachineVirtualCamera>();
        RedPlayerA_FPSCam = redPlayerObject.GetComponentInChildren<CinemachineVirtualCamera>();
        PinkPlayerA_FPSCam = pinkPlayerObject.GetComponentInChildren<CinemachineVirtualCamera>();
        
        allMyCameras.Add(BluePlayerA_3PCam);
        allMyCameras.Add(BluePlayerA_FPSCam);
        allMyCameras.Add(GreenPlayerA_3PCam);
        allMyCameras.Add(GreenPlayerA_FPSCam);
        allMyCameras.Add(RedPlayerA_3PCam);
        allMyCameras.Add(RedPlayerA_FPSCam);
        allMyCameras.Add(PinkPlayerA_3PCam);
        allMyCameras.Add(PinkPlayerA_FPSCam);
        allMyCameras.Add(projectileCam);
        crosshairPanel.SetActive(false);
    }
    
    
    private void Start()
    {
        ResetAllCameras();
    }

    private void Update()
    {
        if(SceneDirectorScript.currentPlayerNumber == 0)
        {
            if (SceneDirectorScript.thirdPersonModeOn)
            {
                if (BluePlayerA_3PCam.Priority != 10)
                {
                    crosshairPanel.SetActive(false);
                    ResetAllCameras();
                    BluePlayerA_3PCam.Priority = 10;
                }
            }

            if (SceneDirectorScript.fPSModeOn)
            {
                if (BluePlayerA_3PCam.Priority != 10)
                {
                    crosshairPanel.SetActive(true);
                    ResetAllCameras();
                    BluePlayerA_FPSCam.Priority = 10;
                }
            }

            if (SceneDirectorScript.projectileModeOn)
            {
                if (projectileCam.Priority != 10)
                {
                    crosshairPanel.SetActive(false);
                    ResetAllCameras();
                    projectileCam.Priority = 10;
                }
            }
        }

        if (SceneDirectorScript.currentPlayerNumber == 1)
        {
            if (SceneDirectorScript.thirdPersonModeOn)
            {
                if (GreenPlayerA_3PCam.Priority != 10)
                {
                    crosshairPanel.SetActive(false);
                    ResetAllCameras();
                    GreenPlayerA_3PCam.Priority = 10;
                }
            }

            if (SceneDirectorScript.fPSModeOn)
            {
                if (GreenPlayerA_3PCam.Priority != 10)
                {
                    crosshairPanel.SetActive(true);
                    ResetAllCameras();
                    GreenPlayerA_FPSCam.Priority = 10;
                }
            }

            if (SceneDirectorScript.projectileModeOn)
            {
                if (projectileCam.Priority != 10)
                {
                    crosshairPanel.SetActive(false);
                    ResetAllCameras();
                    projectileCam.Priority = 10;
                }
            }
        }

        if (SceneDirectorScript.currentPlayerNumber == 2)
        {
            if (SceneDirectorScript.thirdPersonModeOn)
            {
                if (RedPlayerA_3PCam.Priority != 10)
                {
                    crosshairPanel.SetActive(false);
                    ResetAllCameras();
                    RedPlayerA_3PCam.Priority = 10;
                }
            }

            if (SceneDirectorScript.fPSModeOn)
            {
                if (RedPlayerA_3PCam.Priority != 10)
                {
                    crosshairPanel.SetActive(true);
                    ResetAllCameras();
                    RedPlayerA_FPSCam.Priority = 10;
                }
            }

            if (SceneDirectorScript.projectileModeOn)
            {
                if (projectileCam.Priority != 10)
                {
                    crosshairPanel.SetActive(false);
                    ResetAllCameras();
                    projectileCam.Priority = 10;
                }
            }
        }

        if (SceneDirectorScript.currentPlayerNumber == 3)
        {
            if (SceneDirectorScript.thirdPersonModeOn)
            {
                if (PinkPlayerA_3PCam.Priority != 10)
                {
                    crosshairPanel.SetActive(false);
                    ResetAllCameras();
                    PinkPlayerA_3PCam.Priority = 10;
                }
            }

            if (SceneDirectorScript.fPSModeOn)
            {
                if (PinkPlayerA_3PCam.Priority != 10)
                {
                    crosshairPanel.SetActive(true);
                    ResetAllCameras();
                    PinkPlayerA_FPSCam.Priority = 10;
                }
            }

            if (SceneDirectorScript.projectileModeOn)
            {
                if (projectileCam.Priority != 10)
                {
                    crosshairPanel.SetActive(false);
                    ResetAllCameras();
                    projectileCam.Priority = 10;
                }
            }
        }
    }

    void ResetAllCameras()
    {
        foreach (var cam in allMyCameras)
        {
            cam.Priority = 0;
        }
    }
    */
}
