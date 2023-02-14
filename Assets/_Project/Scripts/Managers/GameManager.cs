using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static CharacterSet currentEnemySet;

    [Header("Teams")]
    public Character[] playerTeam;
    public Character[] enemyTeam;

    [Header("Components")]
    public Transform[] playerTeamSpawns;
    public Transform[] enemyTeamSpawns;

    [Header("Data")]
    public PlayerPersistentData playerPersistentData;
    public CharacterSet defaultEnemySet;

    private List<Character> _allCharacters = new();

    private void Awake()
    {
        if(instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        if (currentEnemySet == null)
            CreateCharacters(playerPersistentData, defaultEnemySet);
        else
            CreateCharacters(playerPersistentData, currentEnemySet);
        TurnManager.instance.Begin();
    }

    public void OnCharacterKilled(Character character)
    {
        _allCharacters.Remove(character);

        int playersRemaining = 0;
        int enemiesRemaining = 0;

        for (int i = 0; i < _allCharacters.Count; i++)
        {
            if (_allCharacters[i].team == Character.Team.Player)
                playersRemaining++;
            else
                enemiesRemaining++;
        }

        // Player wins
        if(enemiesRemaining <= 0)
        {
            PlayerTeamWins();
        }
        // Enemy wins
        else if(playersRemaining <= 0)
        {
            EnemyTeamWins();
        }
    }

    private void CreateCharacters(PlayerPersistentData playerData, CharacterSet enemyTeamSet)
    {
        playerTeam = new Character[playerData.characters.Length];
        enemyTeam = new Character[enemyTeamSet.characters.Length];

        int playerSpawnIndex = 0;
        for (int i = 0; i < playerData.characters.Length; i++)
        {
            if (!playerData.characters[i].isDead)
            {
                Character player = CreateCharacter(playerData.characters[i].characterPrefab, playerTeamSpawns[playerSpawnIndex]);
                player.currentHealth = playerData.characters[i].health;
                player.currentMana = playerData.characters[i].mana;
                playerTeam[i] = player;
                playerSpawnIndex++;
            }
            else
            {
                playerTeam[i] = null;
            }
        }

        for (int i = 0; i < enemyTeamSet.characters.Length; i++)
        {
            Character enemy = CreateCharacter(enemyTeamSet.characters[i], enemyTeamSpawns[i]);
            enemyTeam[i] = enemy;
        }

        _allCharacters.AddRange(playerTeam);
        _allCharacters.AddRange(enemyTeam);
    }

    private Character CreateCharacter(GameObject characterPrefab, Transform spawnPosition)
    {
        GameObject temp = Instantiate(characterPrefab, spawnPosition.position, spawnPosition.rotation);
        return temp.GetComponent<Character>();
    }

    private void PlayerTeamWins()
    {
        UpdatePlayerPersistentData();
        MapManager.currentData.hasBeenCleared = true;

        // TODO: Add a victory panel that gives you rewards instead of loading Map Scene
        Invoke(nameof(LoadMapScene), 0.5f);
    }

    private void EnemyTeamWins()
    {
        playerPersistentData.ResetCharacters();
        MapManager.currentData.hasBeenCleared = false;
        // TODO: Add a Game Over panel or Scene instead of loading Map Scene
        Invoke(nameof(LoadMapScene), 0.5f);
    }

    private void UpdatePlayerPersistentData()
    {
        for (int i = 0; i < playerTeam.Length; i++)
        {
            if (playerTeam[i] != null)
            {
                playerPersistentData.characters[i].health = playerTeam[i].currentHealth;
                playerPersistentData.characters[i].mana = playerTeam[i].currentMana;
            }
            else
            {
                playerPersistentData.characters[i].isDead = true;
            }
        }
    }

    private void LoadMapScene()
    {
        SceneManager.LoadScene("Map");
    }
}