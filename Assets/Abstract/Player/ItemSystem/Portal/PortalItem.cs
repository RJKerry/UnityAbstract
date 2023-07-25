
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/ItemBase/PortalItem")]
public class PortalItem : ItemData
{
    //private Camera PortalCam;

    private Camera PlayerCam;

    //public RenderTexture portalTex;
    //public GameObject portal;

    //private bool portalActive = false;

    //private float tick;

    // Update is called once per frame

    private Portal CurrentPortal;

/*    private void Awake()
    {
        //PortalCam = portal.GetComponent<Camera>();
    }*/

/*    private void update()
    {
        if (portalActive)
        {
            // Calculate difference between player camera's rotation and portal's rotation
            Quaternion camRotDiff = Quaternion.Inverse(PortalCam.transform.rotation) * PlayerCam.transform.rotation;

            // Apply difference to portal camera's rotation
            PortalCam.transform.rotation = PortalCam.transform.rotation * camRotDiff;

            tick += Time.deltaTime;


            if (tick >= 10)
            {
                portalActive = false;
                portal.transform.position = new Vector3(0, 0, 0);
            }
        }
    }*/

    public override void ItemUsed()
    {
        Debug.Log("Portal item has been used");

        RaycastHit hit;
        if (Physics.Raycast(PlayerCam.transform.position, PlayerCam.transform.forward, out hit))
        {
            if (hit.transform.CompareTag("Wall"))
            {
                Instantiate(portal, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up) * Quaternion.Euler(90, 0, 0));

                // Calculate position of camera on the other side of the wall
                Vector3 cameraPos = hit.point + hit.normal * 0.01f;

                // Set camera's render texture to the portal texture
                PortalCam.targetTexture = portalTex;

                // Set camera's position
                PortalCam.transform.position = cameraPos;
                portalActive = true;
                //tick = Time.deltaTime;
            }
        }
    }
}