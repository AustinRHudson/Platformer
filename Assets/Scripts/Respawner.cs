using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public BoxCollider2D newRepsawnPoint;
    public SinglePlayerMovement SPM;

    void Start()
    {
        newRepsawnPoint = GetComponent<BoxCollider2D>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        SPM.respawnPoint = newRepsawnPoint;
    }
}
