using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : HealthHandler
{
    CharacterController playerCapsule;

    public override void Init()
    {
        base.Init();
        DontDestroyOnLoad(this.gameObject);
        playerCapsule = GetComponent<CharacterController>();
    }

    public Vector3 PlayerPosition(bool AddHalfHeightToUp)
    { return AddHalfHeightToUp ? transform.position + Vector3.up * playerCapsule.height / 2 : transform.position; }
}