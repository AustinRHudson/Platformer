using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Animation : MonoBehaviour
{
    public Animator anim;
    public SinglePlayerMovement SPM;
    public Collision collision;
    public GravityFlipper GF;
    public GravityController gravController;

    private bool terminalVelocity = false;
    

    // Start is called before the first frame update
    void Start()
    {
        GameObject SPP = GameObject.Find("SinglePlayer_Player");
        SPM = SPP.GetComponent<SinglePlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetBool("isLanding", false);
            
        }
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1)
        {
            anim.SetBool("isLanding", false);

        }
        anim.SetFloat("Speed", Math.Abs(SPM.getX()));
        anim.SetFloat("VertSpeed", SPM.getVelocityY());
        anim.SetBool("isGrounded", collision.isGrounded);
        anim.SetBool("isFlipped", gravController.getIsFlipped());
        

    }
    public void AlertObservers()
    {
        anim.SetBool("isLanding", false);

        anim.SetBool("isLowJumping", false);


    }
    public void isLandingTrueAnim()
    {
        if (!anim.GetBool("isLowJumping") || terminalVelocity)
        {
            anim.SetBool("isLanding", true);
        }
        anim.SetBool("isJumping",false);
        anim.SetBool("isLowJumping", false);
        terminalVelocity = false;
    }
    public void isLowJumpingTrue()
    {
        anim.SetBool("isLowJumping", true);
    }

    public void isTerminalVelocity()
    {
        terminalVelocity = true;
    }

    public void isTeleportingStartTrue()
    {
        anim.SetBool("isTeleportingStart", true);
    }

    public void isTeleportingStartFalse()
    {
        anim.SetBool("isTeleportingStart", false);
    }

}
