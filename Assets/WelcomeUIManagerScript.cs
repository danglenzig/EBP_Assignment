using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WelcomeUIManagerScript : MonoBehaviour
{
    
    // Get all the UI elements from the hierarchy
    [SerializeField] private GameObject howToPlayPanelObject;
    [SerializeField] private GameObject aboutPanelObject;
    [SerializeField] private GameObject buttonsObject;
    [SerializeField] private GameObject splashPanelObject;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private TMP_Text numberOfPlayersValueText;
    [SerializeField] private TMP_Text terrainRandomnessValueText;
    
    // make an empty list
    private List<GameObject> myUIElements = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // put the UI stuff from above into the list
        myUIElements.Add(howToPlayPanelObject);
        myUIElements.Add(aboutPanelObject);
        myUIElements.Add(splashPanelObject);
        
        // Set it up....
        howToPlayPanelObject.SetActive(false);
        aboutPanelObject.SetActive(false);
        buttonsObject.SetActive(true);
        
        // Futuregames & DiscoDawgz logos
        splashPanelObject.SetActive(true);
        StartCoroutine(HideSplashPanel());
    }

    // Update is called once per frame
    void Update()
    {
        
        // refresh the Settings UI values
        numberOfPlayersValueText.text = SceneDirector21SEP2022.numberOfPlayers.ToString();
        if (GridBuilderScript1.likenessFactor > 7)
        {
            if (GridBuilderScript1.likenessFactor >= 15)
            {
                terrainRandomnessValueText.text = "High";
            }
            else
            {
                terrainRandomnessValueText.text = "Medium";
            }
        }
        else
        {
            terrainRandomnessValueText.text = "Low";
        }
    }

    public void StartButton()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void HowToPlayButton()
    {
        ActivateUI(howToPlayPanelObject);
    }

    public void AboutButton()
    {
        ActivateUI(aboutPanelObject);
    }

    public void SettingsButton()
    {
        ActivateUI(settingsPanel);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void HideAboutButton()
    {
        aboutPanelObject.SetActive(false);
    }

    public void HideHowToPlayButton()
    {
        howToPlayPanelObject.SetActive(false);
    }

    public void HideSettingsButton()
    {
        settingsPanel.SetActive(false);
    }

    private void ActivateUI(GameObject uIObject)
    {
        // turn off everything except uIObject
        foreach (GameObject element in myUIElements)
        {
            if (element != uIObject)
            {
                element.SetActive(false);
            }
        }
        uIObject.SetActive(true);
    }

    public void NumberOfPlayersPlusButton()
    {
        if (SceneDirector21SEP2022.numberOfPlayers < 4)
        {
            SceneDirector21SEP2022.numberOfPlayers++;
        }
    }
    public void NumberOfPlayersMinusButton()
    {
        if (SceneDirector21SEP2022.numberOfPlayers > 2)
        {
            SceneDirector21SEP2022.numberOfPlayers--;
        }
    }
    public void TerrainRandomnessPlusButton()
    {
        if (GridBuilderScript1.likenessFactor < 15)
        {
            GridBuilderScript1.likenessFactor += 5;
        }
    }
    public void TerrainRandomnessMinusButton()
    {
        if (GridBuilderScript1.likenessFactor > 5)
        {
            GridBuilderScript1.likenessFactor -= 5;
        }
    }

    private IEnumerator HideSplashPanel()
    {
        yield return new WaitForSeconds(3);
        splashPanelObject.SetActive(false);
    }
}
