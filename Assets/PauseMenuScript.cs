using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanelObject;

    private Keyboard myKB;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenuPanelObject.SetActive(false);
        myKB = Keyboard.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (myKB.escapeKey.wasPressedThisFrame)
        {
            Pause();
        }
    }

    void Pause()
    {
        Time.timeScale = 0;
        pauseMenuPanelObject.SetActive(true);
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
        pauseMenuPanelObject.SetActive(false);
    }
    
    public void MainButton()
    {
        SceneManager.LoadScene("WelcomeScene");
    }
    
    public void QuitButton()
    {
        Application.Quit();
    }
}
