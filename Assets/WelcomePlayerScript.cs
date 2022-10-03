using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class WelcomePlayerScript : MonoBehaviour
{

    private Animator myAnimator;
    private int currentDanceNumber = 0;
    private float currentWaitTime = 3f;

    public float rotateSpeed = .1f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentDanceNumber = Random.Range(1, 17);
        myAnimator = this.GetComponent<Animator>();
        StartCoroutine(NextDance(currentWaitTime, currentDanceNumber));
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDanceNumber == 1)
        {
            myAnimator.SetInteger("DanceNumber",1);
        }
        
        if (currentDanceNumber == 2)
        {
            myAnimator.SetInteger("DanceNumber",2);
        }
        
        if (currentDanceNumber == 3)
        {
            myAnimator.SetInteger("DanceNumber",3);
        }
        
        if (currentDanceNumber == 4)
        {
            myAnimator.SetInteger("DanceNumber",4);
        }
        
        if (currentDanceNumber == 5)
        {
            myAnimator.SetInteger("DanceNumber",5);
        }
        
        if (currentDanceNumber == 6)
        {
            myAnimator.SetInteger("DanceNumber",6);
        }
        
        if (currentDanceNumber == 7)
        {
            myAnimator.SetInteger("DanceNumber",7);
        }
        
        if (currentDanceNumber == 8)
        {
            myAnimator.SetInteger("DanceNumber",8);
        }
        
        if (currentDanceNumber == 9)
        {
            myAnimator.SetInteger("DanceNumber",9);
        }
        
        if (currentDanceNumber == 10)
        {
            myAnimator.SetInteger("DanceNumber",10);
        }
        
        if (currentDanceNumber == 11)
        {
            myAnimator.SetInteger("DanceNumber",11);
        }
        
        if (currentDanceNumber == 12)
        {
            myAnimator.SetInteger("DanceNumber",12);
        }
        
        if (currentDanceNumber == 13)
        {
            myAnimator.SetInteger("DanceNumber",13);
        }
        
        if (currentDanceNumber == 14)
        {
            myAnimator.SetInteger("DanceNumber",14);
        }
        
        if (currentDanceNumber == 15)
        {
            myAnimator.SetInteger("DanceNumber",15);
        }
        
        if (currentDanceNumber == 16)
        {
            myAnimator.SetInteger("DanceNumber",16);
        }
        
        this.transform.Rotate(0,rotateSpeed,0);
        
    }

    private IEnumerator NextDance(float waitTime, int danceNumber)
    {
        yield return new WaitForSecondsRealtime(7);
        currentDanceNumber = Random.Range(1, 17);
        StartCoroutine(NextDance(currentWaitTime, currentDanceNumber));
    }
}
