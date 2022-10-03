using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class WelcomeSceneManagerScript : MonoBehaviour
{
    /*
     * Attach this script to an empty GameObject called "WelcomeSceneManager",
     * in a Unity scene called "WelcomeScene"
     *  -b
     */
    
    // Load up the various UI elements

    [SerializeField] private GameObject welcomePanel;
    [SerializeField] private GameObject LogoSplashPanel;
    [SerializeField] private GameObject HowToPlayPanel;
    [SerializeField] private GameObject SettingsPannel;
    [SerializeField] private GameObject AboutPanel;

    private Keyboard myKB;

    public static bool clearUI = false;

    private void Awake()
    {
        DeactivateAllUIElements();
        StartCoroutine(LogoSplash());
    }

    // Start is called before the first frame update
    void Start()
    {
        myKB = Keyboard.current;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void WelcomeScreen()
    {
        DeactivateAllUIElements();
        welcomePanel.SetActive(true);
    }

    public void DeactivateAllUIElements()
    {
        welcomePanel.SetActive(false);
        LogoSplashPanel.SetActive(false);
        HowToPlayPanel.SetActive(false);
        SettingsPannel.SetActive(false);
        AboutPanel.SetActive(false);
    }

    private IEnumerator LogoSplash()
    {
        LogoSplashPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(3.5f);
        WelcomeScreen();
    }
    
}
