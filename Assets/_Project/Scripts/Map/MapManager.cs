using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    public static MapData currentData;

    public List<Encounter> encounters = new();
    public MapParty party;
    public MapData data;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        if (currentData == null)
        {
            currentData = data;
        }

        for (int i = 0; i < encounters.Count; i++)
        {
            if(i == currentData.currentEncounter)
                encounters[i].hasBeenCleared = currentData.hasBeenCleared;
        }

        UpdateEncounterStates();

        party.transform.position = encounters[currentData.currentEncounter].transform.position;
    }

    public void MoveParty(Encounter encounter)
    {
        party.MoveToEncounter(encounter, OnPartyArriveAtEncounter);
    }

    private void UpdateEncounterStates()
    {
        for (int i = 0; i < encounters.Count; i++)
        {
            if (i < currentData.currentEncounter)
                encounters[i].SetState(Encounter.State.Visited);
            else if (i == currentData.currentEncounter && encounters[i].hasBeenCleared)
                encounters[i].SetState(Encounter.State.Visited);
            else if (i == currentData.currentEncounter && !encounters[i].hasBeenCleared)
                encounters[i].SetState(Encounter.State.CanVisit);
            else if (i == currentData.currentEncounter + 1 && encounters[i - 1].hasBeenCleared)
                encounters[i].SetState(Encounter.State.CanVisit);
            else if (i == currentData.currentEncounter + 1 && !encounters[i - 1].hasBeenCleared)
                encounters[i].SetState(Encounter.State.Locked);
            else if (i > currentData.currentEncounter + 1)
                encounters[i].SetState(Encounter.State.Locked);
        }
    }

    private void OnPartyArriveAtEncounter(Encounter encounter)
    {
        currentData.currentEncounter = encounters.IndexOf(encounter);
        currentData.hasBeenCleared = false;
        GameManager.currentEnemySet = encounter.enemySet;
        SceneManager.LoadScene("Battle");
    }
}