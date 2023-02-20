using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Multi Target Ranged Combat Action", menuName = "Combat Actions/Multi Target Ranged Combat Action")]
public class MultiTargetRangedCombatAction : CombatAction
{
    public GameObject projectilePrefab;

    public override void Cast(Character caster, Character target){}

    public override void MultiCast(Character caster, Character[] targets) 
    {
        if (caster == null || targets == null)
            return;

        GameObject proj = Instantiate(projectilePrefab, caster.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        proj.GetComponent<Projectile>().InitializeMultiple(targets);
    }
}