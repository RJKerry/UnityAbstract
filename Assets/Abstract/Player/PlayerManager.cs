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
    private GameObject DeathReloadData;
    private HealthController healthController;
    public override void Init()
    {
        base.Init();
        DeathScene = "Death";
        DeathReloadData = Resources.Load<GameObject>("DeathScreenLoadPackage");
        playerCapsule = GetComponent<CharacterController>();
        healthController = this.transform.Find("HUD-New").GetComponent<HealthController>();
    }

    public Vector3 PlayerPosition(bool AddHalfHeightToUp)
    { 
        return AddHalfHeightToUp ? transform.position + Vector3.up * playerCapsule.height / 2 : transform.position; 
    }

    public override void DamageEffect(float damage)
    {
        healthController.changeHealth(Health);
    }

    public override void DeathEffect()
    {
        //DontDestroyOnLoad(this.gameObject);
        lastSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadDeathScene());
    }

    public IEnumerator LoadDeathScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(DeathScene);
        DeathReload reloadObj = Instantiate(DeathReloadData).GetComponent<DeathReload>();
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}