using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Paralax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;
    private float cloudSpeed;
    public float cloudIncrement;


    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);
        cloudSpeed += cloudIncrement;
        transform.position = new Vector3(startpos + dist + cloudSpeed, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}