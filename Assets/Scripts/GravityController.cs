using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using System;

public class GravityController : MonoBehaviour
{
    //bool
    public bool gravityFlipped;
    private bool isSwitchingPos;
    private bool isSwitchingNeg;
    private bool gravityVelocityCheck = true;

    //floats
    private float deltaGravity = 0f;
    public float deltaSpeed;
    

    //ref
    private GameObject SPP;
    private Rigidbody2D rb;
    private Transform tf;
    [SerializeField] private SinglePlayerMovement SPM;
    [SerializeField] private Collision collision;

    //objects
    public UnityEvent onGravityFlip; 

    void FixedUpdate()
    {
        //Debug.Log("Delta Speed: " + deltaSpeed);
        //Debug.Log(rb.velocity.y);
    }

    //update
    void Update()
    {
        if (SPM.movementEnabled)
        {
            if (collision.isGrounded)
            {
                gravityVelocityCheck = true;
                SPM.terminalVelocity = 25;
            }
            if (isSwitchingNeg)
            {
                deltaGravity -= Time.deltaTime * deltaSpeed;
                if (deltaGravity < -4)
                {
                    deltaGravity = -4;
                    isSwitchingNeg = false;
                }
                rb.gravityScale = deltaGravity;
            }
            else if (isSwitchingPos)
            {
                deltaGravity += Time.deltaTime * deltaSpeed;
                if (deltaGravity > 4)
                {
                    deltaGravity = 4;
                    isSwitchingPos = false;
                }
                rb.gravityScale = deltaGravity;
            }
        }
    }
 
    //get components
    void Start()
    {
        SPP = GameObject.Find("SinglePlayer_Player");
        rb = SPP.GetComponent<Rigidbody2D>();
        tf = SPP.GetComponent<Transform>();
    }
    //event startup
    void Awake()
    {
        if (onGravityFlip == null) { onGravityFlip = new UnityEvent(); }
    }

    //flip command
    public void flipGravity()
    {
       
        if (rb.gravityScale > 0)
        {
            gravityFlipped = true;
            collision.bottomOffset = new Vector2(0, 1.15f);
            collision.centerOffset.y = -collision.centerOffsetY;
            if (gravityVelocityCheck)
            {
                SPM.terminalVelocity = Math.Abs(rb.velocity.y);
                //Debug.Log(rb.velocity.y);
                deltaSpeed = 100 / (.5f*(Math.Abs(Math.Abs(rb.velocity.y) - 10)));
                //Debug.Log("Delta Speed: " + deltaSpeed);
                //Debug.Log(rb.velocity.y);
                gravityVelocityCheck = false;
            }
            deltaGravity = 4;
            isSwitchingNeg = true;
            tf.localScale = new Vector3(tf.localScale.x, -7, tf.localScale.z);
            SPM.canTeleport = true;
            //Debug.Log("Gravity Down");
        }
        else if (rb.gravityScale < 0)
        {
            gravityFlipped = false;
            collision.bottomOffset = new Vector2(0, -1.15f);
            collision.centerOffset.y = collision.centerOffsetY;
            if (gravityVelocityCheck)
            {
                SPM.terminalVelocity = Math.Abs(rb.velocity.y);
                //Debug.Log(rb.velocity.y);
                deltaSpeed = 100 / (.5f*(Math.Abs(Math.Abs(rb.velocity.y) - 10)));
                //Debug.Log(deltaSpeed);
                //Debug.Log(rb.velocity.y);
                gravityVelocityCheck = false;
            }
            deltaGravity = -4;
            isSwitchingPos = true;
            tf.localScale = new Vector3(tf.localScale.x, 7, tf.localScale.z);
            //Debug.Log("Gravity Up");
        }
        onGravityFlip.Invoke();
        //Debug.Log("FLIPPED");
    }
 
    //get variable
    public void setGravityFlipped(bool isFlipped)
    {
        gravityFlipped = isFlipped;
    }
    public bool getIsFlipped()
    {
        return gravityFlipped;
    }   
    //flip command pad (Not Used)
    public void flipGravityPad()
    {
        if (rb.gravityScale > 0)
        {
            gravityFlipped = true;
            collision.bottomOffset = new Vector2(0, 1.15f);
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * -1);
            rb.gravityScale = -4;
            tf.localScale = new Vector3(tf.localScale.x, -7, tf.localScale.z);
            //Debug.Log("Gravity Down");
        }
        else if (rb.gravityScale < 0)
        {
            gravityFlipped = false;
            collision.bottomOffset = new Vector2(0, -1.15f);
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * -1);
            rb.gravityScale = 4;
            tf.localScale = new Vector3(tf.localScale.x, 7, tf.localScale.z);
           // Debug.Log("Gravity Up");
        }
        onGravityFlip.Invoke();
        //Debug.Log("FLIPPED");
    }
    //unlimited velocity, capped at terminal velocity (not used)
    public void flipGravityInfiniteMomentum()
    {
        if (rb.gravityScale > 0)
        {
            gravityFlipped = true;
            collision.bottomOffset = new Vector2(0, 1.15f);
            Debug.Log(rb.velocity.y);
            Debug.Log(deltaSpeed);
            deltaSpeed = 100 / (Math.Abs(rb.velocity.y) - 10);
            Debug.Log(deltaSpeed);
            Debug.Log(rb.velocity.y);
            deltaGravity = 4;
            isSwitchingNeg = true;
            tf.localScale = new Vector3(tf.localScale.x, -7, tf.localScale.z);
            Debug.Log("Gravity Down");
        }
        else if (rb.gravityScale < 0)
        {
            gravityFlipped = false;
            collision.bottomOffset = new Vector2(0, -1.15f);
            Debug.Log(rb.velocity.y);
            Debug.Log(deltaSpeed);
            deltaSpeed = 100 / (Math.Abs(rb.velocity.y) - 10);
            Debug.Log(deltaSpeed);
            Debug.Log(rb.velocity.y);
            deltaGravity = -4;
            isSwitchingPos = true;
            tf.localScale = new Vector3(tf.localScale.x, 7, tf.localScale.z);
            Debug.Log("Gravity Up");
        }
        onGravityFlip.Invoke();
        Debug.Log("FLIPPED");
    }
}
