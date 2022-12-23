using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class SinglePlayerMovement : MonoBehaviour
{
    //booleans

    public bool wallGrab;
    public bool canTeleport;
    public bool isTeleporting;
    public bool movementEnabled;
    //ints
    public int direction;


    //floats
    private float y;
    public float x;
    public float speed;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public float slideSpeed;
    public float jumpForce;
    public float terminalVelocity;
    public float centerMaxX;
    public float centerMaxY;
    public float teleportDistance = 50f;
    private float diagonalTeleportDistance;
    public float teleportCooldown;
    public float teleportCooldownTimer;
    public float teleportPause;
    public float tempVelX;
    public float tempGrav;

    //references
    [SerializeField] private Collision collision = null;
    [SerializeField] private Transform tf = null;
    [SerializeField] private GravityController gravController = null;
    private Rigidbody2D rb = null;
    public Animator anim = null;


    //objects
    private Vector2 dir;
    public UnityEvent onLowJump;
    public UnityEvent onTerminalVelocity;
    public UnityEvent onTeleportStart;
    public UnityEvent onTeleportEnd;

    //start
    public void init()
    {
        diagonalTeleportDistance = (float)Math.Sqrt((Math.Pow(teleportDistance, 2.0)) / 2.0);
        teleportCooldownTimer = 2f;
        teleportCooldown = 1f;
        teleportPause = .1f;
        speed = 10f;
        fallMultiplier = 3f;
        lowJumpMultiplier = 6f;
        slideSpeed = 5;
        jumpForce = 650f;
        terminalVelocity = 25f;
        isTeleporting = false;
        movementEnabled = true;

}
    void Start()
    {
        init();
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Awake()
    {
        if (onLowJump == null) { onLowJump = new UnityEvent(); }
        if (onTerminalVelocity == null) { onTerminalVelocity = new UnityEvent(); }
        if (onTeleportStart == null) { onTeleportStart = new UnityEvent(); }
        if (onTeleportEnd == null) { onTeleportEnd = new UnityEvent(); }
    }

    //update
    void Update()
    {
        //Dash
        if (collision.isGrounded && !isTeleporting)
        {
            canTeleport = true;
        }
        else if (isTeleporting)
        {
            canTeleport = false;
        }
        if (teleportCooldownTimer > 0)
            teleportCooldownTimer -= Time.deltaTime;
        getDirection();
        teleportationDash();
        //walk
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        dir = new Vector2(x, y);
        walk(dir);

        //jump
        if (Input.GetButtonDown("Jump") && collision.isGrounded) {
            jump();
        }
        betterJump();
        //Terminal Velocity
        if (rb.velocity.y < -1 * terminalVelocity)
        {
            rb.velocity = new Vector2(dir.x * 10, -1 * terminalVelocity);
        }
        else if(rb.velocity.y > terminalVelocity)
        {
            rb.velocity = new Vector2(dir.x * 10, terminalVelocity);
        }
        terminalVelocityCheck();
        //Wall Grab and sliding
        //wallGrab = collision.onWall && Input.GetKey(KeyCode.LeftShift);
        //slide();
        //wallGrabCheck();
    }

    //walk
    private void walk(Vector2 dir) {
        if (movementEnabled)
        {
            rb.velocity = (new Vector2(dir.x * speed, rb.velocity.y));
            if (dir.x < 0)
            {
                tf.localScale = new Vector3(-7, tf.localScale.y, tf.localScale.z);
            }
            else if (dir.x > 0)
            {
                tf.localScale = new Vector3(7, tf.localScale.y, tf.localScale.z);
            }
        }

    }

    //jump
    private void jump() {
        if (movementEnabled)
        {
            if (rb.gravityScale > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, jumpForce));
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, -1 * jumpForce));
            }
            anim.SetBool("isJumping", true);
        }

    }
    private void betterJump() {
        if (movementEnabled)
        {
            if (rb.gravityScale > 0)
            {

                if (rb.velocity.y < 0)
                {
                    rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

                }
                else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
                {
                    rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
                    onLowJump.Invoke();
                }
            }
            else
            {

                if (rb.velocity.y > 0)
                {
                    rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime * -1;

                }
                else if (rb.velocity.y < 0 && !Input.GetButton("Jump"))
                {
                    rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime * -1;
                    onLowJump.Invoke();
                }
            }
        }

    }
    //Wall Grab
    private void wallGrabCheck()
    {
        if (wallGrab)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        else
        {
            if (gravController.getIsFlipped())
            {
                rb.gravityScale = -4;
            }
            else
            {
                rb.gravityScale = 4;
            }
        }
    }
    //Sliding
    private void slide() {

        if (collision.onWall && !collision.isGrounded && movementEnabled)
        {
            if (rb.gravityScale > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, slideSpeed);
            }
        }
    }
    //Terminal Velocity Check
    private void terminalVelocityCheck()
    {
        if (rb.velocity.y == -1 * terminalVelocity)
        {
            onTerminalVelocity.Invoke();
        }
        if (rb.velocity.y == terminalVelocity)
        {
            onTerminalVelocity.Invoke();
        }
    }
    //Teleportation Dash
    private void teleportationDash()
    {
        if (Input.GetButtonDown("Dash") && movementEnabled)
        {
            switch (direction)
            {
               case 0:
                    centerMaxX = collision.center.x;
                    centerMaxY = collision.center.y;
                    for (float i = diagonalTeleportDistance; i > 0f; i -= 0.2f)
                    {
                        collision.center = new Vector2(centerMaxX - i, centerMaxY - i);
                        collision.teleportable = false;
                        if (!Physics2D.OverlapBox(collision.center, collision.playerSize, collision.rotation, collision.nonTeleportableObjects))
                        {
                            collision.teleportable = true;
                            break;
                        }
                    }
                    break;
                case 1:
                    centerMaxX = collision.center.x;
                    centerMaxY = collision.center.y;
                    for (float i = teleportDistance; i > 0f; i -= 0.2f)
                    {
                        collision.center = new Vector2(centerMaxX - i, centerMaxY);
                        collision.teleportable = false;
                        if (!Physics2D.OverlapBox(collision.center, collision.playerSize, collision.rotation, collision.nonTeleportableObjects))
                        {
                            collision.teleportable = true;
                            break;
                        }
                    }
                    break;
                case 2:
                    centerMaxX = collision.center.x;
                    centerMaxY = collision.center.y;
                    for (float i = diagonalTeleportDistance; i > 0f; i -= 0.2f)
                    {
                        collision.center = new Vector2(centerMaxX - i, centerMaxY + i);
                        collision.teleportable = false;
                        if (!Physics2D.OverlapBox(collision.center, collision.playerSize, collision.rotation, collision.nonTeleportableObjects))
                        {
                            collision.teleportable = true;
                            break;
                        }
                    }
                    break;
                case 3:
                    centerMaxX = collision.center.x;
                    centerMaxY = collision.center.y;
                    for (float i = teleportDistance; i > 0f; i -= 0.2f)
                    {
                        collision.center = new Vector2(centerMaxX, centerMaxY + i);
                        collision.teleportable = false;
                        if (!Physics2D.OverlapBox(collision.center, collision.playerSize, collision.rotation, collision.nonTeleportableObjects))
                        {
                            collision.teleportable = true;
                            break;
                        }
                    }
                    break;
                case 4:
                    centerMaxX = collision.center.x;
                    centerMaxY = collision.center.y;
                    for (float i = diagonalTeleportDistance; i > 0f; i -= 0.2f)
                    {
                        collision.center = new Vector2(centerMaxX + i, centerMaxY + i);
                        collision.teleportable = false;
                        if (!Physics2D.OverlapBox(collision.center, collision.playerSize, collision.rotation, collision.nonTeleportableObjects))
                        {
                            collision.teleportable = true;
                            break;
                        }
                    }
                    break;
                case 5:
                    centerMaxX = collision.center.x;
                    centerMaxY = collision.center.y;
                    for (float i = teleportDistance; i > 0f; i -= 0.2f)
                    {
                        collision.center = new Vector2(centerMaxX + i, centerMaxY);
                        collision.teleportable = false;
                        if (!Physics2D.OverlapBox(collision.center, collision.playerSize, collision.rotation, collision.nonTeleportableObjects))
                        {
                            collision.teleportable = true;
                            break;
                        }
                    }
                    break;
                case 6:
                    centerMaxX = collision.center.x;
                    centerMaxY = collision.center.y;
                    for (float i = diagonalTeleportDistance; i > 0f; i -= 0.2f)
                    {
                        collision.center = new Vector2(centerMaxX + i, centerMaxY - i);
                        collision.teleportable = false;
                        if (!Physics2D.OverlapBox(collision.center, collision.playerSize, collision.rotation, collision.nonTeleportableObjects))
                        {
                            collision.teleportable = true;
                            break;
                        }
                    }
                    break;
                case 7:
                    centerMaxX = collision.center.x;
                    centerMaxY = collision.center.y;
                    for (float i = teleportDistance; i > 0f; i -= 0.2f)
                    {
                        collision.center = new Vector2(centerMaxX, centerMaxY - i);
                        collision.teleportable = false;
                        if (!Physics2D.OverlapBox(collision.center, collision.playerSize, collision.rotation, collision.nonTeleportableObjects))
                        {
                            collision.teleportable = true;
                            break;
                        }
                    }
                    break;
            }
            if (canTeleport && collision.teleportable && teleportCooldownTimer <= 0)
            {
                StartCoroutine(teleport(collision.center.x, collision.center.y));
                
            }
        }
    }
    IEnumerator teleport(float x, float y)
    {
        isTeleporting = true;
        onTeleportStart.Invoke();
        canTeleport = false;
        movementEnabled = false;
        tempVelX = rb.velocity.x;
        tempGrav = rb.gravityScale;
        teleportCooldownTimer = teleportPause;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(teleportPause);
        if (tempGrav > 0)
            rb.gravityScale = 4;
        else
            rb.gravityScale = -4;
        tf.position = (new Vector2(x, y)) - collision.centerOffset;
        rb.velocity = new Vector2(tempVelX, 0);
        movementEnabled = true;
        isTeleporting = false;
        onTeleportEnd.Invoke();
    }
    //Get Direction
    private void getDirection()
    {
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                direction = 0;
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                direction = 2;
            }
            else
            {
                direction = 1;
            }
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                direction = 6;
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                direction = 4;
            }
            else
            {
                direction = 5;
            }
        }
        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            direction = 3;
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            direction = 7;
        }

    }
    public void disableMovement()
    {
        movementEnabled = false;
        //Debug.Log("mvmt false");
    }
    
    public void enableMovement()
    {
        movementEnabled = true;
        //Debug.Log("mvmt true");
    }

    //gets
    public float getX()
    {
        return x;
    }

    public float getVelocityY()
    {
        return rb.velocity.y;
    }
    public void debugLow()
    {
        //Debug.Log("LowJump");
    }
    public void debugTerminalVelocity()
    {
        //Debug.Log("TerminalVelocity");
    }





}
