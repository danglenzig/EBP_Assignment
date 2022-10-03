using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NewWelcomeSceneManagerScript : MonoBehaviour
{

    private CinemachineVirtualCamera currentCamera = null;
    private int currentCameraNumber = 0;

    [SerializeField] private CinemachineVirtualCamera camera1;
    [SerializeField] private CinemachineVirtualCamera camera2;
    [SerializeField] private CinemachineVirtualCamera camera3;
    [SerializeField] private CinemachineVirtualCamera camera4;

    private List<CinemachineVirtualCamera> allMyCameras = new List<CinemachineVirtualCamera>();
    // Start is called before the first frame update
    void Start()
    {
        allMyCameras.Add(camera1);
        allMyCameras.Add(camera2);
        allMyCameras.Add(camera3);
        allMyCameras.Add(camera4);
        StartCoroutine(WaitThenSwitch(Random.Range(3, 7)));
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCameraNumber == 1)
        {
            ActivateCamera(camera1);
        }
        
        if (currentCameraNumber == 2)
        {
            ActivateCamera(camera2);
        }
        
        if (currentCameraNumber == 3)
        {
            ActivateCamera(camera3);
        }
        
        if (currentCameraNumber == 4)
        {
            ActivateCamera(camera4);
        }
    }

    private IEnumerator WaitThenSwitch(float waitSeconds)
    {
        yield return new WaitForSecondsRealtime(waitSeconds);
        currentCameraNumber = Random.Range(1, 5);
        StartCoroutine(WaitThenSwitch(Random.Range(3, 7)));
    }

    private void ActivateCamera(CinemachineVirtualCamera activeCam)
    {
        foreach (CinemachineVirtualCamera cammy in allMyCameras)
        {
            if(cammy != activeCam)
            cammy.Priority = 0;
        }

        activeCam.Priority = 10;
    }
    
    
}
