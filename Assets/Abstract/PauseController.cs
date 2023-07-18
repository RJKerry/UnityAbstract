using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class PauseController : MonoBehaviour
{
    bool paused;
    string menuSceneName = "MainMenu";
    Canvas canvas;
    GameObject pauseMenu;
    //Button resumeButton, quitButton;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        //Debug.Log(canvas);
        pauseMenu = Instantiate(Resources.Load<GameObject>("MenuAssets/PauseMenu"));
        pauseMenu.transform.SetParent(canvas.transform, false);
        pauseMenu.SetActive(false);

        //For some reason button is not a component??

        //This is not the best way of doing it but it works for now 


        //THIS DOESNT WORK
        /*UIDocument uiDoc = FindObjectOfType<UIDocument>();
       resumeButton = uiDoc.rootVisualElement.Query<Button>("ResumeButton");
        quitButton = uiDoc.rootVisualElement.Query<Button>("ExitButton");*/ 

        //THIS DOESNT WORK EITHER
        //resumeButton = GameObject.FindGameObjectWithTag("ResumeButton").GetComponent<Button>();
        //quitButton = GameObject.FindGameObjectWithTag("ExitButton").GetComponent<Button>();

        //These need to happen, however null references keep happ[ening
        /*resumeButton.clicked += TogglePause;
        quitButton.clicked += Quit;*/

        //Debug.Log(pauseMenu);
    }

    public void TogglePause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0f : 1f;
        pauseMenu.SetActive(paused);
    }

    public void Quit()
    {
        SceneManager.LoadScene(menuSceneName);
    }

}