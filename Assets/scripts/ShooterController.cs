using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShooterController : MonoBehaviour
{

    /*
     Attach this to a member of the player party
     */


    private Keyboard myKeyboard;
    private Mouse myMouse;
    private Rigidbody myRigidbody;
    private Animator myAnimator;
    private bool isGrounded = true;
    private bool isAiming = false;
    
    public float jumpStrength = 1f;
    public float horizontalMovement = 10f;
    public float rotateSpeed = 1f;
    public float sphereRadius = 1f;
    public float sphereMaxDistance = 1f;

    // Start is called before the first frame update
    void Start()
    {
        myKeyboard = Keyboard.current;
        myMouse = Mouse.current;
        myRigidbody = GetComponent<Rigidbody>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isGrounded)
        {
            myAnimator.SetBool("IsAirborne", false);
        }
        else
        {
            myAnimator.SetBool("IsAirborne", true);
        }
        
        if (myKeyboard.spaceKey.wasReleasedThisFrame)
        {
            if (isGrounded)
            {
                myAnimator.SetTrigger("JumpTrigger");
            }
        }
        if (myKeyboard.dKey.isPressed)
        {
            if (isGrounded)
            {
                transform.Rotate(0f,rotateSpeed,0f);
                myAnimator.SetBool("IsRotating", true);
            }
        }
        else
        {
            if (myKeyboard.aKey.isPressed)
            {
                if (isGrounded)
                {
                    transform.Rotate(0f, -rotateSpeed, 0f);
                    myAnimator.SetBool("IsRotating", true);
                }
            }
            else
            {
                myAnimator.SetBool("IsRotating", false);
            }
        }

        if (myMouse.leftButton.isPressed)
        {
            myAnimator.SetBool("IsAiming", true);
            isAiming = true;
        }
        else
        {
            myAnimator.SetBool("IsAiming", false);
            isAiming = false;
        }

        if (myMouse.rightButton.wasPressedThisFrame)
        {
            myAnimator.SetTrigger("ShootTrigger");
        }
        
        // testing anims here...

        if (myKeyboard.digit0Key.wasReleasedThisFrame)
        {
            myAnimator.SetTrigger("DieTrigger");
        }

        if (myKeyboard.pKey.wasReleasedThisFrame)
        {
            myAnimator.SetTrigger("ResetToIdle");
        }
    }

    private void FixedUpdate()
    {

        Vector3 myPosition = transform.position;
        Vector3 slightlyNorth = new Vector3(transform.position.x, transform.position.y, transform.position.z + .3f);
        Vector3 slightlySouth = new Vector3(transform.position.x, transform.position.y, transform.position.z -.3f);
        Vector3 slightlyEast = new Vector3(transform.position.x + .3f, transform.position.y, transform.position.z);
        Vector3 slightlyWest = new Vector3(transform.position.x - .3f, transform.position.y, transform.position.z);
        
        if (Physics.Raycast(myPosition, transform.TransformDirection(Vector3.down), .05f))
        {
            isGrounded = true;
        }
        else
        {
            if (Physics.Raycast(slightlyNorth, transform.TransformDirection(Vector3.down), .05f))
            {
                isGrounded = true;
            }
            else
            {
                if (Physics.Raycast(slightlySouth, transform.TransformDirection(Vector3.down), .05f))
                {
                    isGrounded = true;
                }
                else
                {
                    if (Physics.Raycast(slightlyEast, transform.TransformDirection(Vector3.down), .05f))
                    {
                        isGrounded = true;
                    }
                    else
                    {
                        if (Physics.Raycast(slightlyWest, transform.TransformDirection(Vector3.down), .05f))
                        {
                            isGrounded = true;
                        }
                        else
                        {
                            isGrounded = false;
                        }
                    }
                }
            }
        }
        
        /*
        
        RaycastHit myRCHit;
        Vector3 myPosition = transform.position;
        if (Physics.SphereCast(myPosition, 10f, transform.TransformDirection(Vector3.down),out myRCHit,1f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        
        */
    }

    public void JumpUp()
    {
        myRigidbody.AddForce(new Vector3(0f, jumpStrength, 0f), ForceMode.Impulse);
        myRigidbody.AddForce(transform.forward * jumpStrength * horizontalMovement);
    }
    
}
