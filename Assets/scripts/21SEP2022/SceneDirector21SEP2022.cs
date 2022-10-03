using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneDirector21SEP2022 : MonoBehaviour
{
    
    /**************** drag everything we'll need from the scene hierarchy into the script **************/
    
    //The four player objects and their associated cameras 
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
    
    // the projectile and its camera [1]
    [SerializeField] private GameObject projectileObject;
    [SerializeField] private CinemachineVirtualCamera projectileCam;
    
    // I'm not actually using the PPVolume in the script anywhere yet, but just in case I decide to..... 
    [SerializeField] private PostProcessVolume myPP;
    
    // HUD elements
    [SerializeField] private GameObject crosshairUIPanel;
    [SerializeField] private GameObject jumpStrengthLabel;
    [SerializeField] private TMP_Text jumpStrengthText;
    [SerializeField] private GameObject gameStatePanel;
    [SerializeField] private TMP_Text currentPlayerText;
    [SerializeField] private TMP_Text currentHPText;
    [SerializeField] private GameObject jumpHelpText;
    [SerializeField] private GameObject youWinText;

    // and the lightning VFX
    [SerializeField] private GameObject blueLightning;
    [SerializeField] private GameObject greenLightning;
    [SerializeField] private GameObject redLightning;
    [SerializeField] private GameObject pinkLightning;
    [SerializeField] private GameObject projectileLightning;
    
    // expose some interesting game parameters to the editor
    public float projectileSpeed = 1f;
    public float aimSpeed = .05f;
    public float rotateSpeed = .3f;
    public float verticalJump = 100f;
    public float horizontalJump = 100f;
    public int maxHP = 2; 
   
    // some private variables we'll need
    private GameObject objectUnderControl = null;
    private GameObject winningPlayer = null;
    private CinemachineVirtualCamera winningCamera = null;
    private Rigidbody currentRigidBody;
    private CinemachineVirtualCamera currentFPSCam = null;
    private CinemachineVirtualCamera current3PCam = null;
    private Animator currentAnimator = null;
    private Animator blueAnimator = null;
    private Animator greenAnimator = null;
    private Animator redAnimator = null;
    private Animator pinkAnimator = null;
    private bool alreadyJumped = false;
    private bool alreadyFired = false;
    
    // expose these variables to the other scripts 
    public static string currentMode = "ThirdPerson";
    public static int numberOfPlayers = 4;
    public static int currentPlayerNumber = 0;
    public static float jumpMultiplier = 1;
    public static bool projectileMode = false;
    public static bool fPSMode = false;
    public static bool thirdPersonMode = true;
    //public static bool playerIsGrounded = true; // player trigger collider's "OnEnter()" will look for "Ground" tag and set this
    public static bool isCrouched = false;
    public static bool bluePlayerDead = false;
    public static bool greenPlayerDead = false;
    public static bool redPlayerDead = false;
    public static bool pinkPlayerDead = false;
    public static bool reactSequence = false;
    public static bool projectileOutOfBounds = false;
    public static bool terrainHit = false;
    public static bool playerOutOfBounds = false;
    public static bool blueIsDead = false;
    public static bool greenIsDead = false;
    public static bool redIsDead = false;
    public static bool pinkIsDead = false;
    public static bool currentPlayerIsDead = false;
    public static int bluePlayerHP = 0;
    public static int greenPlayerHP = 0;
    public static int redPlayerHP = 0;
    public static int pinkPlayerHP = 0;
    public static int deadPlayers = 0;
    
    // Listify the cameras, lightning, and scene UI elements
    private List<CinemachineVirtualCamera> allMyCinemachineVirtualCameras = new List<CinemachineVirtualCamera>();
    private List<GameObject> allMyLightningFX = new List<GameObject>();
    private List<GameObject> allMyUIElements = new List<GameObject>();
    
    // create a keyboard to receive inpuut
    private Keyboard myKB;
    
    void Start()
    {
        
        Debug.Log("FUUUUUUUUUUUUUUUUUUUCK " + numberOfPlayers);
        
        // add the cameras to the camera list
        allMyCinemachineVirtualCameras.Add(bluePlayer3PCam);
        allMyCinemachineVirtualCameras.Add(bluePlayerFPSCam);
        allMyCinemachineVirtualCameras.Add(greenPlayer3PCam);
        allMyCinemachineVirtualCameras.Add(greenPlayerFPSCam);
        allMyCinemachineVirtualCameras.Add(redPlayer3PCam);
        allMyCinemachineVirtualCameras.Add(redPlayerFPSCam);
        allMyCinemachineVirtualCameras.Add(pinkPlayer3PCam);
        allMyCinemachineVirtualCameras.Add(pinkPlayerFPSCam);
        allMyCinemachineVirtualCameras.Add(projectileCam);
        
        // the UI elements to the UI elements list
        allMyUIElements.Add(crosshairUIPanel);
        allMyUIElements.Add(jumpStrengthLabel);
        allMyUIElements.Add(gameStatePanel);
        allMyUIElements.Add(jumpHelpText);
        allMyUIElements.Add(youWinText);
        
        // so on and so forth...
        allMyLightningFX.Add(blueLightning);
        allMyLightningFX.Add(greenLightning);
        allMyLightningFX.Add(redLightning);
        allMyLightningFX.Add(pinkLightning);
        allMyLightningFX.Add(projectileLightning);
        
        // populate the keyboard variable
        myKB = Keyboard.current;
        
        // populate the animators
        blueAnimator = bluePlayerObject.GetComponent<Animator>();
        greenAnimator = greenPlayerObject.GetComponent<Animator>();
        redAnimator = redPlayerObject.GetComponent<Animator>();
        pinkAnimator = pinkPlayerObject.GetComponent<Animator>();

        // initialize the scene
        projectileObject.SetActive(false);
        bluePlayerHP = maxHP;
        greenPlayerHP = maxHP;
        redPlayerHP = maxHP;
        pinkPlayerHP = maxHP;
        blueLightning.SetActive(false);
        greenLightning.SetActive(false);
        redLightning.SetActive(false);
        pinkLightning.SetActive(false);
        ClearUI();
    }

    void Update()
    {
        if (deadPlayers >= 3) // i.e. if there is only one player left standing
        {
            // focus on the winning player....
            if (!blueIsDead)
            {
                winningPlayer = bluePlayerObject;
                winningCamera = bluePlayer3PCam;
            }
            if (!greenIsDead)
            {
                winningPlayer = greenPlayerObject;
                winningCamera = greenPlayer3PCam;
            }
            if (!redIsDead)
            {
                winningPlayer = redPlayerObject;
                winningCamera = redPlayer3PCam;
            }
            if (!pinkIsDead)
            {
                winningPlayer = pinkPlayerObject;
                winningCamera = pinkPlayer3PCam;
            }
            
            // Party!
            WinnerScene(winningPlayer,winningCamera);
        }
        else // ...otherwise gameplay proceeds normally
        {
            // Secret dev command -- shift+O dumps some interesting variables to the editor console.
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
            if (currentPlayerNumber == 0) // Blue player
            {
                if (objectUnderControl != bluePlayerObject)
                {
                    objectUnderControl = bluePlayerObject;
                    if (blueIsDead)
                    {
                        currentPlayerIsDead = true;
                    }
                    else currentPlayerIsDead = false;
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
    
            if (currentPlayerNumber == 1) // Green player
            {
                if (objectUnderControl != greenPlayerObject)
                {
                    objectUnderControl = greenPlayerObject;
                    if (greenIsDead)
                    {
                        currentPlayerIsDead = true;
                    }
                    else currentPlayerIsDead = false;
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
    
            if (currentPlayerNumber == 2) // Red player
            {
                if (objectUnderControl != redPlayerObject)
                {
                    objectUnderControl = redPlayerObject;
                    if (redIsDead)
                    {
                        currentPlayerIsDead = true;
                    }
                    else currentPlayerIsDead = false;
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
    
            if (currentPlayerNumber == 3) // Pink player
            {
                if (objectUnderControl != pinkPlayerObject)
                {
                    objectUnderControl = pinkPlayerObject;
                    if (pinkIsDead)
                    {
                        currentPlayerIsDead = true;
                    }
                    else currentPlayerIsDead = false;
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
            
            // assign the appropriate animator
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
            
            if (!reactSequence) // i.e. if the player is not currently being electrocuted...
            {
                // fetch keyboard input. The controls differ based on the current game state.
                if (currentMode == "ThirdPerson")
                {
                    if (currentPlayerIsDead) // if our guy is dead...
                    {
                        AdvanceGame(); // ....move along
                    }
                    
                    // initialize the turn...
                    jumpHelpText.SetActive(true);
                    projectileOutOfBounds = false;
                    terrainHit = false;
                    playerOutOfBounds = false;
                    projectileObject.SetActive(false);
                    ActivateCamera(current3PCam);
                    ActivateSoloUIPanel(gameStatePanel);
                    
                    if (myKB.dKey.isPressed) //rotate player object clockwise
                    {
                        currentAnimator.SetBool("RotateRight", true);
                        objectUnderControl.transform.Rotate(0, rotateSpeed, 0);
                    }
                    else
                    {
                        currentAnimator.SetBool("RotateRight", false);
                    }
    
                    if (myKB.aKey.isPressed) //rotate player anticlockwise
                    {
                        currentAnimator.SetBool("RotateLeft", true);
                        objectUnderControl.transform.Rotate(0, -rotateSpeed, 0);
                    }
                    else
                    {
                        currentAnimator.SetBool("RotateLeft", false);
                    }
    
                    if (myKB.wKey.isPressed) // rotate current 3P camera up
                    {
                        current3PCam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y += rotateSpeed/100f;
                    }
                    if (myKB.sKey.isPressed) // rotate current 3P camera down
                    {
                        current3PCam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y -= rotateSpeed/100f;
                    }
                    
                    if (myKB.spaceKey.isPressed) // get ready to jump
                    {
                        jumpHelpText.SetActive(false);
                        isCrouched = true; // this tells the LauchWidgetControllerScript to turn on the launch widget
                        currentAnimator.SetBool("CrouchBool", true);
                    }
    
                    if (myKB.enterKey.wasPressedThisFrame)
                    {
                        AdvanceGame(); // player presses Enter to proceed to FPS mode without jumping
                    }
                    
                    if (myKB.spaceKey.wasReleasedThisFrame)
                    {
                        isCrouched = false; // LauchWidgetControllerScript does its thing, and turns off the launch widget
                        currentAnimator.SetBool("CrouchBool", false);
                        Jump();
                    }
                }
    
                if (currentMode == "FPS") // this is the current player's attack phase
                {
                    if (currentPlayerIsDead)
                    {
                        AdvanceGame();
                    }
                    if (playerOutOfBounds)
                    {
                        AdvanceGame();
                    }
                    
                    // set it up
                    ActivateSoloUIPanel(crosshairUIPanel);
                    projectileObject.SetActive(false);
                    ActivateCamera(currentFPSCam);
    
                    if (myKB.wKey.isPressed) // rotate FPS cam clockwise
                    {
                        currentFPSCam.transform.Rotate(new Vector3(-aimSpeed, 0, 0), Space.Self);
                    }
    
                    if (myKB.sKey.isPressed) // rotate FPS cam anticlockwise
                    {
                        currentFPSCam.transform.Rotate(new Vector3(aimSpeed, 0, 0), Space.Self);
                    }
    
                    if (myKB.dKey.isPressed) // rotate FPS cam up
                    {
                        currentFPSCam.transform.Rotate(new Vector3(0, aimSpeed, 0), Space.World);
                    }
    
                    if (myKB.aKey.isPressed) // // rotate FPS cam down
                    {
                        currentFPSCam.transform.Rotate(new Vector3(0, -aimSpeed, 0), Space.World);
                    }
    
                    if (myKB.spaceKey.wasPressedThisFrame) // shoot your gun
                    {
                        ClearUI();
                        currentAnimator.SetTrigger("FireTrigger");
                        LaunchProjectile();
                    }
                }
            }
        }
    }

    private void FixedUpdate() // I moved the projectile movement to here from Update(), and it looks way better  
    {
        if (!reactSequence)
        {
            if (currentMode == "Projectile")
            {
                projectileLightning.SetActive(true);
                ActivateCamera(projectileCam);
                
                // I'm moving the projectile object with its transform, not the rigidbody
                // because I need it to move in a perfectly straight line, from the center 
                // of the player's FPS cam (where the crosshair is) toward the target.
                projectileObject.transform.localPosition += Vector3.forward * projectileSpeed;
                
                if (projectileOutOfBounds || terrainHit)
                {
                    AdvanceGame();
                }
            }
        }
        
    }
    public void ActivateCamera(CinemachineVirtualCamera cam)
    {
        // set all the camera priorities to 0, except for the one I want active
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
        // Deactivate all the UI elements, except for the one I want active
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
        // Deactivate all the UI elements
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
                    // this resets the 3P cam if the player moved it during the previous phase
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
        InputSystem.DisableDevice(myKB);
        // the launch widget controller sets jumpMultiplier, based on the result
        // of the launch QTE event
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
                blueIsDead = true;
                deadPlayers++;
            }
            if (bluePlayerHP <= 0)
            {
                blueLightning.SetActive(true);
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
                greenIsDead = true;
                deadPlayers++;
            }
            if (greenPlayerHP <= 0)
            {
                greenLightning.SetActive(true);
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
                redIsDead = true;
                deadPlayers++;
            }
            if (redPlayerHP <= 0)
            {
                redLightning.SetActive(true);
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
                pinkIsDead = true;
                deadPlayers++;
            }
            if (pinkPlayerHP <= 0)
            {
                pinkLightning.SetActive(true);
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
        Time.timeScale = 1f;
        ClearUI();
    }
    
    private IEnumerator ClearUIAfterDelay(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        InputSystem.EnableDevice(myKB);
        ClearUI();
        AdvanceGame();
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

    void WinnerScene(GameObject winnerObject, CinemachineVirtualCamera winnerCamera)
    {
        ActivateCamera(winnerCamera);
        var transposer = winnerCamera.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_FollowOffset.z = 1;
        Animator winnerAnim = winnerObject.GetComponent<Animator>();
        winnerAnim.SetTrigger("WinDanceTrigger");
        ActivateSoloUIPanel(youWinText);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("WelcomeScene");
    }
}
