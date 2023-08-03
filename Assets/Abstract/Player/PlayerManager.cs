using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : HealthHandler
{
    public Vector3 PlayerPosition()
    { return transform.position; }
}
