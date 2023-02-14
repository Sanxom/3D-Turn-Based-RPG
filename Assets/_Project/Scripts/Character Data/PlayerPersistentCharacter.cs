using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPersistentCharacter
{
    public GameObject characterPrefab;
    public int health;
    public int mana;
    public bool isDead;
}