using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Effect effectToApply;
    public int minDamage;
    public int maxDamage;
    public int manaCost;
    public int healAmount;
    public float moveSpeed;

    private Character _target;
    private Character[] _targets;

    private void Update()
    {
        if(_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position + new Vector3(0, 0.5f, 0), moveSpeed * Time.deltaTime);
        }

        if (_targets == null)
            return;

        for (int i = 0; i < 2; i++)
        {
            if (_targets[i] != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targets[i].transform.position + new Vector3(0, 0.5f, 0), moveSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_target != null && other.gameObject == _target.gameObject)
        {
            ImpactSingleTarget();
            Destroy(gameObject);
        }

        if (_targets == null)
            return;

        for (int i = 0; i < 2; i++)
        {
            if (_targets[i] != null && other.gameObject == _targets[i].gameObject)
            {
                ImpactMultipleTargets();
                Destroy(gameObject);
            }
        }
    }

    public void InitializeSingle(Character target)
    {
        _target = target;
    }

    public void InitializeMultiple(Character[] targets)
    {
        _targets = targets;
    }

    private void ImpactSingleTarget()
    {
        if(minDamage > 0 || maxDamage > 0)
            _target.TakeDamageRandom(minDamage, maxDamage);

        if(healAmount > 0)
            _target.Heal(healAmount);

        if (effectToApply != null)
            _target.GetComponent<CharacterEffects>().AddNewEffect(effectToApply);
    }

    private void ImpactMultipleTargets()
    {
        for (int i = 0; i < _targets.Length; i++)
        {
            if (_targets[i] == null)
                continue;

            if(minDamage > 0 || maxDamage > 0)
                _targets[i].TakeDamageRandom(minDamage, maxDamage);

            if(healAmount > 0)
                _targets[i].Heal(healAmount);

            if (effectToApply != null)
                _targets[i].GetComponent<CharacterEffects>().AddNewEffect(effectToApply);
        }
    }
}