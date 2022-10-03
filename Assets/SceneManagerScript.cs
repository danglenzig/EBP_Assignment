using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SceneManagerScript : MonoBehaviour
{

    private Keyboard myKeyboard;

    [SerializeField] private GameObject playerObject; // put a prefab here
    [SerializeField] private GameObject pauseMenuObject;
    [SerializeField] private GameObject howToPlayMenuObject;
    
    public static bool gamePaused = false;
    
    //Start is called before the first frame update

    private void Awake()
    {

    }

    void Start()
    {
        pauseMenuObject.SetActive(false);
        howToPlayMenuObject.SetActive(false);
        myKeyboard = Keyboard.current;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (myKeyboard.escapeKey.wasPressedThisFrame)
        {
            if (!gamePaused)
            {
                PauseGame();
            }
            else
            {
                PauseResumeButton();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        gamePaused = true;
        pauseMenuObject.SetActive(true);
    }

    public void PauseResumeButton()
    {
        pauseMenuObject.SetActive(false);
        howToPlayMenuObject.SetActive(false);
        gamePaused = false;
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("User quits the application");
        Application.Quit();
    }

    public void PauseHowToPlayButton()
    {
        pauseMenuObject.SetActive(false);
        howToPlayMenuObject.SetActive(true);
    }
}
