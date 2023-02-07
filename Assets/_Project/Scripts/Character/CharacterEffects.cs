using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    private List<EffectInstance> _currentEffects = new();
    private Character _character;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void AddNewEffect(Effect effect)
    {
        EffectInstance effectInstance = new EffectInstance(effect);

        if(effect.activePrefabGO != null)
            effectInstance.currentActiveGameObject = Instantiate(effect.activePrefabGO, transform);

        if(effect.tickPrefabGO != null)
            effectInstance.currentTickParticle = Instantiate(effect.tickPrefabGO, transform).GetComponent<ParticleSystem>();

        _currentEffects.Add(effectInstance);
        ApplyEffect(effectInstance);
    }

    /// <summary>
    /// Done at the start of each turn for ongoing effects.
    /// </summary>
    public void ApplyCurrentActiveEffects()
    {
        for (int i = 0; i < _currentEffects.Count; i++)
        {
            ApplyEffect(_currentEffects[i]);
        }
    }

    /// <summary>
    /// Checks a single instance of an effect to determine which functionality to use.
    /// </summary>
    private void ApplyEffect(EffectInstance effectInstance)
    {
        if (TurnManager.instance.CurrentTurnCharacter == GetComponent<Character>())
        {
            effectInstance.currentTickParticle.Play();

            if (effectInstance.effect as DamageEffect)
            {
                _character.TakeDamageConstant((effectInstance.effect as DamageEffect).damagePerTurn);
            }
            else if (effectInstance.effect as HealEffect)
            {
                _character.Heal((effectInstance.effect as HealEffect).heal);
            }

            effectInstance.turnsRemaining--;

            if (effectInstance.turnsRemaining == 0)
                RemoveEffect(effectInstance);
        }
    }

    private void RemoveEffect(EffectInstance effectInstance)
    {
        if(effectInstance.currentActiveGameObject != null)
            Destroy(effectInstance.currentActiveGameObject);

        if(effectInstance.currentTickParticle != null)
            Destroy(effectInstance.currentTickParticle);

        _currentEffects.Remove(effectInstance);
    }
}