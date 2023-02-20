using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map Data", menuName = "New Map Data")]
public class MapData : ScriptableObject
{
    public int currentEncounter;
    public bool hasBeenCleared;

    public string currentEncounterKey = "currentEncounter";
    public string hasBeenClearedKey = "hasBeenCleared";

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(currentEncounterKey))
            currentEncounter = PlayerPrefs.GetInt(currentEncounterKey);
        else
            currentEncounter = 0;

        if (PlayerPrefs.HasKey(hasBeenClearedKey))
        {
            hasBeenCleared = Convert.ToBoolean(PlayerPrefs.GetString(hasBeenClearedKey));
        }
        else
            hasBeenCleared = false;
    }

    public void IncrementEncounter()
    {
        currentEncounter++;
    }

    // FOR TESTING
    //private void OnDisable()
    //{
    //    PlayerPrefs.DeleteAll();
    //}
}