using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Combat Action", menuName = "Combat Actions/Ranged Combat Action")]
public class RangedCombatAction : CombatAction
{
    public GameObject projectilePrefab;

    public override void Cast (Character caster, Character target)
    {
        if (caster == null || target == null)
            return;

        GameObject proj = Instantiate(projectilePrefab, caster.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        proj.GetComponent<Projectile>().InitializeSingle(target);
    }

    public override void MultiCast(Character caster, Character[] targets) { }
}