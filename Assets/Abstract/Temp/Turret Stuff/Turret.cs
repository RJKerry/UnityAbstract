using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, ICanBeDisabled
{
    public GameObject player;
    public float lookSpeed;
    public float activationDistance;

    public bool TurretActive = true;
    public Quaternion baseRot;

    public GameObject TurretHead;

    public GameObject ExplosionEffect;

    // Start is called before the first frame update
    void Start()
    {
        TurretHead = GameObject.Find("Top");
        player = GameObject.Find("Player");
        baseRot = TurretHead.transform.rotation;
    }

    private void Update()
    {
        if (!TurretActive)
            return;

        if (!LocatePlayer())
            Pan();
        // otherwise, rotate the top of the turret freely
    }

    private void Pan()
    {
        Transform turretTop = TurretHead.transform;

        // Calculate the new rotation angle
        float newAngle = Mathf.PingPong(Time.time * 30f * lookSpeed, 180f) - 90f;
        Quaternion newRotation = baseRot * Quaternion.Euler(0f, newAngle, 0f);

        // Apply the new rotation
        turretTop.rotation = Quaternion.Slerp(turretTop.rotation, newRotation, lookSpeed * Time.deltaTime);
    }

    private bool LocatePlayer()
    {
        Transform turretTransform = GetTransform();
        Transform playerTransform = player.transform;

        Vector3 toPlayer = player.transform.position - turretTransform.position;
        float dot = Vector3.Dot(turretTransform.forward, toPlayer.normalized);
        float distance = toPlayer.magnitude;

        if (dot > 0.0f && dot > Mathf.Cos(Mathf.PI / 3.0f) && distance < activationDistance && distance > 1f)
        {
            Vector3 targetDirection = playerTransform.position - turretTransform.position;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            TurretHead.transform.rotation = Quaternion.Slerp(TurretHead.transform.rotation, targetRotation, lookSpeed * Time.deltaTime);

            return true;
        }
        return false;
    }

    public Transform GetTransform()
    {
        return this.gameObject.transform;
    }

    public void Enable()
    {
        TurretActive = true;
    }
    public void Disable()
    {
        TurretActive = false;
    }

    public void OnDisableInteract()
    {
        Disable();
    }
}
/*public struct TurretVars 
{
    public bool TurretActive;

    public TurretVars(bool ActiveByDefault)
    {
        this.TurretActive = ActiveByDefault;
    }
}*/