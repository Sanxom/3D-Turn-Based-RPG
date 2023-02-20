using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPersistentData", menuName = "New Player Persistent Data")]
public class PlayerPersistentData : ScriptableObject
{
    public PlayerPersistentCharacter[] characters;

    public string savedHealth = "savedHealth";
    public string savedMana = "savedMana";
    public string savedIsDead = "savedIsDead";

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(savedHealth + "0") || PlayerPrefs.HasKey(savedHealth + "1") || PlayerPrefs.HasKey(savedHealth + "2"))
        {
            for (int i = 0; i < characters.Length; i++)
            {
                characters[i].health = PlayerPrefs.GetInt(savedHealth + $"{i}");
                characters[i].mana = PlayerPrefs.GetInt(savedMana + $"{i}");
                characters[i].isDead = Convert.ToBoolean(PlayerPrefs.GetString(savedIsDead + $"{i}"));
            }
        }
        else
            ResetCharacters();
    }

    public void ResetCharacters()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].health = characters[i].characterPrefab.GetComponent<Character>().maxHealth;
            characters[i].mana = characters[i].characterPrefab.GetComponent<Character>().maxMana;
            characters[i].isDead = false;
        }
    }
}