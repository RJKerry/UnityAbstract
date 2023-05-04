using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void OnInteract();
}

public interface IDamageable
{
    void OnDamage();
}

public interface ICanBeDisabled
{
    void OnDisableInteract();
}