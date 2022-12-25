using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private Death death= null;
    private float timer = 0;

    void Update()
    {
        timer -= Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (timer < 0)
        {
            if (other.CompareTag("Player"))
            {
                death.KillPlayer();
            }
            timer = 0.5f;
        }
    }
}
