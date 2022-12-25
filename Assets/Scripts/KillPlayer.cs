using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public SinglePlayerMovement SPM;
    public BoxCollider2D deathBarrier;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(SPM.death());
    }
}
