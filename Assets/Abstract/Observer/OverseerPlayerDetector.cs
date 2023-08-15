using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using FMODUnity;

public class OverseerPlayerDetector : MonoBehaviour
{
    public const float DAMAGE_BASE = 0.005f;
    public float DamageScalar = 1f;

    float DamageBufferTime = 0.25f;

    public PlayerManager Player;

    private Transform RayOrigin;

    bool CanDoDamage = true;

    public int TurretIDGroup = 0;
    public List<IOverseerListener> Turrets;

    public string
        PlayerSeen = "event:/Overseer/PlayerSeen";

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager manager = other.gameObject.GetComponent<PlayerManager>();
        if (manager != null)
        {
            Player = manager;
            RuntimeManager.PlayOneShot(PlayerSeen, transform.position);

            if (CanDoDamage) 
                StartCoroutine(ApplyDamage());

            GatherTurrets();
            UpdateTurrets(true); //Turrets can try and see the player - this passes a referenece to the turrets to enable them
        }
    }

    private void Awake()
    {
        RayOrigin = transform.parent;
    }

    /// <summary>
    /// Performs a check that the player is within line of sight
    /// if so, apply damage using the OnDamageRecieved method
    /// </summary>
    /// <returns></returns>
    public IEnumerator ApplyDamage()
    {
        if (CanDoDamage && Player != null)
        {
            RaycastHit HitObject;
            //Debug.DrawRay(RayOrigin.position, Player.PlayerPosition(true) - RayOrigin.position, Color.red);
            if (Physics.Raycast(RayOrigin.position, Player.PlayerPosition(true) - RayOrigin.position, out HitObject))
            {
                if (HitObject.transform.GetComponent<PlayerManager>() == Player)
                {
                    CanDoDamage = false;
                    Player.OnDamageRecieved(DAMAGE_BASE * DamageScalar);
                    yield return new WaitForSecondsRealtime(DamageBufferTime);
                    CanDoDamage = true;
                    StartCoroutine(ApplyDamage());
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
        var turrets = FindObjectsOfType<Turret>().OfType<IOverseerListener>();
        foreach (IOverseerListener currentTurret in turrets)
        {
            if (currentTurret.IDGroup == TurretIDGroup)
                Turrets.Add(currentTurret);
        }
    }

    private void UpdateTurrets(bool clear)
    {
        foreach (IOverseerListener turret in Turrets)
        {
            if (clear)
                Turrets.Clear();

            turret.OnOverseerPing(clear ? null : Player); //returns null to overwrite player ref in Turret
        }
    }
    //Uncomment block above on turret complete
}