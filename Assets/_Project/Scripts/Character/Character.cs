using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public static UnityAction<Character> OnCharacterDeath;

    public enum Team
    {
        Player,
        Enemy
    }

    [Header("Stats")]
    public Team team;
    public string displayName;
    public int currentHealth;
    public int maxHealth;
    public int currentMana;
    public int maxMana;

    [Header("Combat Actions")]
    public CombatAction[] combatActions;

    [Header("Components")]
    public CharacterEffects characterEffects;
    public CharacterUI characterUI;
    public GameObject selectionVisualGO;
    public DamageFlash damageFlash;

    [Header("Prefabs")]
    public GameObject healParticlePrefab;

    private Vector3 _standingPosition;

    private void Start()
    {
        _standingPosition = transform.position;
        characterUI.SetCharacterNameText(displayName);
        characterUI.UpdateHealthBar(currentHealth, maxHealth);

        if (team == Team.Enemy)
            characterUI.manaBarGO.SetActive(false);
        if (team == Team.Player)
            characterUI.UpdateManaBar(currentMana, maxMana);
    }

    private void OnEnable()
    {
        TurnManager.instance.OnNewTurn += OnNewTurn;
    }

    private void OnDisable()
    {
        TurnManager.instance.OnNewTurn -= OnNewTurn;
    }

    public void ToggleSelectionVisual(bool toggle)
    {
        if(selectionVisualGO != null)
            selectionVisualGO.SetActive(toggle);
    }

    public void CastCombatAction(CombatAction combatAction, bool isMulti, Character target = null)
    {
        if (target == null)
            target = this;

        if (!isMulti)
            combatAction.Cast(this, target);
        else
        {
            if (team == Team.Enemy)
                combatAction.MultiCast(this, GameManager.instance.playerTeam);
            else if (team == Team.Player)
                combatAction.MultiCast(this, GameManager.instance.enemyTeam);
        }

        currentMana -= combatAction.manaCost;
        characterUI.UpdateManaBar(currentMana, maxMana);
    }

    public void TakeDamageConstant(int amount)
    {
        currentHealth -= amount;

        characterUI.UpdateHealthBar(currentHealth, maxHealth);

        damageFlash.Flash();

        if (currentHealth <= 0)
            Die();
    }

    public void TakeDamageRandom(int minAmount, int maxAmount)
    {
        int randomAmount = Random.Range(minAmount, maxAmount + 1);
        currentHealth -= randomAmount;

        characterUI.UpdateHealthBar(currentHealth, maxHealth);

        damageFlash.Flash();

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        characterUI.UpdateHealthBar(currentHealth, maxHealth);
        Instantiate(healParticlePrefab, transform);
    }

    public void MoveToTarget(Character target, UnityAction<Character> arriveCallback)
    {
        StartCoroutine(MeleeAttackAnimation());

        IEnumerator MeleeAttackAnimation()
        {
            while(transform.position != target.transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 10 * Time.deltaTime);
                yield return null;
            }

            arriveCallback?.Invoke(target);

            while(transform.position != _standingPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, _standingPosition, 10 * Time.deltaTime);
                yield return null;
            }
        }
    }

    private void OnNewTurn()
    {
        characterUI.ToggleTurnVisual(TurnManager.instance.CurrentTurnCharacter == this);
        characterEffects.ApplyCurrentActiveEffects();
    }

    private void Die()
    {
        OnCharacterDeath?.Invoke(this);
        Destroy(gameObject);
    }
}