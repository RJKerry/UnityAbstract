using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class OverseerPlayerDetector : MonoBehaviour
{
    public float Damage = 0.005f, DamageScalar = 1f;

    float DamageBufferTime = 0.25f;

    public PlayerManager Player;

    private Transform RayOrigin;

    bool CanDoDamage = true;

    public int TurretIDGroup = 0;
    public List<Turret> Turrets;

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager manager = other.gameObject.GetComponent<PlayerManager>();
        if (manager != null)
        {
            Player = manager;
            StartCoroutine(ApplyDamage());
            UpdateTurrets(true); //Turrets can try and see the player - this passes a referenece to the turrets to enable them
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
        if (other.gameObject == Player.gameObject) 
        { 
            Player = null;
            UpdateTurrets(true); //This will clear the player val of turrets referenced by this script
        }
    }

    /// <summary>
    /// References all turrets and compares ID's, stores a reference if the ID is valid (easy object grouping)
    /// </summary>
    private void GatherTurrets()
    {
        Turret[] turrets = FindObjectsOfType<Turret>();
        foreach (Turret currentTurret in turrets)
        {
            if (currentTurret.ID == TurretIDGroup)
                Turrets.Add(currentTurret);
        }
    }

    void UpdateTurrets(bool clear)
    {
        foreach (Turret turret in Turrets)
        {
            turret.PassPlayerRef(clear ? null : Player);
        }
    }

}