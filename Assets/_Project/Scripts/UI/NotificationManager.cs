using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager instance;

    [SerializeField] private GameObject notificationPanelGO;
    [SerializeField] private TextMeshProUGUI notificationText;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void ActivateNotification()
    {
        notificationPanelGO.SetActive(true);
    }

    public void DeactivateNotification()
    {
        notificationPanelGO.SetActive(false);
    }

    public void ChangeText(string newText)
    {
        StopCoroutine(TextAnimation());
        StartCoroutine(TextAnimation());

        notificationText.text = newText;

        IEnumerator TextAnimation()
        {
            yield return new WaitForSeconds(1f);
            DeactivateNotification();
        }
    }
}