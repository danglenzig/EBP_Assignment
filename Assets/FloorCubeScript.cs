using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FloorCubeScript : MonoBehaviour
{

    private float cubeHeight = 0.1f;
    private int randomHeight = 0;



    private void Awake()
    {
        randomHeight = Random.Range(1, 4);
        if (randomHeight == 1)
        {
            cubeHeight = 0.1f;
        }
        if (randomHeight == 2)
        {
            cubeHeight = 0.6f;
        }
        if (randomHeight == 3)
        {
            cubeHeight = 01.1f;
        }
        this.transform.localScale = new Vector3(this.transform.localScale.z,cubeHeight,this.transform.localScale.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
