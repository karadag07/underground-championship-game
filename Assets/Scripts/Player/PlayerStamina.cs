using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    public float maxStamina = 100f;
    float currentStamina;

    public float drainPerSecond = 25f;
    public float regenPerSecond = 8f;

    public Image staminaBar;

    bool isBlocking;

    void Start()
    {
        currentStamina = maxStamina;
        UpdateUI();
    }

    void Update()
    {
        HandleStamina();
    }

    void HandleStamina()
    {
        if (isBlocking && currentStamina > 0f)
        {
            currentStamina -= drainPerSecond * Time.deltaTime;

            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                isBlocking = false;
            }
        }
        else
        {
            currentStamina += regenPerSecond * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }

        UpdateUI();
    }

    public bool CanBlock()
    {
        return currentStamina > 0f;
    }

    public void SetBlocking(bool value)
    {
        if (value && currentStamina <= 0f)
            return;

        isBlocking = value;
    }

    public bool IsBlocking()
    {
        return isBlocking;
    }

    void UpdateUI()
    {
        if (staminaBar != null)
            staminaBar.fillAmount = currentStamina / maxStamina;
    }
    // 🔥 YENİ: Round başında stamina resetle
    public void ResetStamina()
    {
        currentStamina = maxStamina;
        isBlocking = false;
        UpdateUI();
        Debug.Log("Player stamina resetlendi!");
    }
}
