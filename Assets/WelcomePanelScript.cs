using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class WelcomePanelScript : MonoBehaviour
{

    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject aboutPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void HowToPlayButton()
    {
        howToPlayPanel.SetActive(true);
    }

    public void SettingsButton()
    {
        settingsPanel.SetActive(true);
    }

    public void AboutButton()
    {
        aboutPanel.SetActive(true);
    }

    public void QuitButton()
    {
        Debug.Log("User quits the application");
        Application.Quit();
    }
    
    


}
