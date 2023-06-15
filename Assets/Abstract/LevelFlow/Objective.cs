using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public int ID = 0; //ID to represent validator group (theres probably a more uniform way of doing this)
    public bool ObjectiveComplete = false;
    public virtual void ReturnSomething() { }

    public virtual void PuzzleComplete() { }
}
