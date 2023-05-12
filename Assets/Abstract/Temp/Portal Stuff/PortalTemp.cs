using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTemp : MonoBehaviour
{
    public Camera PortalCam;
    public Camera PlayerCam;
    public RenderTexture portalTex;
    public GameObject portal;
    private bool portalActive = false;
    private float tick;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
    
            RaycastHit hit;
            if (Physics.Raycast(PlayerCam.transform.position, PlayerCam.transform.forward, out hit))
            {
                if (hit.transform.CompareTag("Wall"))
                {
                    // Place portal on the wall
                    portal.transform.position = hit.point;
                    portal.transform.rotation = Quaternion.LookRotation(hit.normal, Vector3.up) * Quaternion.Euler(90, 0, 0);

                    // Calculate position of camera on the other side of the wall
                    Vector3 cameraPos = hit.point + hit.normal * 0.01f;

                    // Set camera's render texture to the portal texture
                    PortalCam.targetTexture = portalTex;

                    // Set camera's position
                    PortalCam.transform.position = cameraPos;
                    portalActive = true;
                    tick = Time.deltaTime;
                }
            }
        }

        if (portalActive)
        {
            // Calculate difference between player camera's rotation and portal's rotation
            Quaternion camRotDiff = Quaternion.Inverse(transform.rotation) * PlayerCam.transform.rotation;

            // Apply difference to portal camera's rotation
            PortalCam.transform.rotation = transform.rotation * camRotDiff;

            tick += Time.deltaTime;


            if (tick >= 10) 
            {
                portalActive = false;
                portal.transform.position = new Vector3(0, 0, 0);
            }
        }


    }
}
