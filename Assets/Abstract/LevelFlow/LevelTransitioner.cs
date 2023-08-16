using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitioner : MonoBehaviour
{
    public int NextSceneIndex = 0; //Default is zero - this will load the main menu unless you override it
    public LoadSceneMode loadMode = LoadSceneMode.Single; //Single by default, fresh isolated Scene, Additive for compound scenes

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerManager>() == null)
            return;

        StartCoroutine(LoadNewScene(NextSceneIndex, loadMode));
    }

    protected IEnumerator LoadNewScene(int newSceneIndex, LoadSceneMode loadMode)
    { 
        //DontDestroyOnLoad(this.transform.parent);
        AsyncOperation levelLoad = SceneManager.LoadSceneAsync(NextSceneIndex, loadMode);
        while (!levelLoad.isDone)
        {
            yield return null;
        }
    }
}
