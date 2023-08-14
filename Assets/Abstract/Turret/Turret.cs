using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IOverseerListener, ICanBeDisabled
{
    public int IDGroup { get; set; }
    private Transform pivot;
    private PlayerManager currentPlayer;
    private Coroutine trackingCoroutine;

    private int coroutineTriggerCounter = 0; 
    private int raycastTriggerThreshold = 5;

    // Start is called before the first frame update
    void Start()
    {


        pivot = gameObject.transform.Find("Pivot");

        // Validity check on pivot
        if (pivot == null)
        {
            Debug.LogError("Turret has no pivot");
        }

        PlayerManager player = GameObject.Find("Player").GetComponent<PlayerManager>();
        OnOverseerPing(player);

    }



    /// <summary>
    /// OnOverseerPing is called by the OverseerPlayerDetector when the player enters the trigger
    /// its purpose is to set the current player to the player that entered the trigger
    /// and then start the coroutine that will track the player
    /// </summary>



    public void OnOverseerPing(PlayerManager player)
    {
        // Set the current player to the player that entered the trigger if it is not null
        if (player != null)
        {
            currentPlayer = player;
            // Start the coroutine if it is not already running
            if (trackingCoroutine == null)
            {
                trackingCoroutine = StartCoroutine(TrackPlayerCoroutine());
            }
        }
        else
        {
            currentPlayer = null;
            // Stop the coroutine if it is running
            if (trackingCoroutine != null)
            {
                StopCoroutine(trackingCoroutine);
                trackingCoroutine = null;
            }
        }
    }


    /// <summary>
    /// The core purpose of the Track Player Coroutine is to track the player and 
    /// fire a raycast at the player every 5th call to the coroutine.
    /// </summary>


    private IEnumerator TrackPlayerCoroutine()
    {
        while (currentPlayer != null)
        {
            TrackPlayer(currentPlayer);

            coroutineTriggerCounter++; 

            if (coroutineTriggerCounter >= raycastTriggerThreshold)
            {
                coroutineTriggerCounter = 0; // Reset the counter
                FireRaycast(); // Fire the raycast
            }

            yield return new WaitForSeconds(0.05f);
        }
    }


    // TrackPlayer is called by the TrackPlayerCoroutine
    // its purpose is to track the player by rotating the pivot to look at the player
    private void TrackPlayer(PlayerManager player)
    {
        Vector3 playerPositionWithoutY = new Vector3(player.transform.position.x, pivot.position.y, player.transform.position.z);
        pivot.LookAt(2 * pivot.position - playerPositionWithoutY, Vector3.up);
    }

    // FireRaycast is called by the TrackPlayerCoroutine
    // its purpose is to fire a raycast at the player
    private void FireRaycast()
    {
        // Perform the raycast logic here
        RaycastHit hit;
        if (Physics.Raycast(pivot.position, pivot.forward * -1, out hit))
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
            if (hit.collider.gameObject == currentPlayer.gameObject)
            {
                Debug.Log("Player is in sight");
            }
        }
    }



    public void OnActivated()
    {
        return;
    }

    //Implementing ICanBeDisabled Functions
    public void OnDisable()
    {
        return;
    }

}
