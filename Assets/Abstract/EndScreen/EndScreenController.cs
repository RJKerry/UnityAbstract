using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenController : LevelTransitioner
{
    public int TimeToWait = 5;

    private void Awake()
    {
        StartCoroutine(GameEndSequence());
    }

    protected IEnumerator GameEndSequence() 
    {
        yield return new WaitForSecondsRealtime(TimeToWait);
        StartCoroutine(LoadNewScene(NextSceneIndex, loadMode));
    }

}
