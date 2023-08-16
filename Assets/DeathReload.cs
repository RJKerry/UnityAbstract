using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathReload : MonoBehaviour
{
    private int previousLevelBuildIndex;
    private int DeathScreenDuration = 10;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        previousLevelBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.sceneLoaded += BeginReload;
    }

    /// <summary>
    /// Match the function signature of sceneLoaded event 
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadMode"></param>
    public void BeginReload(Scene scene, LoadSceneMode loadMode) 
    {
        StartCoroutine(Reload());
    }

    /// <summary>
    /// Reload previous scene
    /// </summary>
    /// <returns></returns>
    private IEnumerator Reload()
    {
        yield return new WaitForSecondsRealtime(DeathScreenDuration);
        AsyncOperation reload = SceneManager.LoadSceneAsync(previousLevelBuildIndex);
        while (!reload.isDone)
            yield return null;

        Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= BeginReload;
    }
}
