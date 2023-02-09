using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect Combat Action", menuName = "Combat Actions/Effect Combat Action")]
public class EffectCombatAction : CombatAction
{
    public Effect effectToCast;
    public bool canAffectSelf;
    public bool canAffectTeam;
    public bool canAffectEnemy;

    public override void Cast(Character caster, Character target)
    {
        if(caster == null || target == null)
            return;

        target.characterEffects.AddNewEffect(effectToCast);
    }
}