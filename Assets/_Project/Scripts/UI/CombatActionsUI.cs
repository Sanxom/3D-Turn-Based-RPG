using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatActionsUI : MonoBehaviour
{
    public GameObject combatActionsPanel;
    public GameObject descriptionPanel;
    public TextMeshProUGUI descriptionText;
    public CombatActionButton[] buttons;

    private void OnEnable()
    {
        TurnManager.instance.OnNewTurn += OnNewTurn;
    }

    private void OnDisable()
    {
        TurnManager.instance.OnNewTurn -= OnNewTurn;
    }

    public void DisplayCombatActions(Character character)
    {
        combatActionsPanel.SetActive(true);

        for (int i = 0; i < buttons.Length; i++)
        {
            if(i < character.combatActions.Length)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].SetCombatAction(character.combatActions[i]);
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    public void DisableCombatActions()
    {
        combatActionsPanel.SetActive(false);
        DisableCombatActionDescription();
    }

    public void SetCombatActionDescription(CombatAction combatAction)
    {
        descriptionPanel.SetActive(true);
        descriptionText.text = combatAction.description;
    }

    public void DisableCombatActionDescription()
    {
        descriptionPanel.SetActive(false);
    }

    private void OnNewTurn()
    {
        // Display Player's actions if it's their turn.
        if(TurnManager.instance.CurrentTurnCharacter.team == Character.Team.Player)
        {
            DisplayCombatActions(TurnManager.instance.CurrentTurnCharacter);
        }
        // Disable Player's actions if it's not their turn.
        else
        {
            DisableCombatActions();
        }
    }
}