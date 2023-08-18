using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{

    public Button resetButton;

    void Awake()
    {
        resetButton = this.transform.Find("Reset").GetComponent<Button>();
        resetButton.onClick.AddListener(Reset);
    }

    void Reset()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
