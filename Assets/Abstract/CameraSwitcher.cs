using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    List<CinemachineVirtualCameraBase> Cameras;
    int activePriority = 10, inactivePriority = 0;

    void GatherCameraReferences()
    {
        Cameras = new List<CinemachineVirtualCameraBase>(FindObjectsOfType<CinemachineVirtualCameraBase>());
    }
    void SwitchToCamera(CinemachineVirtualCameraBase camera)
    {
        foreach (CinemachineVirtualCamera cam in Cameras)
        {
            if (cam != camera)
                cam.Priority = inactivePriority;
        }
        camera.Priority = activePriority;
    }
}