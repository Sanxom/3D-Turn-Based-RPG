using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatActionButton : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    private CombatAction combatAction;
    private CombatActionsUI ui;

    private void Awake()
    {
        ui = FindObjectOfType<CombatActionsUI>();
    }

    public void SetCombatAction(CombatAction ca)
    {
        combatAction = ca;
        nameText.text = ca.displayName;
    }

    public void OnClick()
    {
        PlayerCombatManager.instance.SetCurrentCombatAction(combatAction);
    }

    public void OnHoverEnter()
    {
        ui.SetCombatActionDescription(combatAction);
    }

    public void OnHoverExit()
    {
        ui.DisableCombatActionDescription();
    }
}