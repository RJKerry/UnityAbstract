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
        Health = GetMaxHealthDefault();
    }

    public void OnDamageRecieved(float damage)
    {
        Health -= damage;
        if(Health < 0 ) 
        {
            HealthDepleted();
        }
    }

    public virtual void HealthDepleted()
    {
        Debug.Log("Health 0 for " + this.gameObject.name);
    }
}