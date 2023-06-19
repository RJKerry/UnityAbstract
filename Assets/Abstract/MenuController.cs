using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    string SceneToLoad = "Turret Test";
    public void Play()
    { SceneManager.LoadScene(SceneToLoad); }
    public void Exit()
    { Application.Quit(); }
}