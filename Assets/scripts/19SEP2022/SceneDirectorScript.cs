using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SceneDirectorScript : MonoBehaviour
{
    // important game states //
    public static bool fPSModeOn = false;
    public static bool projectileModeOn = false;
    public static bool thirdPersonModeOn = true;
    public static  int currentPlayerNumber = 0;
    public static int numberOfPlayers = 0;

    [Tooltip("2-4 players")]public int numberOfPlayersPublic = 2;

    [SerializeField] private GameObject bluePlayerObjectPrivate;
    [SerializeField] private GameObject greenPlayerObjectPrivate;
    [SerializeField] private GameObject redPlayerObjectPrivate;
    [SerializeField] private GameObject pinkPlayerObjectPrivate;
    [SerializeField] private GameObject projectileObjectPrivate;
    
    public static GameObject bluePlayerObject = null;
    public static GameObject greenPlayerObject = null;
    public static GameObject redPlayerObject = null;
    public static GameObject pinkPlayerObject = null;
    public static GameObject projectileObject = null;
    
    private List<GameObject> allMyPlayerObjects = new List<GameObject>();
    private List<Boolean> allMyGameStates = new List<bool>();
    private Keyboard myKB;

    private void Awake()
    {
        bluePlayerObject = this.bluePlayerObjectPrivate;
        greenPlayerObject = this.greenPlayerObjectPrivate;
        redPlayerObject = this.redPlayerObjectPrivate;
        pinkPlayerObject = this.pinkPlayerObjectPrivate;
        projectileObject = this.projectileObjectPrivate;
        
        myKB = Keyboard.current;
        allMyPlayerObjects.Add(bluePlayerObject);
        allMyPlayerObjects.Add(greenPlayerObject);
        allMyPlayerObjects.Add(redPlayerObject);
        allMyPlayerObjects.Add(pinkPlayerObject);
        
        allMyGameStates.Add(fPSModeOn);
        allMyGameStates.Add(projectileModeOn);
        allMyGameStates.Add(thirdPersonModeOn);
        
        projectileObject.SetActive(false);
        numberOfPlayers = numberOfPlayersPublic;

    }


    // Start is called before the first frame update
    void Start()
    {
        myKB = Keyboard.current;
    }
}
