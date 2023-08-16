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
    public bool hasTurrets = false;

    public string
        PlayerSeen = "event:/Overseer/PlayerSeen",
        PlayerLost = "event:/Overseer/PlayerLost";

    public OverseerViewTargeter viewTargeter;
    float playerTrackDuration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager manager = other.gameObject.GetComponent<PlayerManager>();
        if (manager != null)
        {
            Player = manager;

            RuntimeManager.PlayOneShot(PlayerSeen, transform.position);

            if (CanDoDamage) 
                StartCoroutine(ApplyDamage());

            if (hasTurrets)
            {
                print("Overseer: " + transform.root.name + " has turrets enabled");
                //Turrets can try and see the player - this passes a referenece to the turrets to enable them

                GatherTurrets();
                UpdateTurrets(false);
            }
             //if its null nothing will happen else it will be cleared
            //if(hasTurrets) GatherTurrets(); //hence only updating needs to fall within the check. GatherTurrets will just return null
        }
    }

    private void Awake()
    {
        RayOrigin = transform.parent; //this objeccts pivot 
        viewTargeter = transform.root.GetComponentInChildren<OverseerViewTargeter>();

        if(hasTurrets) Turrets = new List<IOverseerListener>();
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
                    viewTargeter.PlayerSeen(playerTrackDuration, Player.gameObject); //this will allow the overseer to track the player for as long as it is dealing damage
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
            RuntimeManager.PlayOneShotAttached(PlayerLost, transform.root.gameObject);
            Player = null;
            if(hasTurrets) UpdateTurrets(true); //This will clear the player val of turrets referenced by this script
        }
    }

    /// <summary>
    /// References all turrets and compares ID's, stores a reference if the ID is valid (easy object grouping)
    /// </summary>
    private void GatherTurrets()
    {
        var turrets = FindObjectsOfType<Turret>().OfType<IOverseerListener>();
        
        if(turrets == null)
            return;

        foreach (IOverseerListener currentTurret in turrets)
        {
            if (currentTurret.IDGroup == TurretIDGroup)
                Debug.Log(currentTurret);
                Turrets.Add(currentTurret);
        }
    }

    /// <summary>
    /// Pings turrets to update player ref based on detection,
    /// clears the array if cutting off / clearing player refs
    /// </summary>
    /// <param name="clear"></param>
    private void UpdateTurrets(bool clear)
    {
        if (Turrets == null) //no turrets? nothing to clear, nothing to update.
            return;

        foreach (IOverseerListener turret in Turrets) //send out messages
        {
            Debug.Log("Hitting Turrets");
            turret.OnOverseerPing(clear ? null : Player); //returns null to overwrite player ref in turrets - important for turret functionality
        }
        if (clear)
        {
            Debug.Log("Clearing Turrets");
            Turrets.Clear();

        } //dispose of all turrets
            
    }
}