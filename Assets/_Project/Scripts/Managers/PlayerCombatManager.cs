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
        
    }

    private void OnDisable()
    {
        
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
    }

    public void SetCurrentCombatAction(CombatAction combatAction)
    {

    }

    private void OnNewTurn()
    {

    }

    private void EnablePlayerCombat()
    {

    }

    private void DisablePlayerCombat()
    {

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

            // Able to select self and character hovered over is on their turn
            if(_canSelectSelf && character == TurnManager.instance.CurrentTurnCharacter)
            {
                SelectCharacter(character);
                return;
            }
            // Able to select teammates and the character hovered over is on the Player team
            else if(_canSelectTeam && character.team == Character.Team.Player)
            {
                SelectCharacter(character);
                return;
            }
            else if(_canSelectEnemy && character.team == Character.Team.Enemy)
            {
                SelectCharacter(character);
                return;
            }
        }

        UnselectCharacter();
    }

    private void SelectCharacter(Character character)
    {

    }

    private void UnselectCharacter()
    {

    }

    private void CastCombatAction()
    {

    }

    private void NextTurnDelay()
    {

    }
}