using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class explosion : MonoBehaviour
{

    //ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        
        //Debug.Log("ps"+ps);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setExplosionSize(float radius)
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
        main.startLifetimeMultiplier = radius / main.startSpeedMultiplier;
        ps.Play();
    }
}
