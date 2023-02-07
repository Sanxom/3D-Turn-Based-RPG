using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatAction : ScriptableObject
{
    public string displayName;
    public string description;
    public int manaCost;

    public abstract void Cast(Character caster, Character target);
}