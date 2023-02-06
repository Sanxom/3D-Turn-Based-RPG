using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectInstance
{
    public GameObject currentActiveGameObject;
    public ParticleSystem currentTickParticle;
    public Effect effect;
    public int turnsRemaining;

    public EffectInstance(Effect effect)
    {
        this.effect = effect;
        turnsRemaining = effect.durationOfTurns;
    }
}