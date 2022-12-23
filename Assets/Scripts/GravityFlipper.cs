using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GravityFlipper : MonoBehaviour
{
    //floats
    private float timer = 0f;

    [SerializeField] private GravityController gravController;

    void Update() 
    {
        timer -= Time.deltaTime;
    }


    void OnTriggerExit2D(Collider2D other) {
        if (timer <= 0) 
        {
            if (other.CompareTag("Player")) 
            {
                gravController.flipGravity();
                timer = .2f;
            }
        }
    }
}

