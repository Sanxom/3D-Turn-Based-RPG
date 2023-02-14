using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map Data", menuName = "New Map Data")]
public class MapData : ScriptableObject
{
    public int currentEncounter;
    public bool hasBeenCleared;
}