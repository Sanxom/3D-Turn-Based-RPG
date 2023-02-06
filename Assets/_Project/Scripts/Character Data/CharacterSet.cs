using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Set", menuName = "New Character Set")]
public class CharacterSet : ScriptableObject
{
    public GameObject[] characters;
}