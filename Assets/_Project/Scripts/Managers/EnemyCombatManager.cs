using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyCombatManager : MonoBehaviour
{
    [Header("AI")]
    public float minWaitTime;
    public float maxWaitTime;

    [Header("Attacking")]
    public float attackWeakestChance;

    [Header("Chance Curves")]
    public AnimationCurve healChanceCurve;

    private Character _currentEnemy;

    private void OnEnable()
    {
        TurnManager.instance.OnNewTurn += OnNewTurn;
    }

    private void OnDisable()
    {
        TurnManager.instance.OnNewTurn -= OnNewTurn;
    }

    private void OnNewTurn()
    {
        if(TurnManager.instance.CurrentTurnCharacter.team == Character.Team.Enemy)
        {
            _currentEnemy = TurnManager.instance.CurrentTurnCharacter;
            Invoke(nameof(DecideCombatAction), Random.Range(minWaitTime, maxWaitTime));
        }
    }

    private void DecideCombatAction()
    {
        // Does enemy need to heal a teammate? Based on the AnimationCurve
        if (HasCombatActionOfType(typeof(HealCombatAction)))
        {
            Character weakestEnemy = GetWeakestLivingCharacter(Character.Team.Enemy);

            if(Random.value < healChanceCurve.Evaluate(GetHealthPercentage(weakestEnemy)))
            {
                CastCombatAction(GetHealCombatAction(), weakestEnemy);
                return;
            }
        }

        // Deal damage to a Player character
        Character playerToDamage;
        if(Random.value < attackWeakestChance)
            playerToDamage = GetWeakestLivingCharacter(Character.Team.Player);
        else
            playerToDamage = GetRandomLivingCharacter(Character.Team.Player);
        
        if(playerToDamage != null)
        {
            if(HasCombatActionOfType(typeof(MeleeCombatAction)) || HasCombatActionOfType(typeof(RangedCombatAction)))
            {
                CastCombatAction(GetDamageCombatAction(), playerToDamage);
                return;
            }
        }

        Invoke(nameof(EndTurn), Random.Range(minWaitTime, maxWaitTime));
    }

    private void CastCombatAction(CombatAction combatAction, Character target)
    {
        _currentEnemy.CastCombatAction(combatAction, target);
        Invoke(nameof(EndTurn), Random.Range(minWaitTime, maxWaitTime));
    }

    private void EndTurn()
    {
        TurnManager.instance.EndTurn();
    }

    private float GetHealthPercentage(Character character)
    {
        return (float)character.currentHealth / (float)character.maxHealth;
    }

    private bool HasCombatActionOfType(System.Type type)
    {
        foreach (CombatAction ca in _currentEnemy.combatActions)
        {
            if (ca.GetType() == type)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns a random Melee or Ranged CombatAction from the current enemy's list of CombatActions
    /// </summary>
    /// <returns></returns>
    private CombatAction GetDamageCombatAction()
    {
        CombatAction[] ca = _currentEnemy.combatActions.Where(x => x.GetType() == typeof(MeleeCombatAction) || x.GetType() == typeof(RangedCombatAction)).ToArray();

        if (ca == null || ca.Length == 0)
            return null;

        return ca[Random.Range(0, ca.Length)];
    }

    private CombatAction GetHealCombatAction()
    {
        CombatAction[] ca = _currentEnemy.combatActions.Where(x => x.GetType() == typeof(HealCombatAction)).ToArray();

        if (ca == null || ca.Length == 0)
            return null;

        return ca[Random.Range(0, ca.Length)];
    }

    private CombatAction GetEffectCombatAction()
    {
        CombatAction[] ca = _currentEnemy.combatActions.Where(x => x.GetType() == typeof(EffectCombatAction)).ToArray();

        if (ca == null || ca.Length == 0)
            return null;

        return ca[Random.Range(0, ca.Length)];
    }

    /// <summary>
    /// Returns the weakest health living character on the team that is being targeted by an Enemy.
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    private Character GetWeakestLivingCharacter(Character.Team team)
    {
        int weakestHealth = 999999;
        int weakestIndex = 0;

        Character[] characters = team == Character.Team.Player ? GameManager.instance.playerTeam : GameManager.instance.enemyTeam;

        for (int i = 0; i < characters.Length; i++)
        {
            // If dead, skip this iteration
            if (characters[i] == null)
                continue;

            if (characters[i].currentHealth < weakestHealth)
            {
                weakestHealth = characters[i].currentHealth;
                weakestIndex = i;
            }
        }

        return characters[weakestIndex];
    }

    private Character GetRandomLivingCharacter(Character.Team team)
    {
        Character[] characters = null;

        // Makes sure to only find a Player or Enemy character that is alive (not null)
        if (team == Character.Team.Player)
            characters = GameManager.instance.playerTeam.Where(x => x != null).ToArray();
        else if (team == Character.Team.Enemy)
            characters = GameManager.instance.enemyTeam.Where(x => x != null).ToArray();

        return characters[Random.Range(0, characters.Length)];
    }
}