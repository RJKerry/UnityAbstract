using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTargetManager : MonoBehaviour
{
    public List<GameObject> ViewTargets;

    private void Awake()
    {
        InitViewTargets();
    }

    public void InitViewTargets()
    {
        int childrem = transform.childCount;

    }
}
