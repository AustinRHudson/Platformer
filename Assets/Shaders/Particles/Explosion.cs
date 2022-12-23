using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSys;
    // Start is called before the first frame update
    void Start()
    {
        if (particleSys = null)
            GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    public void playExplodeParticle()
    {
        particleSys.Play();
    }
}
