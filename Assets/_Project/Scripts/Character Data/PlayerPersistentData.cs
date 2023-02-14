using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPersistentData", menuName = "New Player Persistent Data")]
public class PlayerPersistentData : ScriptableObject
{
    public PlayerPersistentCharacter[] characters;

#if UNITY_EDITOR
    private void OnValidate()
    {
        ResetCharacters();
    }
#endif

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