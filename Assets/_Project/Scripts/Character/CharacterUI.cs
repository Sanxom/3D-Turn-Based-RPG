using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterUI : MonoBehaviour
{
    public GameObject manaBarGO;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public Image healthFill;
    public Image manaFill;
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
    public void UpdateManaBar(int currentMana, int maxMana)
    {
        manaText.text = $"{currentMana} / {maxMana}";
        manaFill.fillAmount = (float)currentMana / (float)maxMana;
    }
}