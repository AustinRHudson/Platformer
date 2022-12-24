using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GravityController gravController;
    public SinglePlayerMovement SPM;

    public UnityEvent onPlayerRespawn;
    void Start()
    {
        rb = SPM.GetComponent<Rigidbody2D>();
    }
    void Awake()
    {
        if (onPlayerRespawn == null)
            onPlayerRespawn = new UnityEvent();
    }
    public void SetRespawnPoint(Vector3 position)
    {
        respawnPoint.position = position;
    }
    public void RespawnPlayer()
    {
        player.position = respawnPoint.position;

        if (rb.gravityScale < 0)
            gravController.flipGravity();
        else
            rb.gravityScale = 4;

        onPlayerRespawn.Invoke();
    }
}
