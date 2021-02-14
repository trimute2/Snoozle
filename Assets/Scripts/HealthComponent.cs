using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//based off of this old script: https://github.com/trimute2/FightToTheTop/blob/master/Assets/None%20Plugins/Scripts/ComponentScripts/HealthComponent.cs

public class HealthComponent : MonoBehaviour
{
    public delegate void HealthUpdate();
    public event HealthUpdate HealthUpdateEvent;
    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

    public int maxHealth = 100;
    private int health;
    public int Health
    {
        get
        {
            return health;
        }
    }

    private bool deathCalled;

    private void Start()
    {
        health = maxHealth;
        deathCalled = false;
    }

    public bool Damage(int damage, Vector2 knockBack, float stunDuration = 0.3f, int stunPoints = 0)
    {

        health -= damage;
        //call health update event
        if (HealthUpdateEvent != null)
        {
            HealthUpdateEvent();
        }
        if (health <= 0 && !deathCalled)
        {
            OnDeath();
            deathCalled = true;
        }
        return true;
    }
}
