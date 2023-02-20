using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatManager : MonoBehaviour
{
    public static PlayerCombatManager instance;

    public LayerMask selectionLayerMask;
    public float selectionCheckRate = 0.02f;

    [Header("Components")]
    public CombatActionsUI combatActionsUI;

    private Character _currentSelectedCharacter;
    private CombatAction _currentSelectedCombatAction;
    private Camera _mainCamera;
    private float _lastSelectionCheckTime;
    private bool _isActive;

    // Selection Flags
    private bool _canSelectSelf;
    private bool _canSelectTeam;
    private bool _canSelectEnemy;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        TurnManager.instance.OnNewTurn += OnNewTurn;
    }

    private void OnDisable()
    {
        TurnManager.instance.OnNewTurn -= OnNewTurn;
    }

    private void Update()
    {
        // If we are able to cast a combat action and if we have an action selected
        if (!_isActive || _currentSelectedCombatAction == null)
            return;

        if(Time.time - _lastSelectionCheckTime > selectionCheckRate)
        {
            _lastSelectionCheckTime = Time.time;
            SelectionCheck();
        }

        if (Mouse.current.leftButton.isPressed && _currentSelectedCharacter != null)
        {
            CastCombatAction();
        }
    }

    public void SetCurrentCombatAction(CombatAction combatAction)
    {
        _currentSelectedCombatAction = combatAction;

        _canSelectSelf = false;
        _canSelectTeam = false;
        _canSelectEnemy = false;

        if(combatAction as MeleeCombatAction || combatAction as RangedCombatAction || combatAction as MultiTargetRangedCombatAction)
        {
            _canSelectEnemy = true;
        }
        else if(combatAction as HealCombatAction)
        {
            _canSelectSelf = true;
            _canSelectTeam = true;
        }
        else if(combatAction as EffectCombatAction)
        {
            _canSelectSelf = (combatAction as EffectCombatAction).canAffectSelf;
            _canSelectTeam = (combatAction as EffectCombatAction).canAffectTeam;
            _canSelectEnemy = (combatAction as EffectCombatAction).canAffectEnemy;
        }
    }

    private void OnNewTurn()
    {
        if (TurnManager.instance.CurrentTurnCharacter.team == Character.Team.Player)
            EnablePlayerCombat();
        else
            DisablePlayerCombat();
    }

    private void EnablePlayerCombat()
    {
        _currentSelectedCharacter = null;
        _currentSelectedCombatAction = null;
        _isActive = true;
    }

    private void DisablePlayerCombat()
    {
        _isActive = false;
    }

    /// <summary>
    /// Creates a Ray in screen space based on the mouse position and determines which character should be selected. 
    /// Unselects the character if the hovered character is not viable.
    /// </summary>
    private void SelectionCheck()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 999, selectionLayerMask))
        {
            Character character = hit.collider.GetComponent<Character>();

            // Only want to call SelectCharacter function once when hovered over a character
            if (_currentSelectedCharacter != null && _currentSelectedCharacter == character)
                return;

            if (_currentSelectedCombatAction.GetType() == typeof(MultiTargetRangedCombatAction) && character.team == Character.Team.Enemy)
            {
                SelectCharacters(GameManager.instance.enemyTeam);
                return;
            }
            // Able to select self and character hovered over is on their turn
            else if (_canSelectSelf && character == TurnManager.instance.CurrentTurnCharacter)
            {
                SelectCharacter(character);
                return;
            }
            // Able to select teammates and the character hovered over is on the Player team
            else if (_canSelectTeam && character.team == Character.Team.Player)
            {
                SelectCharacter(character);
                return;
            }
            // Able to select an enemy and the character hovered over is on the Enemy team
            else if (_canSelectEnemy && character.team == Character.Team.Enemy && _currentSelectedCombatAction.GetType() != typeof(MultiTargetRangedCombatAction))
            {
                SelectCharacter(character);
                return;
            }
        }

        ChooseUnselectType();
    }

    private void SelectCharacter(Character character)
    {
        UnselectCharacter();
        _currentSelectedCharacter = character;
        character.ToggleSelectionVisual(true);
    }

    private void SelectCharacters(Character[] characters)
    {
        UnselectCharacters(characters);

        for (int i = 0; i < characters.Length; i++)
        {
            _currentSelectedCharacter = characters[i];
            characters[i].ToggleSelectionVisual(true);
        }
    }

    private void UnselectCharacter()
    {
        if (_currentSelectedCharacter == null)
            return;

        _currentSelectedCharacter.ToggleSelectionVisual(false);
        _currentSelectedCharacter = null;
    }

    private void UnselectCharacters(Character[] characters)
    {
        if (_currentSelectedCharacter == null)
            return;

        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].ToggleSelectionVisual(false);
        }

        _currentSelectedCharacter = null;
    }

    private void ChooseUnselectType()
    {
        if (_currentSelectedCombatAction.GetType() == typeof(MultiTargetRangedCombatAction))
            UnselectCharacters(GameManager.instance.enemyTeam);
        else
            UnselectCharacter();
    }

    private void CastCombatAction()
    {
        if (TurnManager.instance.CurrentTurnCharacter.currentMana >= _currentSelectedCombatAction.manaCost)
        {
            if(_currentSelectedCombatAction.GetType() != typeof(MultiTargetRangedCombatAction))
                TurnManager.instance.CurrentTurnCharacter.CastCombatAction(_currentSelectedCombatAction, false, _currentSelectedCharacter);
            else
                TurnManager.instance.CurrentTurnCharacter.CastCombatAction(_currentSelectedCombatAction, true, _currentSelectedCharacter);

            ChooseUnselectType();
            _currentSelectedCombatAction = null;
            DisablePlayerCombat();
            combatActionsUI.DisableCombatActions();
            TurnManager.instance.endTurnButton.SetActive(false);

            Invoke(nameof(NextTurnDelay), 1);
        }
        else
        {
            NotificationManager.instance.ActivateNotification();
            NotificationManager.instance.ChangeText("Not enough mana.");

            ChooseUnselectType();
            _currentSelectedCombatAction = null;
            TurnManager.instance.ResetTurn();
        }
    }

    private void NextTurnDelay()
    {
        TurnManager.instance.EndTurn();
    }
}