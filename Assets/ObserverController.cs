using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverController : MonoBehaviour, ITerminalListener
{
    public OverseerViewTargeter ViewTargeter;
    public CinemachineVirtualCamera OverseerView;

    public int posessionTime = 5;

    public int _IDGroup = 0;
    public int IDGroup { get => _IDGroup; set => _IDGroup = value; }

    public void OnActivated()
    {
        StartCoroutine(PossessionSequence());
    }

    private IEnumerator PossessionSequence()
    {
        OverseerView.Priority = 100;
        yield return new WaitForSecondsRealtime(posessionTime);
        OverseerView.Priority = 0;
    }
    private void Awake()
    {
        ViewTargeter = GetComponentInChildren<OverseerViewTargeter>();
        OverseerView = GetComponentInChildren<CinemachineVirtualCamera>();
    }
}