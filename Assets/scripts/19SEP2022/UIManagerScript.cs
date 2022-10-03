using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManagerScript : MonoBehaviour
{

    [SerializeField] private GameObject playerLabelPanel;
    [SerializeField] private TMP_Text currentPlayerNumberText;
    
    // Start is called before the first frame update
    void Start()
    {
        playerLabelPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //currentPlayerNumberText.text = (SceneDirectorScript.currentPlayerNumber +1).ToString();
    }
}
