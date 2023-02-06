using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum Team
    {
        Player,
        Enemy
    }

    [Header("Stats")]
    public Team team;
    public string displayName;
    public int currentHealth;
    public int maxHealth;
    public int currentMana;
    public int maxMana;

    [Header("Components")]
    public CharacterEffects characterEffects;
    public CharacterUI characterUI;
    public GameObject selectionVisualGO;
    public DamageFlash damageFlash;

    [Header("Prefabs")]
    public GameObject healParticlePrefab;

    private Vector3 _standingPosition;
}