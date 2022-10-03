using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class SceneDirector26SEP2022 : MonoBehaviour
{
    
    [SerializeField] private GameObject bluePlayerObject;
    [SerializeField] private CinemachineVirtualCamera bluePlayer3PCam;
    [SerializeField] private CinemachineVirtualCamera bluePlayerFPSCam = null;
    
    [SerializeField] private GameObject greenPlayerObject;
    [SerializeField] private CinemachineVirtualCamera greenPlayer3PCam;
    [SerializeField] private CinemachineVirtualCamera greenPlayerFPSCam = null;
    
    [SerializeField] private GameObject redPlayerObject;
    [SerializeField] private CinemachineVirtualCamera redPlayer3PCam;
    [SerializeField] private CinemachineVirtualCamera redPlayerFPSCam = null;
    
    [SerializeField] private GameObject pinkPlayerObject;
    [SerializeField] private CinemachineVirtualCamera pinkPlayer3PCam;
    [SerializeField] private CinemachineVirtualCamera pinkPlayerFPSCam = null;
    
    [SerializeField] private GameObject projectileObject;
    [SerializeField] private CinemachineVirtualCamera projectileCam;
    
    [SerializeField] private PostProcessVolume myPP;
    [SerializeField] private GameObject crosshairUIPanel;

    [SerializeField] private GameObject jumpStrengthLabel;
    [SerializeField] private TMP_Text jumpStrengthText;

    [SerializeField] private GameObject blueLightning;
    [SerializeField] private GameObject greenLightning;
    [SerializeField] private GameObject redLightning;
    [SerializeField] private GameObject pinkLightning;
    [SerializeField] private GameObject projectileLightning;
    [SerializeField] private GameObject gameStatePanel;
    [SerializeField] private TMP_Text currentPlayerText;
    [SerializeField] private TMP_Text currentHPText;
    [SerializeField] private GameObject StartUIPanelObject;
    
    
    public float projectileSpeed = 1f;
    public float aimSpeed = .05f;
    public float rotateSpeed = .3f;
    public float verticalJump = 100f;
    public float horizontalJump = 100f;
    public int maxHP = 5; 
   
    private GameObject objectUnderControl = null;
    private Rigidbody currentRigidBody;
    private CinemachineVirtualCamera currentFPSCam = null;
    private CinemachineVirtualCamera current3PCam = null;
    private Animator currentAnimator = null;
    private Animator blueAnimator = null;
    private Animator greenAnimator = null;
    private Animator redAnimator = null;
    private Animator pinkAnimator = null;
    private Keyboard myKB;
    private bool isIntro = true;

    public static string currentMode = "ThirdPerson";
    public static int numberOfPlayers = 4;
    public static int currentPlayerNumber = 0;
    public static float jumpMultiplier = 1;
    public static bool projectileMode = false;
    public static bool fPSMode = false;
    public static bool thirdPersonMode = true;
    public static bool playerIsGrounded = true; // player trigger collider's "OnEnter()" will look for "Ground" tag and set this
    public static bool isCrouched = false;
    public static bool bluePlayerDead = false;
    public static bool greenPlayerDead = false;
    public static bool redPlayerDead = false;
    public static bool pinkPlayerDead = false;
    public static bool reactSequence = false;

    public static int bluePlayerHP = 0;
    public static int greenPlayerHP = 0;
    public static int redPlayerHP = 0;
    public static int pinkPlayerHP = 0;

    private List<CinemachineVirtualCamera> allMyCinemachineVirtualCameras = new List<CinemachineVirtualCamera>();
    private List<GameObject> allMyLightningFX = new List<GameObject>();
    private List<GameObject> allMyUIElements = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        allMyCinemachineVirtualCameras.Add(bluePlayer3PCam);
        allMyCinemachineVirtualCameras.Add(bluePlayerFPSCam);
        allMyCinemachineVirtualCameras.Add(greenPlayer3PCam);
        allMyCinemachineVirtualCameras.Add(greenPlayerFPSCam);
        allMyCinemachineVirtualCameras.Add(redPlayer3PCam);
        allMyCinemachineVirtualCameras.Add(redPlayerFPSCam);
        allMyCinemachineVirtualCameras.Add(pinkPlayer3PCam);
        allMyCinemachineVirtualCameras.Add(pinkPlayerFPSCam);
        // add all the other player cams
        allMyCinemachineVirtualCameras.Add(projectileCam);
        // add scene cams
        
        // add UI elements to list
        allMyUIElements.Add(crosshairUIPanel);
        allMyUIElements.Add(jumpStrengthLabel);
        allMyUIElements.Add(gameStatePanel);
        allMyUIElements.Add(StartUIPanelObject);
        
        allMyLightningFX.Add(blueLightning);
        allMyLightningFX.Add(greenLightning);
        allMyLightningFX.Add(redLightning);
        allMyLightningFX.Add(pinkLightning);
        allMyLightningFX.Add(projectileLightning);
        
        myKB = Keyboard.current;

        projectileObject.SetActive(false);
        
        bluePlayerHP = maxHP;
        greenPlayerHP = maxHP;
        redPlayerHP = maxHP;
        pinkPlayerHP = maxHP;

        blueAnimator = bluePlayerObject.GetComponent<Animator>();
        greenAnimator = greenPlayerObject.GetComponent<Animator>();
        redAnimator = redPlayerObject.GetComponent<Animator>();
        pinkAnimator = pinkPlayerObject.GetComponent<Animator>();
        
        blueLightning.SetActive(false);
        greenLightning.SetActive(false);
        redLightning.SetActive(false);
        pinkLightning.SetActive(false);
        ClearUI();
    }

    void Update()
    {

        if (isIntro)
        {
            ActivateSoloUIPanel(StartUIPanelObject);
            Time.timeScale = 0;
            if (numberOfPlayers == 2)
            {
                pinkPlayerObject.SetActive(false);
                redPlayerObject.SetActive(false);
            }

            if (numberOfPlayers == 3)
            {
                pinkPlayerObject.SetActive(false);
            }
        } else StartUIPanelObject.SetActive(false);

        if (myKB.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }

        if (myKB.shiftKey.isPressed && myKB.pKey.wasPressedThisFrame)
        {
            ClearUI();
            AdvanceGame();
        }

        if (myKB.shiftKey.isPressed && myKB.oKey.wasPressedThisFrame)
        {
            Debug.Log("Current player number: " + currentPlayerNumber);
            Debug.Log("Current mode: " + currentMode);
            Debug.Log("Object under control: " + objectUnderControl);
            Debug.Log("Current 3Pcamera: " + current3PCam);
            Debug.Log("Blue 3P Cam Priority: " + bluePlayer3PCam.Priority);
            Debug.Log("Green 3P Cam Priority: " + greenPlayer3PCam.Priority);
            Debug.Log("Red 3P Cam Priority: " + redPlayer3PCam.Priority);
            Debug.Log("Pink 3P Cam Priority: " + pinkPlayer3PCam.Priority);
            Debug.Log("Blue FPS Cam Priority: " + bluePlayerFPSCam.Priority);
            Debug.Log("Green FPS Cam Priority: " + greenPlayerFPSCam.Priority);
            Debug.Log("Red FPS Cam Priority: " + redPlayerFPSCam.Priority);
            Debug.Log("Pink FPS Cam Priority: " + pinkPlayerFPSCam.Priority);
            Debug.Log("///////////////////////////////////////////////////////////////////////////////////////////");
        }

        // grab the appropriate player object, FPS, and 3P cameras, and RigidBody for this turn

        if (currentPlayerNumber == 0 && !isIntro) // Blue player
        {
            if (objectUnderControl != bluePlayerObject)
            {
                objectUnderControl = bluePlayerObject;
            }

            if (current3PCam != bluePlayer3PCam)
            {

                current3PCam = bluePlayer3PCam;
            }

            if (currentFPSCam != bluePlayer3PCam)
            {
                currentFPSCam = bluePlayerFPSCam;
            }
        }

        if (currentPlayerNumber == 1  && !isIntro) // Green player
        {
            if (objectUnderControl != greenPlayerObject)
            {
                objectUnderControl = greenPlayerObject;
            }

            if (current3PCam != greenPlayer3PCam)
            {
                current3PCam = greenPlayer3PCam;
            }

            if (currentFPSCam != greenPlayer3PCam)
            {
                currentFPSCam = greenPlayerFPSCam;
            }
        }

        if (currentPlayerNumber == 2 && !isIntro) // Red player
        {
            if (objectUnderControl != redPlayerObject)
            {
                objectUnderControl = redPlayerObject;
            }

            if (current3PCam != redPlayer3PCam)
            {
                current3PCam = redPlayer3PCam;
            }

            if (currentFPSCam != redPlayer3PCam)
            {
                currentFPSCam = redPlayerFPSCam;
            }
        }

        if (currentPlayerNumber == 3 && !isIntro) // Pink player
        {
            if (objectUnderControl != pinkPlayerObject)
            {
                objectUnderControl = pinkPlayerObject;
            }

            if (current3PCam != pinkPlayer3PCam)
            {
                current3PCam = pinkPlayer3PCam;
            }

            if (currentFPSCam != pinkPlayer3PCam)
            {
                currentFPSCam = pinkPlayerFPSCam;
            }
        }

        if (currentAnimator != objectUnderControl.GetComponent<Animator>())
        {
            currentAnimator = objectUnderControl.GetComponent<Animator>();
        }
        
        // set up the HUD info
        if (objectUnderControl == bluePlayerObject)
        {
            currentPlayerText.text = "Blue Guy";
            currentHPText.text = bluePlayerHP.ToString();
        }
        if (objectUnderControl == greenPlayerObject)
        {
            currentPlayerText.text = "Green Guy";
            currentHPText.text = greenPlayerHP.ToString();
        }
        if (objectUnderControl == redPlayerObject)
        {
            currentPlayerText.text = "Red Guy";
            currentHPText.text = redPlayerHP.ToString();
        }
        if (objectUnderControl == pinkPlayerObject)
        {
            currentPlayerText.text = "Pink Guy";
            currentHPText.text = pinkPlayerHP.ToString();
        }

        if (!reactSequence)
        {
            if (currentMode == "ThirdPerson")
            {
                projectileObject.SetActive(false);
                ActivateCamera(current3PCam);
                ActivateSoloUIPanel(gameStatePanel);
                
                if (myKB.dKey.isPressed)
                {
                    currentAnimator.SetBool("RotateRight", true);
                    objectUnderControl.transform.Rotate(0, rotateSpeed, 0);
                }
                else
                {
                    currentAnimator.SetBool("RotateRight", false);
                }

                if (myKB.aKey.isPressed)
                {
                    currentAnimator.SetBool("RotateLeft", true);
                    objectUnderControl.transform.Rotate(0, -rotateSpeed, 0);
                }
                else
                {
                    currentAnimator.SetBool("RotateLeft", false);
                }

                if (myKB.spaceKey.isPressed)
                {
                    isCrouched = true;
                    currentAnimator.SetBool("CrouchBool", true);
                }
                
                //////////////////////////////////////////////////////////////////
                // 3P CAMERA UP/DOWN
                if (myKB.wKey.isPressed)
                {
                    Debug.Log("ROTATE UP!");
                    current3PCam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y += rotateSpeed/100f;
                }
                if (myKB.sKey.isPressed)
                {
                    // "tracked object offset" down
                    Debug.Log("ROTATE DOWN!");
                    current3PCam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y -= rotateSpeed/100f;
                }
                /*else
                {
                    // default y offset is ..6f
                    current3PCam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y = .6f;
                }*/
                ///////////////////////////////////////////////////////////////////////
                
                if (myKB.spaceKey.wasReleasedThisFrame)
                {
                    isCrouched = false;
                    currentAnimator.SetBool("CrouchBool", false);
                    Jump();
                }
            }

            if (currentMode == "FPS")
            {
                ActivateSoloUIPanel(crosshairUIPanel);

                projectileObject.SetActive(false);

                ActivateCamera(currentFPSCam);

                if (myKB.wKey.isPressed)
                {
                    currentFPSCam.transform.Rotate(new Vector3(-aimSpeed, 0, 0), Space.Self);
                }

                if (myKB.sKey.isPressed)
                {
                    currentFPSCam.transform.Rotate(new Vector3(aimSpeed, 0, 0), Space.Self);
                }

                if (myKB.dKey.isPressed)
                {
                    currentFPSCam.transform.Rotate(new Vector3(0, aimSpeed, 0), Space.World);
                }

                if (myKB.aKey.isPressed)
                {
                    currentFPSCam.transform.Rotate(new Vector3(0, -aimSpeed, 0), Space.World);
                }

                if (myKB.spaceKey.wasPressedThisFrame)
                {
                    ClearUI();
                    currentAnimator.SetTrigger("FireTrigger");
                    LaunchProjectile();
                }
            }
        }

        
    }

    private void FixedUpdate()
    {
        if (!reactSequence)
        {
            if (currentMode == "Projectile")
            {
                projectileLightning.SetActive(true);
                ActivateCamera(projectileCam);
            
                // move the fucking thing
                projectileObject.transform.localPosition += Vector3.forward * projectileSpeed;
            }
        }
        
    }

    private void LateUpdate()
    {
        
    }

    public void ActivateCamera(CinemachineVirtualCamera cam)
    {
        if(cam.Priority != 10)
        {
            foreach (CinemachineVirtualCamera thisCam in allMyCinemachineVirtualCameras)
            {
                thisCam.Priority = 0;
            }
            cam.Priority = 10;
        }
    }

    void ActivateSoloUIPanel(GameObject thisUIPanel)
    {
        if (!thisUIPanel.activeInHierarchy)
        {
            foreach (var uIElement in allMyUIElements)
            {
                uIElement.SetActive(false);
            }
            thisUIPanel.SetActive(true);
        }
    }

    void ClearUI()
    {
        foreach (var uIElement in allMyUIElements)
        {
            uIElement.SetActive(false);
        }
    }

    void AdvanceGame()
    {
        if(currentMode == "ThirdPerson")
        {
            currentMode = "FPS";
            foreach (CinemachineVirtualCamera cammy in allMyCinemachineVirtualCameras)
            {
                if (cammy.CompareTag("3PCam"))
                {
                    cammy.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y = 0.6f;
                }
            }
        } 
        else if(currentMode == "FPS")
        {
            currentMode = "ThirdPerson";
            currentPlayerNumber = (currentPlayerNumber + 1) % numberOfPlayers;
        } 
        else if(currentMode == "Projectile")
        {
            currentMode = "ThirdPerson";
            currentPlayerNumber = (currentPlayerNumber + 1) % numberOfPlayers;
        }
        else
        {
            Debug.Log("Unknown string value in currentMode!!!!!!!!!!");
        }
    }

    

    void Jump()
    {
        String JumpStrengthValueString = (jumpMultiplier * 100).ToString();
        JumpStrengthValueString += "%";
        jumpStrengthText.text = JumpStrengthValueString;
        ActivateSoloUIPanel(jumpStrengthLabel);
        Rigidbody myRB = objectUnderControl.GetComponent<Rigidbody>();
        myRB.AddForce(new Vector3(0f, verticalJump * jumpMultiplier, 0f), ForceMode.Impulse);
        myRB.AddRelativeForce(transform.InverseTransformDirection(transform.forward * horizontalJump*jumpMultiplier));
        StartCoroutine(ClearUIAfterDelay(2));
    }

    void LaunchProjectile()
    {
        projectileObject.SetActive(true);
        Vector3 newRotationEulers = new Vector3(currentFPSCam.transform.eulerAngles.x + 90f, currentFPSCam.transform.eulerAngles.y, currentFPSCam.transform.eulerAngles.z);
        Vector3 newPositionVector = currentFPSCam.transform.position;
        projectileObject.transform.position = newPositionVector;
        projectileObject.transform.eulerAngles = newRotationEulers;
        projectileObject.transform.SetParent(currentFPSCam.transform);
        currentMode = "Projectile";
    }

    public void GetHit(String playerColor)
    {
        projectileObject.SetActive(false);
        if (playerColor == "Blue")
        {
            reactSequence = true;
            ActivateCamera(bluePlayer3PCam);
            bluePlayerHP -= 1;
            if (bluePlayerHP <= 0)
            {
                bluePlayerDead = true;
                blueAnimator.SetTrigger("DieTrigger");
                StartCoroutine(WaitTwoSecondsThenAdvanceTheGame());
            }
            else
            {
                blueLightning.SetActive(true);
                blueAnimator.SetTrigger("ReactTrigger");
                StartCoroutine(WaitTwoSecondsThenAdvanceTheGame());
            }
        }
        if (playerColor == "Green")
        {
            reactSequence = true;
            ActivateCamera(greenPlayer3PCam);
            greenPlayerHP -= 1;
            if (greenPlayerHP <= 0)
            {
                greenPlayerDead = true;
                greenAnimator.SetTrigger("DieTrigger");
                StartCoroutine(WaitTwoSecondsThenAdvanceTheGame());
            }
            else
            {
                greenLightning.SetActive(true);
                greenAnimator.SetTrigger("ReactTrigger");
                StartCoroutine(WaitTwoSecondsThenAdvanceTheGame());
            }
        }
        if (playerColor == "Red")
        {
            reactSequence = true;
            ActivateCamera(redPlayer3PCam);
            redPlayerHP -= 1;
            if (redPlayerHP <= 0)
            {
                redPlayerDead = true;
                redAnimator.SetTrigger("DieTrigger");
                StartCoroutine(WaitTwoSecondsThenAdvanceTheGame());
            }
            else
            {
                redLightning.SetActive(true);
                redAnimator.SetTrigger("ReactTrigger");
                StartCoroutine(WaitTwoSecondsThenAdvanceTheGame());
            }
        }
        if (playerColor == "Pink")
        {
            reactSequence = true;
            ActivateCamera(pinkPlayer3PCam);
            pinkPlayerHP -= 1;
            if (pinkPlayerHP <= 0)
            {
                pinkPlayerDead = true;
                pinkAnimator.SetTrigger("DieTrigger");
                StartCoroutine(WaitTwoSecondsThenAdvanceTheGame());
            }
            else
            {
                pinkLightning.SetActive(true);
                pinkAnimator.SetTrigger("ReactTrigger");
                StartCoroutine(WaitTwoSecondsThenAdvanceTheGame());
            }
        }
        
    }

    private void ActivateStartUIPanel()
    {
        StartUIPanelObject.SetActive(true);
    }

    public void twoButton()
    {
        numberOfPlayers = 2;
    }

    public void threeButton()
    {
        numberOfPlayers = 3;
    }
    
    public void fourButton()
    {
        numberOfPlayers = 4;
    }
    
    public void ContinueButton()
    {
        isIntro = false;
        Time.timeScale = 1;
        ClearUI();
    }

    private IEnumerator WaitForNextFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
    }

    

    private IEnumerator ClearUIAfterDelay(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        ClearUI();
    }

    private IEnumerator WaitTwoSecondsThenAdvanceTheGame()
    {
        yield return new WaitForSecondsRealtime(1.9f);
        foreach (var thing in allMyLightningFX)
        {
            thing.SetActive(false);
        }

        yield return new WaitForSecondsRealtime(.1f);
        reactSequence = false;
        AdvanceGame();
    }

}
