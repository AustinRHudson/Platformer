using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaders : MonoBehaviour
{
    Material material;

    bool isDissolving = false;
    bool isFabricating = false;
    //bool isBlinking = false;
    bool onBlink = true;
    float blinkLight = 0f;
    float fade = 1f;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //dissolve();
        //blink();
    }

    void dissolve()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (fade > 0.0f)
            {
                isDissolving = true;
                isFabricating = false;
            }
            else if (fade < 1f)
            {
                isFabricating = true;
                isDissolving = false;
            }

        }

        if (isDissolving)
        {
            fade -= Time.deltaTime;

            if (fade <= 0.0f)
            {
                fade = 0.0f;
                isDissolving = false;
            }

            material.SetFloat("_Fade", fade);
        }

        if (isFabricating)
        {
            fade += Time.deltaTime;

            if (fade >= 1f)
            {
                fade = 1f;
                isFabricating = false;
            }

            material.SetFloat("_Fade", fade);
        }
    }

    void blink()
    {
        if (fade == 0f)
        {
            if (onBlink)
            {
                blinkLight += Time.deltaTime * 0.5f;

            } else if (!onBlink)
            {
                blinkLight -= (Time.deltaTime * 2);
            }
            
            

            if (blinkLight >= 0.2f)
            {
                blinkLight = 0.2f;
                onBlink = false;

            }
            if (blinkLight <= 0)
            {
                blinkLight = 0f;
                onBlink = true;
            }

            material.SetFloat("_Fade", blinkLight);
        }
    }
}
