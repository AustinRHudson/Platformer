using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigid;
    public float speed = 10;
    private float x;
    private float y;
    private Vector2 dir;
    [SerializeField] private Transform tf;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }  

    public void move(float x, float y) {
        dir = new Vector2(x, y);
        rigid.velocity = (new Vector2(dir.x * speed, dir.y * speed));
        if (x < 0)
        {
            tf.localScale = new Vector3 (-7, 7, 7);
        } else if (x > 0)
        {
            tf.localScale = new Vector3(7, 7, 7);
        }
    }


}
