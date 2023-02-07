using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    public event UnityAction OnNewTurn;

    [Header("Components")]
    public GameObject endTurnButton;

    private List<Character> _turnOrder = new();
    private Character _currentTurnCharacter;
    private int _currentTurnOrderIndex;

    public Character CurrentTurnCharacter { get => _currentTurnCharacter; private set => _currentTurnCharacter = value; }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    // TODO: Make it the enemy's turn if player is ambushed or add speed variables and check which is higher.
    public void Begin()
    {
        GenerateTurnOrder(Character.Team.Player);
        NewTurn(_turnOrder[0]);
    }

    public void EndTurn()
    {
        _currentTurnOrderIndex++;

        if (_currentTurnOrderIndex == _turnOrder.Count)
            _currentTurnOrderIndex = 0;

        // While character is dead, we skip their turn.
        while (_turnOrder[_currentTurnOrderIndex] == null)
        {
            _currentTurnOrderIndex++;

            if (_currentTurnOrderIndex == _turnOrder.Count)
                _currentTurnOrderIndex = 0;
        }

        NewTurn(_turnOrder[_currentTurnOrderIndex]);
    }

    /// <summary>
    /// Called whenever you cannot cast a combat action due to low mana.
    /// </summary>
    public void ResetTurn()
    {
        NewTurn(CurrentTurnCharacter);
    }

    private void GenerateTurnOrder(Character.Team startingTeam)
    {
        if(startingTeam == Character.Team.Player)
        {
            _turnOrder.AddRange(GameManager.instance.playerTeam);
            _turnOrder.AddRange(GameManager.instance.enemyTeam);
        }
        else if(startingTeam == Character.Team.Enemy)
        {
            _turnOrder.AddRange(GameManager.instance.enemyTeam);
            _turnOrder.AddRange(GameManager.instance.playerTeam);
        }
    }

    private void NewTurn(Character character)
    {
        CurrentTurnCharacter = character;
        OnNewTurn?.Invoke();

        endTurnButton.SetActive(character.team == Character.Team.Player);
    }
}