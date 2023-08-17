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

    public GameObject UIComponents;
    public Button rightButton;
    public Button leftButton;

    private Dictionary<ITerminalListener, Button> buttonDictionary;

    public void Awake()
    {
        UIComponents.SetActive(false);
        BackgroundImage.sprite = SpriteSequence[0];
        rightButton.onClick.AddListener(() => cycleButtons("right"));
        leftButton.onClick.AddListener(() => cycleButtons("left"));
    }


    public void Activated() 
    {
        StartCoroutine (ScreenTransition(false));
    }

    public void Deactivate() 
    {
        UIComponents.SetActive(false);
        StartCoroutine (ScreenTransition(true));
    }

    public void receiveButtonDictionary(Dictionary<ITerminalListener, Button> IncomingButtonDictionary)
    {
        buttonDictionary = IncomingButtonDictionary;
    }

    /// <summary>
    /// Direction will receive a "right" or "left" input, which will cycle the buttons in that direction
    /// The buttons are all set as parents of the UIComponents object, so they can be cycled through with setActive.
    /// There may be more than two buttons, so the buttons will be cycled through in a loop
    /// This function will be triggered by the rightButton and leftButton objects
    /// </summary>
    /// <param name="direction"></param>

    public void cycleButtons(string direction)
    {
        if (direction == "right" || direction == "left")
        {
            int currentIndex = -1;
            ITerminalListener currentKey = null;

            // Create a list to store keys
            List<ITerminalListener> keysList = new List<ITerminalListener>(buttonDictionary.Keys);

            foreach (var kvp in buttonDictionary)
            {
                if (kvp.Value.gameObject.activeSelf)
                {
                    currentKey = kvp.Key;
                    currentIndex = keysList.IndexOf(currentKey);
                    kvp.Value.gameObject.SetActive(false);
                    break;
                }
            }

            if (currentKey != null && currentIndex != -1)
            {
                int nextIndex = (currentIndex + 1) % keysList.Count;
                int prevIndex = (currentIndex - 1 + keysList.Count) % keysList.Count;

                if (direction == "right")
                {
                    buttonDictionary[keysList[nextIndex]].gameObject.SetActive(true);
                }
                else if (direction == "left")
                {
                    buttonDictionary[keysList[prevIndex]].gameObject.SetActive(true);
                }
            }
        }
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
        OnComplete(Reverse);
    }

    /// <summary>
    /// This could be used to trigger events once the sprite sequence has completed
    /// or anything else you could want
    /// </summary>
    public void OnComplete(bool reversed)
    {
        if (!reversed && buttonDictionary.Count > 0)
        {
            UIComponents.SetActive(true);

            foreach (var kvp in buttonDictionary)
            {
                // Set the parent of each button to the terminal canvas
                kvp.Value.transform.SetParent(transform);
                kvp.Value.gameObject.SetActive(false);

                // Set rotation and position of each button to 0
                kvp.Value.transform.localRotation = Quaternion.identity;
                kvp.Value.transform.localPosition = Vector3.zero;

            }

            foreach (var kvp in buttonDictionary)
            {
                // Activate UIComponents for the first button and break the loop
                kvp.Value.gameObject.SetActive(true);
                break;
            }
        }

        Debug.Log("Image Sequence Complete");
    }


}