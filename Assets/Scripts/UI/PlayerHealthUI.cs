using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] Image healthImage;

    Health playerHealth;
    float currentHealth;

    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

    }

    void Update()
    {
        healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, playerHealth.GetHealthRatio(), 0.1f);
    }
}
