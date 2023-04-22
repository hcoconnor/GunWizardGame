using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStats : MonoBehaviour
{
    // Start is called before the first frame update\
    public float maxHealth = 10;
    [HideInInspector]
    public float health;

    protected void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    protected void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual void hurt(float damage, DamageType dt)
    {
        health -= damage;
    }
}
