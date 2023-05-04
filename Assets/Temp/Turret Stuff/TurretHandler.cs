using System.Collections.Generic;
using UnityEngine;

public class TurretHandler : MonoBehaviour
{
    public Transform player;
    public List<Transform> turrets;
    public float lookSpeed;
    public GameObject explosionPrefab;
    public float activationDistance;

    private Dictionary<Transform, bool> turretStatuses = new Dictionary<Transform, bool>();
    private Dictionary<Transform, GameObject> explosionObjects = new Dictionary<Transform, GameObject>();

    private Dictionary<Transform, Quaternion> initialRotations = new Dictionary<Transform, Quaternion>();

    private void Start()
    {
        // Initialize the turret statuses to be enabled
        foreach (Transform turret in turrets)
        {
            turretStatuses[turret] = true;

            // Save the initial rotation of the turret top
            Transform turretTop = turret.Find("Top");
            initialRotations[turretTop] = turretTop.rotation;
        }
    }

    private void Update()
    {
        foreach (Transform turret in turrets)
        {
            if (!turretStatuses[turret])
            {
                // Skip this turret if it's disabled
                continue;
            }

            // calculate the dot product between the turret's forward direction and the vector from the turret to the player
            Vector3 toPlayer = player.position - turret.position;
            float dot = Vector3.Dot(turret.forward, toPlayer.normalized);
            float distance = toPlayer.magnitude;
            if (dot > 0.0f && dot > Mathf.Cos(Mathf.PI / 3.0f) && distance < activationDistance && distance > 1f)
            {
                Vector3 targetDirection = player.position - turret.Find("Top").position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                turret.Find("Top").rotation = Quaternion.Slerp(turret.Find("Top").rotation, targetRotation, lookSpeed * Time.deltaTime);
            }
            // otherwise, rotate the top of the turret freely
            else
            {
                Transform turretTop = turret.Find("Top");

                // Calculate the new rotation angle
                float newAngle = Mathf.PingPong(Time.time * 30f * lookSpeed, 180f) - 90f;
                Quaternion newRotation = initialRotations[turretTop] * Quaternion.Euler(0f, newAngle, 0f);

                // Apply the new rotation
                turretTop.rotation = Quaternion.Slerp(turretTop.rotation, newRotation, lookSpeed * Time.deltaTime);
            }
        }
        // Check if the player has pressed the 'X' key
        if (Input.GetKeyDown(KeyCode.X))
        {
            // Find the closest turret to the player
            Transform closestTurret = getNearestTurret();

            // Toggle the status of the closest turret
            if (closestTurret != null)
            {
                if (turretStatuses[closestTurret])
                {
                    DisableTurret(closestTurret);
                }
                else
                {
                    EnableTurret(closestTurret);
                }
            }
        }
    }


    private Transform getNearestTurret()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestTurret = null;
        foreach (Transform turret in turrets)
        {
            float distance = Vector3.Distance(turret.position, player.position);
            if (distance < closestDistance && distance < 3f)
            {
                closestTurret = turret;
                closestDistance = distance;
            }
        }

        return closestTurret;
        
    }

    // Disable a turret by setting its status to false and instantiate explosion effect
    private void DisableTurret(Transform turret)
    {
        turretStatuses[turret] = false;
        Debug.Log("Disabled turret " + turret.name);

        // Instantiate explosion effect on the top of the turret
        GameObject explosionObject = Instantiate(explosionPrefab, turret.Find("Top").position, Quaternion.identity);
        explosionObjects[turret] = explosionObject;

        // Destroy the explosion effect object after the particle system has finished playing
        ParticleSystem explosionParticleSystem = explosionObject.GetComponent<ParticleSystem>();
        Destroy(explosionObject, explosionParticleSystem.main.duration);
    }

    // Enable a turret by setting its status to true and delete explosion effect object
    private void EnableTurret(Transform turret)
    {
        turretStatuses[turret] = true;
        Debug.Log("Enabled turret " + turret.name);

        // Destroy the explosion effect object if it exists
        if (explosionObjects.ContainsKey(turret))
        {
            Destroy(explosionObjects[turret]);
            explosionObjects.Remove(turret);
        }
    }
}
