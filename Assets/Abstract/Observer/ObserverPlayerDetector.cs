using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObserverPlayerDetector : MonoBehaviour
{
    public float Damage = 0.05f, DamageScalar = 1f;

    public PlayerManager Player;

    private Transform RayOrigin;

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager manager = other.gameObject.GetComponent<PlayerManager>();
        if (manager != null)
        {
            Player = manager;
        }
    }

    private void Awake()
    {
        RayOrigin = transform.parent;
    }

    private void Update() //This will need changing eventually to not rapidfire
    {
        if (Player != null)
        {
            Debug.Log("Not Null");
            RaycastHit HitObject;

            Debug.DrawRay(RayOrigin.position, Player.PlayerPosition(true) - RayOrigin.position, Color.cyan);
            if (Physics.Raycast(RayOrigin.position, Player.PlayerPosition(true) - RayOrigin.position, out HitObject))
            {
                if (HitObject.transform.GetComponent<PlayerManager>() == Player)
                {
                    Player.OnDamageRecieved(Damage * DamageScalar);
                }
                else
                    Debug.Log("Hit object is not player");
            }
            else
                Debug.Log("Cast missed");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == Player.gameObject)
            Player = null;
    }
}
