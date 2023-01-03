using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collision : MonoBehaviour
{
    public LayerMask groundLayer;
    public LayerMask nonTeleportableObjects;
    public LayerMask nonTeleportableBlocks;
    public bool teleportable;
    public bool isGrounded;
    public bool wasGrounded;
    public bool onWall;
    public float rotation;
    public float collisionRadius = 0.25f;
    public readonly float centerOffsetX = -0.006f;
    public readonly float centerOffsetY = -0.5f;
    public Vector2 bottomOffset, rightOffset, leftOffset, center, centerOffset, playerSize;
    private Color debugCollisionColor = Color.red;
    public UnityEvent onLandEvent;
    public SinglePlayerMovement SPM;

    void Awake() {
        if (onLandEvent == null)
            onLandEvent = new UnityEvent();
    }
    void Start()
    {
        centerOffset = new Vector2(centerOffsetX, centerOffsetY);
        playerSize = new Vector2(0.64f, 1.24f);
        rotation = 0f;
    }

    void FixedUpdate()
    {
        if (!SPM.isTeleporting) 
        center = (Vector2)transform.position + centerOffset;
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer) 
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        wasGrounded = isGrounded;
        isGrounded = false;
        if (Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer)) {
            isGrounded = true;
            if (!wasGrounded) {
                onLandEvent.Invoke();
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
        //Gizmos.DrawWireSphere((Vector2)transform.position + centerOffset, radius);

        Gizmos.DrawWireCube(center, playerSize);
    }
}
