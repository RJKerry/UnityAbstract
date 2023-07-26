using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void OnInteract(PlayerInputManager messageSource);
}

public interface IDamageable
{
    public float Health { get; set; }
    float GetMaxHealthDefault(float MAX_HEALTH_DEFAULT = 1.0f); //Do not override
    public void OnDamage(float damage);
}

public interface ICanBeDisabled
{
    void OnDisableInteract();
}

public interface ICanBeGrabbed
{
    void OnGrabbed(Vector3 hitPoint);
}

public interface ITerminalListener
{
    public int IDGroup { get; set; } //Interface has field IDGroup, implementing classes can have ID field this hooks into
    void OnActivated();
}