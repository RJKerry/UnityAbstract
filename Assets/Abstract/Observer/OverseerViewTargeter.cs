using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverseerViewTargeter : MonoBehaviour
{
    public GameObject viewTarget;
    public List<GameObject> POIs;

    public GameObject currentTarget;

    public float eTime, tTime = 1f;

    public bool moving = false;

    private void Update()
    {
        if (moving) { //while updating if moving
            if (eTime / tTime < 1) //and elapsed time != target time
            { 
                MoveViewTargetToPoint(currentTarget.transform.position, eTime, tTime); //update the position of the object
            }
            if (eTime >= tTime)
            {
                moving = false;
                InitNewTarget();
            }
        }
    }

    /// <summary>
    /// A method call to start the behaviours
    /// </summary>
    public void Activate()
    {
        InitNewTarget();
    }

    /// <summary>
    /// Resets the lerp to be ready for a new transition
    /// </summary>
    void InitNewTarget()
    { 
        eTime = 0;
        currentTarget = SelectNewTarget(currentTarget);
        moving = true;
    }

    /// <summary>
    /// SelectNewTarget will randomly generate an index from targets,
    /// if it is identical to the previous target, it will call again until a different
    /// one is selected
    /// </summary>
    /// <param name="oldTarget">The previous view target</param>
    /// <returns></returns>
    public GameObject SelectNewTarget(GameObject oldTarget)
    {
        GameObject newTarget = POIs[Random.Range(0, POIs.Count)];
        if (newTarget == oldTarget)
            return SelectNewTarget(oldTarget);
        return newTarget;
    }

    /// <summary>
    /// Lerper for object
    /// </summary>
    /// <param name="point">Point in 3d space to move to</param>
    /// <param name="elapsedTime">Time passed since trigger</param>
    /// <param name="targetTime">Target time to transition within</param>
    void MoveViewTargetToPoint(Vector3 point, float elapsedTime, float targetTime)
    { 
        viewTarget.transform.position = Vector3.Lerp(viewTarget.transform.position, point, elapsedTime/targetTime);
        eTime += Time.deltaTime;
    }
}