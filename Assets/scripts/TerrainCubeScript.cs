using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCubeScript : MonoBehaviour
{

    /*
     Attach this to a cube prefab
     */

    private float cubeHeight;
    private int randomNumber;

    private void Awake()
    {
        this.transform.localScale = new Vector3(1f, GridBuilderScript1.nextCubeHeight, 1f);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            SceneDirector21SEP2022.terrainHit = true;
        }
    }
}
