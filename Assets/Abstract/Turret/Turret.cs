using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IOverseerListener, ICanBeDisabled
{
    public int IDGroup { get; set; }
    public GameObject bulletPrefab;
    [Range(0f, 100f)]
    public int bulletSpeed;
    [Range(0f, 1000f)]
    public int rotationSpeed;
    private Transform pivot;
    private Transform barrel;
    private PlayerManager currentPlayer;
    private Coroutine trackingCoroutine;

    private int coroutineTriggerCounter = 0; 
    private int raycastTriggerThreshold = 12;

    // Start is called before the first frame update
    void Start()
    {


        pivot = gameObject.transform.Find("Pivot");
        barrel = pivot.GetChild(0).transform.Find("Barrel");

        // Validity check on pivot
        if (pivot == null)
        {
            Debug.LogError("Turret has no pivot");
        }

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
                FireShot(); // Fire the raycast
            }

            yield return new WaitForSecondsRealtime(0.02f);
        }
    }


    // TrackPlayer is called by the TrackPlayerCoroutine
    // its purpose is to track the player by rotating the pivot to look at the player
    private void TrackPlayer(PlayerManager player)
    {
        Vector3 playerPositionWithoutY = new Vector3(player.transform.position.x, pivot.position.y, player.transform.position.z);

        // Calculate the target rotation to look at the player
        Quaternion targetRotation = Quaternion.LookRotation(pivot.position - playerPositionWithoutY);

        // Smoothly rotate towards the target rotation
        pivot.rotation = Quaternion.RotateTowards(pivot.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }


    // FireRaycast is called by the TrackPlayerCoroutine
    // its purpose is to fire a raycast at the player
    private void FireShot()
    {
        // Perform the raycast logic here
        RaycastHit hit;
        if (Physics.Raycast(pivot.position, pivot.forward * -1, out hit))
        {

            // Calculate the direction from the barrel to the hit point
            Vector3 bulletDirection = hit.point - barrel.position;

            // Calculate the rotation needed for the bullet to face the hit point
            Quaternion bulletRotation = Quaternion.LookRotation(bulletDirection);

            // Instantiate the bullet prefab and set its initial position and rotation
            GameObject bullet = Instantiate(bulletPrefab, barrel.position + bulletDirection.normalized, bulletRotation);

            // Get the Rigidbody component from the instantiated bullet
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

            // Set the bullet's initial velocity to move it forward
            bulletRigidbody.velocity = bulletDirection.normalized * bulletSpeed; // Adjust the bulletSpeed as needed
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
