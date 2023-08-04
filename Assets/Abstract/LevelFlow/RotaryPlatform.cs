using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotaryPlatform : MonoBehaviour, ITerminalListener
{
    public int _IDGroup;
    public int IDGroup { get => _IDGroup; set => _IDGroup = value; }

    public Sprite _TerminalButtonIcon;
    public Sprite TerminalButtonIcon { get => _TerminalButtonIcon; set => _TerminalButtonIcon = value; }

    public PlayerManager Player;

    Transform RotationOrigin;

    public float eTime, tTime = 1f; 
    public bool transitioning;

    public string
        PlatformTransition = "event:/Bridge/BridgeMoving";

    private void Awake()
    {
        RotationOrigin = transform.parent;
    }

    public void OnActivated()
    {
        StartCoroutine(RotateOverTime(
            transform.eulerAngles, //CurrentRot
            new Vector3(0, transform.eulerAngles.y + 90, 0), //Rotated 90, 0 will keep it stabilised on other axes
            tTime));
    }

    public IEnumerator RotateOverTime(Vector3 StartRot, Vector3 EndRot, float TargetTime)
    {
        if(transitioning)
            yield break;

        transitioning = true;

        eTime = 0;
        yield return new WaitForSecondsRealtime(0.4f); //works to offset audio issue, this will be fixed

        FMODUnity.RuntimeManager.PlayOneShot(PlatformTransition, RotationOrigin.transform.position);

        while (eTime < tTime)
        {
            RotationOrigin.rotation = Quaternion.Lerp(Quaternion.Euler(StartRot), Quaternion.Euler(EndRot), eTime / tTime);
            eTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(2f);
        
        transitioning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager manager = other.gameObject.GetComponent<PlayerManager>();
        if (manager != null)
        {
            Player = manager;
            Player.transform.parent = RotationOrigin;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player.gameObject)
        {
            Player.transform.parent = null;
            Player = null; 
        }
    }
}