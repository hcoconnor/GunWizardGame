using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerStats : ObjectStats
{

    public GameObject looseScreen; 

    public Slider Healthbar;
    public Slider ManaBar;
    public TextMeshProUGUI scoreText;

    public float invincibilityTime;

    public float maxMana;
    [HideInInspector]
    public float mana;
    [HideInInspector]
    public int points;

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

        scoreText.text = "Score: " + points;
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
        if(health <= 0)
        {
            Die();
            PlayerPrefs.SetInt("highScore", Mathf.Max(points, PlayerPrefs.GetInt("highScore")));
        }
        
    }

    public void Die()
    {
        Time.timeScale = 0;
        looseScreen.active = true;
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
