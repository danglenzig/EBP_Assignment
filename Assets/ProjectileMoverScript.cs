using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMoverScript : MonoBehaviour
{

    [SerializeField] private GameObject projectilePrefab;

    private Vector3 projectileStartPosition;
    private Vector3 projectEndPosition;

    private void Awake()
    {
        Instantiate(projectilePrefab);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.projectilePrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
