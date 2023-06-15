using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For any given stage of progression we will
/// want to ensure required objectives are
/// completed before triggering forms of progression
/// 
/// This class provides a means to validate and react accordingly
/// 
/// If you were to open a door, and need two things done,
/// 2 objectives (existing in the world) will await completion,
/// and when theyre all popped the progression event - in ObjectiveCollectionComplete
/// will occur
/// </summary>
public class ObjectiveValidator : MonoBehaviour
{
    public List<Objective> Objectives; //centralised objectives collection
    private int IDGroup = 0; //ID to look for when setting up

    private void Awake()
    {
        //THIS WANTS CHANGING, RIGHT NOW IT GETS ALL OBJECTS, WE COULD DO WITH THE MEANS TO IDENTIFY
        //A SET OF PUZZLES, PERHAPS BY VECTOR DISTANCE, OR SOME FORM OF CLASS TYPE BASED RESTRICTION, OR ID SYSTEM

        List<Objective> _objectives = new List<Objective>(FindObjectsOfType<Objective>());
        foreach (Objective _obj in _objectives)
            if (_obj.ID == IDGroup)
            { 
                Objectives.Add(_obj);
                Debug.Log("Added " + _obj + " to " + this.name + "'s validation collection"); 
            }
    }

    /// <summary>
    /// Checks if any objectives remain, and if they do + are completed remove them
    /// then if none are left, this specific instance of a puzzle group is complete
    /// </summary>
    public void Validate()
    {
        if (Objectives.Count <= 0)
        {   ObjectiveCollectionComplete(); 
            return; }

        foreach (Objective obj in Objectives) 
        {
            if (!obj.ObjectiveComplete) //If objective not completed
                return; //break
            else
                Objectives.Remove(obj); //else pop it
        }
    }

    /// <summary>
    /// Override this in the child class to add functionality for whatever you want
    /// i.e. door opening, animation cutscene playing
    /// </summary>
    public virtual void ObjectiveCollectionComplete() 
    {
        Debug.Log("This set of objectives is done congrats");
    }
}