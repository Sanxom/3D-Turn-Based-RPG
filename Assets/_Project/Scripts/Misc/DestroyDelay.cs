using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelay : MonoBehaviour
{
    [SerializeField] private float _lifetime;

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }
}