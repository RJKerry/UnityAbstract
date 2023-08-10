using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Currently used to display the terminal screen turning on and off 
/// </summary>
public class TerminalCanvasController : MonoBehaviour
{
    public Image BackgroundImage;

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

    /// <summary>
    /// Iterates through different sprite frames either forwards or backwards, based on framespeed
    /// </summary>
    /// <param name="Reverse">Should a specific instance of this call cause the soritesheet to play in reverse</param>
    public IEnumerator ScreenTransition(bool Reverse)
    {
        if (transitioning)
            yield return null;

        int currentFrameIndex = 0;

        List<Sprite> sequence = new(SpriteSequence);
        if (Reverse)
            sequence.Reverse();

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

    /// <summary>
    /// This could be used to trigger events once the sprite sequence has completed
    /// or anything else you could want
    /// </summary>
    public void OnComplete()
    {
        Debug.Log("Image Sequence Complete");
    }
}