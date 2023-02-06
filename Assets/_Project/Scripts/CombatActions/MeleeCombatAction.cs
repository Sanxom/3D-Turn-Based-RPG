using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee Combat Action", menuName = "Combat Actions/Melee Combat Action")]
public class MeleeCombatAction : CombatAction
{
    public int meleeDamage;

    public override void Cast (Character caster, Character target)
    {
        
    }
}