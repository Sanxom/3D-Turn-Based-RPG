using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Effect effectToApply;
    public int minDamage;
    public int maxDamage;
    public int healAmount;
    public float moveSpeed;

    private Character _target;

    private void Update()
    {
        if(_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position + new Vector3(0, 0.5f, 0), moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_target != null && other.gameObject == _target.gameObject)
        {
            ImpactTarget();
            Destroy(gameObject);
        }
    }

    public void Initialize(Character target)
    {
        _target = target;
    }

    private void ImpactTarget()
    {
        if(minDamage > 0 || maxDamage > 0)
            _target.TakeDamageRandom(minDamage, maxDamage);

        if(healAmount > 0)
            _target.Heal(healAmount);

        if (effectToApply != null)
            _target.GetComponent<CharacterEffects>().AddNewEffect(effectToApply);
    }
}