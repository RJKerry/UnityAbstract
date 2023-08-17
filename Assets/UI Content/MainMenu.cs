using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button startGame;


    void loadLevel()
    {
        Application.LoadLevel("Level_1");
    }

    void Start()
    {
        startGame.onClick.AddListener(loadLevel);
    }
    
}
