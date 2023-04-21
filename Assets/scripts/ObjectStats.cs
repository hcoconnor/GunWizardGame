using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStats : MonoBehaviour
{
    // Start is called before the first frame update\

    public float health = 10;

    protected void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void hurt(float damage, DamageType dt)
    {
        health -= damage;
    }
}
