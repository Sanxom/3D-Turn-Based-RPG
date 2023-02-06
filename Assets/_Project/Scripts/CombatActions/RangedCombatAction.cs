using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Combat Action", menuName = "Combat Actions/Ranged Combat Action")]
public class RangedCombatAction : CombatAction
{
    public GameObject projectilePrefab;

    public override void Cast(Character caster, Character target)
    {

    }
}