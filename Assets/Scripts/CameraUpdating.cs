using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUpdating : MonoBehaviour
{
    private Transform tf;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject SPP = GameObject.Find("SinglePlayer_Player");
        tf = SPP.GetComponent<Transform>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(tf.position.x, tf.position.y, -10);
    }
}
