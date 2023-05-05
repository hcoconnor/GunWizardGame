using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerStats : ObjectStats
{

    public Slider Healthbar;
    public float invincibilityTime;

    float damageCooldown = 0;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Healthbar.maxValue = health;
    }

    // Update is called once per frame
    new void Update()
    {
        damageCooldown -= Time.deltaTime;
        damageCooldown = Mathf.Max(0, damageCooldown);
    }

    public override void hurt(float damage, DamageType dt)
    {
        //Debug.Log("iframes:" + damageCooldown);
        if(damageCooldown <= 0)
        {
            base.hurt(damage, dt);
            Healthbar.value = health;
            damageCooldown = invincibilityTime;
        }
        
    }
}
