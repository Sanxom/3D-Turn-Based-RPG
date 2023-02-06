using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public int durationOfTurns;

    [Header("Prefabs")]
    public GameObject activePrefabGO;
    public GameObject tickPrefabGO;
}