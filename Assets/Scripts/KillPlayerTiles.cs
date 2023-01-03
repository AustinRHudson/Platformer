using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KillPlayerTiles : MonoBehaviour
{
    public SinglePlayerMovement SPM;
    public TilemapCollider2D tileDeath;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(SPM.death());
    }
}
