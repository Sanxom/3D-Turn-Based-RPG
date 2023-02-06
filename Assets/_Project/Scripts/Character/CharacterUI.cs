using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterUI : MonoBehaviour
{
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI healthText;
    public Image healthFill;
    public Image turnVisual;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        transform.forward = transform.position - _mainCamera.transform.position;
    }

    public void ToggleTurnVisual(bool toggle)
    {
        turnVisual.gameObject.SetActive(toggle);
    }

    public void SetCharacterNameText(string characterName)
    {
        characterNameText.text = characterName;
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthText.text = $"{currentHealth} / {maxHealth}";
        healthFill.fillAmount = (float)currentHealth / (float)maxHealth;
    }
}