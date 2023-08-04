using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObserverPlayerDetector : MonoBehaviour
{
    public float Damage = 0.005f, DamageScalar = 1f;

    float DamageBufferTime = 0.25f;

    public PlayerManager Player;

    private Transform RayOrigin;

    bool CanDoDamage = true;

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager manager = other.gameObject.GetComponent<PlayerManager>();
        if (manager != null)
        {
            Player = manager;
            StartCoroutine(ApplyDamage());
        }
    }

    private void Awake()
    {
        RayOrigin = transform.parent;
    }

    public IEnumerator ApplyDamage()
    {
        if (CanDoDamage && Player != null)
        {
            RaycastHit HitObject;
            Debug.DrawRay(RayOrigin.position, Player.PlayerPosition(true) - RayOrigin.position, Color.red);
            if (Physics.Raycast(RayOrigin.position, Player.PlayerPosition(true) - RayOrigin.position, out HitObject))
            {
                if (HitObject.transform.GetComponent<PlayerManager>() == Player)
                {
                    CanDoDamage = false;
                    Player.OnDamageRecieved(Damage * DamageScalar);
                    yield return new WaitForSecondsRealtime(DamageBufferTime);
                    StartCoroutine(ApplyDamage());
                    CanDoDamage = true;
                }
                else
                {
                    Debug.Log(HitObject.transform.name);
                }
            }
        }
        yield return null;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == Player.gameObject)
            Player = null;
    }
}
