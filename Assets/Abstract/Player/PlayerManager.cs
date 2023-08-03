using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : HealthHandler
{
    CapsuleCollider playerCapsule;

    private void Awake()
    {
        playerCapsule = GetComponent<CapsuleCollider>();
    }

    public Vector3 PlayerPosition(bool AddHalfHeightToUp)
    { return AddHalfHeightToUp ? transform.position : transform.position + Vector3.up * playerCapsule.height / 2; }
}
