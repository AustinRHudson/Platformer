using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Implosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSys;
    [SerializeField] private Transform PlayerTF;


    private void Start()
    {
        if (particleSys == null)
        {
            GetComponent<ParticleSystem>();
        }
    }

    public void play()
    {
        particleSys.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particleSys.Emit(20);
        ManageParticles();       
    }

    private void ManageParticles()
    {
        List<Vector4> customData = new List<Vector4>();

        particleSys.GetCustomParticleData(customData, ParticleSystemCustomData.Custom1);

        Vector4 centerPosition = PlayerTF.position;

        for (int i = 0; i < customData.Count; i++)
        {
            customData[i] = centerPosition;
        }
        particleSys.SetCustomParticleData(customData, ParticleSystemCustomData.Custom1);
    }
}
