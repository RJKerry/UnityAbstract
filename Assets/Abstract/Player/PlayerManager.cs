using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : HealthHandler
{
    CharacterController playerCapsule;
    private string lastSceneName;
    private string DeathScene;
    public override void Init()
    {
        DeathScene = "Death";
        base.Init();
        playerCapsule = GetComponent<CharacterController>();
    }

    public Vector3 PlayerPosition(bool AddHalfHeightToUp)
    { 
        return AddHalfHeightToUp ? transform.position + Vector3.up * playerCapsule.height / 2 : transform.position; 
    }

    public override void DamageEffect(float damage)
    {

    }

    public override void DeathEffect()
    {
        DontDestroyOnLoad(this.gameObject);
        lastSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(loadDeathScene());
    }

    public async IEnumerator loadDeathScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(DeathScene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }

}