using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public GameObject virtualCam;
    //public GameObject respawnPoint;
    //public GameObject killCollider;
    //public GameObject background;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(true);
            //respawnPoint.SetActive(true);
            //killCollider.SetActive(true);
            //background.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(false);
            //respawnPoint.SetActive(false);
            //killCollider.SetActive(false);
            //background.SetActive(false);
        }
    }

    private void pause()
    {
        Time.timeScale = 0f;
    }

    private void unPause()
    {
        Time.timeScale = 1f;
    }
}
