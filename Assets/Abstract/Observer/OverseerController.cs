using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverseerController : MonoBehaviour, ITerminalListener
{
    private OverseerViewTargeter ViewTargeter;
    private CinemachineVirtualCamera OverseerView;

    public int posessionTime = 5; //duration of time overseer can be posessed for

    //Terminal ID group
    private int _IDGroup = 0;
    public int IDGroup { get => _IDGroup; set => _IDGroup = value; } //init interface ref

    //icon to show on terminal
    public Sprite _TerminalButtonIcon;
    public Sprite TerminalButtonIcon { get => _TerminalButtonIcon; set => _TerminalButtonIcon = value; }

    /// <summary>
    /// When activated by terminallistener message
    /// </summary>
    public void OnActivated()
    {
        StartCoroutine(PossessionSequence());
    }

    /// <summary>
    /// set overseers camera to primary for a duration, then revert it.
    /// </summary>
    /// <returns></returns>
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