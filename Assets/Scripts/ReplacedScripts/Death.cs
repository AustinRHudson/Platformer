using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Death : MonoBehaviour
{
    [SerializeField] private float deathTime;
    [SerializeField] private Respawn respawn;
    public SinglePlayerMovement SPM;
    private Rigidbody2D rb;
    public UnityEvent onPlayerDeath;
    public int deathCounter;

    void Start()
    {
        rb = SPM.GetComponent<Rigidbody2D>();
    }
    void Awake()
    {
        if (onPlayerDeath == null)
            onPlayerDeath = new UnityEvent();
    }
    public void KillPlayer()
    {
        StartCoroutine(KillPlayerCoroutine());
    }
    IEnumerator KillPlayerCoroutine()
    {
        onPlayerDeath.Invoke();
        deathCounter++;
        rb.velocity = new Vector3(0,0,0);
        if (rb.gravityScale < 0)
        {
            rb.gravityScale = -0.0000001f;
        } else
        {
            rb.gravityScale = 0.0000001f;
        }
        
        Debug.Log("Run Kill");
        yield return new WaitForSeconds(deathTime);
        respawn.RespawnPlayer();
        //add disable SPM later
    }

}
