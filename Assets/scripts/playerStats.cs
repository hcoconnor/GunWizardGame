using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerStats : ObjectStats
{

    public Slider Healthbar;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Healthbar.maxValue = health;
    }

    // Update is called once per frame
    new void Update()
    {
        
    }

    public override void hurt(float damage, DamageType dt)
    {
        base.hurt(damage, dt);
        Healthbar.value = health;
    }
}
