using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RedPlayerScript : MonoBehaviour
{

    private Keyboard myKB;

    [SerializeField] private GameObject sceneDirector;
    
    // Start is called before the first frame update
    void Start()
    {
        myKB = Keyboard.current;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (SceneDirector21SEP2022.currentPlayerNumber != 2)
        {
            if (other.CompareTag("Projectile"))
            {
                Debug.Log("Red got hit!");
                sceneDirector.GetComponent<SceneDirector21SEP2022>().GetHit("Red");
            }
        }
    }
}