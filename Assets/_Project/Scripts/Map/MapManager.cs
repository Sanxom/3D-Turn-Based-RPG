using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    public List<Encounter> encounters = new();
    public MapParty party;
    public MapData data;

    [SerializeField] private TextMeshProUGUI _currentIndex;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < encounters.Count; i++)
        {
            if(i == data.currentEncounter)
                encounters[i].hasBeenCleared = data.hasBeenCleared;
        }

        UpdateEncounterStates();

        party.transform.position = encounters[data.currentEncounter].transform.position;

        _currentIndex.text = $"Current Index: {data.currentEncounter}";
    }

    public void MoveParty(Encounter encounter)
    {
        party.MoveToEncounter(encounter, OnPartyArriveAtEncounter);
    }

    private void UpdateEncounterStates()
    {
        for (int i = 0; i < encounters.Count; i++)
        {
            if (i < data.currentEncounter)
                encounters[i].SetState(Encounter.State.Visited);
            else if (i == data.currentEncounter && encounters[i].hasBeenCleared)
                encounters[i].SetState(Encounter.State.Visited);
            else if (i == data.currentEncounter && !encounters[i].hasBeenCleared)
                encounters[i].SetState(Encounter.State.CanVisit);
            else if (i == data.currentEncounter + 1 && encounters[i - 1].hasBeenCleared)
                encounters[i].SetState(Encounter.State.CanVisit);
            else if (i == data.currentEncounter + 1 && !encounters[i - 1].hasBeenCleared)
                encounters[i].SetState(Encounter.State.Locked);
            else if (i > data.currentEncounter + 1)
                encounters[i].SetState(Encounter.State.Locked);
        }
    }

    private void OnPartyArriveAtEncounter(Encounter encounter)
    {
        data.currentEncounter = encounters.IndexOf(encounter);
        PlayerPrefs.SetInt(data.currentEncounterKey, data.currentEncounter);
        data.hasBeenCleared = false;
        PlayerPrefs.SetString(data.hasBeenClearedKey, data.hasBeenCleared.ToString());
        GameManager.currentEnemySet = encounter.enemySet;
        SceneManager.LoadScene("Battle");
    }
}