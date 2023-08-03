using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObserverPlayerDetector : MonoBehaviour
{
    public float Damage = 0.05f, DamageScalar = 1f;

    public PlayerManager Player;

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager manager = other.gameObject.GetComponent<PlayerManager>();
        if (manager != null)
        {
            Player = manager;
        }
    }

    private void Update() //This will need changing eventually to not rapidfire
    {
        if (Player != null) 
        {
            RaycastHit HitObject;
            if (Physics.Raycast(transform.position, transform.position-Player.PlayerPosition().normalized, out HitObject))
                if(HitObject.transform.GetComponent<PlayerManager>() == Player)
                    Player.OnDamageRecieved(Damage * DamageScalar);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == Player.gameObject)
            Player = null;
    }

}
