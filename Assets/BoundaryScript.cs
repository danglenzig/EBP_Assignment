using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryScript : MonoBehaviour
{

    [SerializeField] private GameObject bluePlayerObject;
    [SerializeField] private GameObject greenPlayerObject;
    [SerializeField] private GameObject redPlayerObject;
    [SerializeField] private GameObject pinkPlayerObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            SceneDirector21SEP2022.projectileOutOfBounds = true;
        }

        if (other.CompareTag("Player"))
        {
            if (SceneDirector21SEP2022.currentPlayerNumber == 0)
            {
                Teleport(bluePlayerObject);
            }
            if (SceneDirector21SEP2022.currentPlayerNumber == 1)
            {
                Teleport(greenPlayerObject);
            }
            if (SceneDirector21SEP2022.currentPlayerNumber == 2)
            {
                Teleport(redPlayerObject);
            }
            if (SceneDirector21SEP2022.currentPlayerNumber == 3)
            {
                Teleport(pinkPlayerObject);
            }
            SceneDirector21SEP2022.playerOutOfBounds = true;
        }
    }

    private void Teleport(GameObject objectToTeleport)
    {
        Debug.Log("Teleport");
        objectToTeleport.transform.position = new Vector3(1, 2, 1);
    }
}
