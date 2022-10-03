using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridBuilderScript : MonoBehaviour
{

    private Vector3 cubePosition;
    private float xPos = 0f;
    private float yPos = 0f;
    private float zPos = 0f;
    private int thisCube = 0;


    public int numberOfRows = 10;
    public int numberOfColumns = 10;
    [Tooltip("1-20, lower numbers = greater variability")]public int likenessFactor = 15;

    private GameObject floorCube = null;
    [SerializeField] private GameObject earthCube;
    [SerializeField] private GameObject fireCube;
    [SerializeField] private GameObject waterCube;
    
    
    private void Awake()
    {

        cubePosition = new Vector3(0, 0, 0);
        floorCube = earthCube;
        
        for (int rowNumber = 0; rowNumber < numberOfRows; rowNumber++)
        {
            for (int columnNumber = 0; columnNumber < numberOfColumns; columnNumber++)
            {


                if (Random.Range(1, 21) < likenessFactor)
                {
                    floorCube = floorCube;
                }
                else
                {
                    thisCube = Random.Range(1, 4);
                    if (thisCube == 1)
                    {
                        floorCube = earthCube;
                    }
                    if (thisCube == 2)
                    {
                        floorCube = fireCube;
                    }
                    if (thisCube == 3)
                    {
                        floorCube = waterCube;
                    }
                }
                cubePosition = new Vector3(xPos, yPos, zPos);
                Instantiate(floorCube, cubePosition, Quaternion.identity);
                zPos += 1;
            }
            zPos = 0;
            xPos += 1;
        }
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
