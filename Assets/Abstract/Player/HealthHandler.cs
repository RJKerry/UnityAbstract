using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour, IDamageable
{
    public float _health; //ACTUAL VAL
    public float Health { get => _health; set => _health = value; } //TEMPLATE BIND

    public float GetMaxHealthDefault(float MAX_HEALTH_DEFAULT = 1)
    {
        return MAX_HEALTH_DEFAULT;
    }

    private void Awake()
    {
        Init();
    }

    public virtual void Init() 
    {
        Health = GetMaxHealthDefault();
    }

    public void OnDamageRecieved(float damage)
    {
        if((Health -= damage) < 0) 
        {
            HealthDepleted();
            return;
        }
        DamageEffect(damage);
    }

    //Extend me in children
    public virtual void DamageEffect(float damage) 
    {
        Debug.Log(this.gameObject.name +": "+ damage + " damage recieved, " + Health + "Health Remaining");
    }

    public void HealthDepleted()
    {
        DeathEffect();
    }

    //Extend me in children
    public virtual void DeathEffect()
    {
        Debug.Log(this.gameObject.name);
    }
}