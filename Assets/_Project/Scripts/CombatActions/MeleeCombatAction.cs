using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee Combat Action", menuName = "Combat Actions/Melee Combat Action")]
public class MeleeCombatAction : CombatAction
{
    public int minMeleeDamage;
    public int maxMeleeDamage;

    public override void Cast (Character caster, Character target)
    {
        caster.MoveToTarget(target, OnDamageTargetCallback);
    }

    private void OnDamageTargetCallback(Character target)
    {
        target.TakeDamageRandom(minMeleeDamage, maxMeleeDamage);
    }
}