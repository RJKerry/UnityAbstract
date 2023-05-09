using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    string SceneToLoad = "SampleScene";
    void Play()
    { SceneManager.LoadScene(SceneToLoad); }
    void Exit()
    { Application.Quit(); }
}
