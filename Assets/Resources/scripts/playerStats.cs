using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerStats : ObjectStats
{

    public Slider Healthbar;
    public Slider ManaBar;
    public float invincibilityTime;

    public float maxMana;
    [HideInInspector]
    public float mana;

    float damageCooldown = 0;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        mana = maxMana;
        Healthbar.maxValue = health;
        ManaBar.maxValue = mana;
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

    public void castSpell(float cost)
    {
        mana -= cost;
        Mathf.Max(mana, 0);
        ManaBar.value = mana;

    }

    public void restoreMana(float value)
    {
        mana += value;
        Mathf.Min(mana, maxMana);
        ManaBar.value = mana;
    }
}
