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
    void OnDamage();
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
    void OnActivated();
    int GetID();
}