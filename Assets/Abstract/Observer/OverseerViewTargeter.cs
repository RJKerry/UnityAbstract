using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverseerViewTargeter : MonoBehaviour
{
    public GameObject viewTarget;
    public List<GameObject> POIs;

    public int currentTargetIndex;

    public float eTime;
    public float tTime = 1f;

    public bool Active = true;

    //public SelectorTypes InitialState; //static
    public SelectorTypes selectorType; //dynamic


    private void Awake()
    {
        InitNewTarget();

        TargetSelectorType(selectorType);
    }

    private void Update()
    {
        if (Active) { //while updating if moving
            if (eTime >= tTime)
            {
                InitNewTarget();
                return;
            }
            if (eTime / tTime < 1) //and elapsed time != target time
            { 
                MoveViewTargetToPoint(POIs[currentTargetIndex].transform.position, eTime, tTime); //update the position of the object
            }
        }
    }

    public int TargetSelectorType(SelectorTypes newState)
    {
        switch (newState)
        {
            case SelectorTypes.LinearSwitch:
                return LinearSwitch(currentTargetIndex);
            case SelectorTypes.RandomSwitch:
                return RandomTargetSelect(currentTargetIndex);
            default:
                break;
        }
        return currentTargetIndex;
    }

    public int LinearSwitch(int oldTargetIndex)
    {
        if(POIs.Count < 1)
            return oldTargetIndex;

        return (oldTargetIndex + 1) > POIs.Count-1 ? 0 : oldTargetIndex + 1; //next element, or the first element
    }

    /// <summary>
    /// Resets the lerp to be ready for a new transition
    /// </summary>
    void InitNewTarget()
    { 
        eTime = 0;
        currentTargetIndex = TargetSelectorType(selectorType);
        //Active = true;
    }

    /// <summary>
    /// SelectNewTarget will randomly generate an index from targets,
    /// if it is identical to the previous target, it will call again until a different
    /// one is selected
    /// </summary>
    /// <param name="oldTargetIndex">The previous view target</param>
    /// <returns></returns>
    public int RandomTargetSelect(int oldTargetIndex)
    {
        if (POIs.Count < 1)
            return oldTargetIndex;

        int newTarget = Random.Range(0, POIs.Count);
        if (newTarget == oldTargetIndex)
            return RandomTargetSelect(oldTargetIndex);
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

public enum SelectorTypes
{ 
    LinearSwitch,
    RandomSwitch,
};