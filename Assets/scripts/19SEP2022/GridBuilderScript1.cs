using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilderScript1 : MonoBehaviour
{

    /*
     Attatch this to an empty game object called GridBuilder
     * */

    public static float nextCubeHeight = 2.01f;
    public static int likenessFactor = 10;
    
    
    [SerializeField] private GameObject terrainCubeObject; // put a cube prefab here
    public int numberOfRows = 10;
    public int numberOfColumns = 10;
    private int randomNumber = 2;
    private Vector3 cubeLocation = new Vector3(0f,0f,0f);
    
    

    // Start is called before the first frame update
    void Start()
    {
        
        // lay down the cubes one at a time, in a raster pattern
        // roll a D20, if the result is lower than likenessFactor,
        // randomize the size of the next cube. Otherwise, make it
        // the same size as the last one.
        // likenessFactor is exposed to the player in the Seeings
        // menu as "Terrain randomness"
        
        for(int rowNumber = 0; rowNumber < numberOfRows; rowNumber++)
        {
            for (int columnNumber = 0; columnNumber < numberOfColumns; columnNumber++)
            {
                if ((Random.Range(0, 20) > likenessFactor && nextCubeHeight > 1.25) || columnNumber == 0)
                {
                    randomNumber = Random.Range(0, 5);
                }
                if(randomNumber == 0)
                {
                    if (Random.Range(0, 10) < 9)
                    {
                        nextCubeHeight = 0.11f;
                    }
                    else
                    {
                        nextCubeHeight = 1.8f;
                    }
                    
                }
                if(randomNumber == 1)
                {
                    nextCubeHeight = .31f;
                }
                if(randomNumber == 2)
                {
                    if (Random.Range(0, 10) < 8)
                    {
                        nextCubeHeight = 1.6f;
                    }
                    nextCubeHeight = 0.6f;
                }
                if (randomNumber == 3)
                {
                    nextCubeHeight = 1.21f;
                }
                if (randomNumber == 4)
                {
                    nextCubeHeight = 1-7f;
                }
                cubeLocation = new Vector3(rowNumber, nextCubeHeight / 2, columnNumber);
                Instantiate(terrainCubeObject, cubeLocation, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
