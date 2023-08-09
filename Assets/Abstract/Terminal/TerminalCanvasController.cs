using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TerminalCanvasController : MonoBehaviour
{
    public UnityEngine.UI.Image BackgroundImage;

    public List<Sprite> SpriteSequence;

    public float FrameSpeed;

    public bool transitioning = false;

    public void Activated() 
    {
        StartCoroutine (ScreenTransition(false));
    }

    public void Deactivate() 
    {
        StartCoroutine (ScreenTransition(true));
    }

    public IEnumerator ScreenTransition(bool Reverse)
    {
        int currentFrameIndex = 0;

        List<Sprite> sequence = new(SpriteSequence);
        if(Reverse)
            sequence.Reverse();

        Debug.Log(SpriteSequence.Count);

        if (transitioning)
            yield return null;
        transitioning = true;

        float ETime = 0; 
        float TTime = 1;

        while (ETime < TTime)
        {
            int newFrameIndex = (int)(SpriteSequence.Count * (ETime / TTime)); //allows iteration over time, scaled

            if (newFrameIndex == currentFrameIndex) //No Change in frame this iteration
                yield return null;

            currentFrameIndex = newFrameIndex;
            BackgroundImage.sprite = sequence[currentFrameIndex];
            ETime += Time.deltaTime * FrameSpeed;
            yield return null;
        }
        transitioning = false;
        OnComplete();
    }

    public void OnComplete()
    {
        Debug.Log("Complete");
    }

    private void Awake()
    {
        //StartCoroutine(ScreenTransition(false));
    }
}
