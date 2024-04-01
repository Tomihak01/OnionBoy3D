using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Text healthText;
    public Image healthBar;

    float health, maxhealth = 100;
    float lerpSpeed;

    private void Start()
    {
        health = maxhealth;

    }

    private void Update()
    {
        healthText.text = "Health:" + health + "%";
        if (health > maxhealth) health = maxhealth;

        lerpSpeed = 3f * Time.deltaTime;

        HealthBarFiller();

    }

    void HealthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxhealth, lerpSpeed);

    }

    public void Damage(float damagePoints)
    {
        if (health > 0)
            health -= damagePoints; 
        
    }

    public void Heal(float healingPoints)
    {
        if(health < maxhealth)
            health += healingPoints;

    }


}

