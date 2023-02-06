using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Effect", menuName = "Effects/Damage Effect")]
public class DamageEffect : Effect
{
    public int minDamage;
    public int maxDamage;
}