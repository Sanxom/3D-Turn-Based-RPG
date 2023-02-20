using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Combat Action", menuName = "Combat Actions/Heal Combat Action")]
public class HealCombatAction : CombatAction
{
    public int healAmount;

    public override void Cast(Character caster, Character target)
    {
        if (caster == null || target == null)
            return;

        target.Heal(healAmount);
    }

    public override void MultiCast(Character caster, Character[] targets){}
}