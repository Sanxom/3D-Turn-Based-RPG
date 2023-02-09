using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    private Renderer[] renderers;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void Flash()
    {
        StartCoroutine(FlashCoroutine());

        IEnumerator FlashCoroutine()
        {
            SetMREmission(Color.white);
            yield return new WaitForSeconds(0.05f);

            SetMREmission(Color.black);
        }
    }

    private void SetMREmission(Color color)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.SetColor("_EmissionColor", color);
        }
    }
}