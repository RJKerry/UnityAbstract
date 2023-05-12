using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public Sprite fourLives;
    public Sprite threeLives;
    public Sprite twoLives;
    public Sprite oneLife;
    public Image lives;

    // Start is called before the first frame update
    void Start()
    {
        changeLives(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeLives(int x)
    {
        switch(x)
        {
            case 1:
                lives.sprite = oneLife;
                break;
            case 2:
                lives.sprite = twoLives;
                break;
            case 3:
                lives.sprite = threeLives;
                break;
            case 4:
                lives.sprite = fourLives;
                break;
            default:
                lives.sprite = fourLives;
                break;
        }
    }

}
