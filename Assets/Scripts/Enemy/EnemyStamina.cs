using UnityEngine;
using UnityEngine.UI;

public class EnemyStamina : MonoBehaviour
{
    public float maxStamina = 100f;
    float currentStamina;

    public float drainPerSecond = 20f;
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
                Debug.Log("⚠️ ENEMY STAMINA BİTTİ - BLOCK KAPANDI!");
            }
        }
        else
        {
            currentStamina += regenPerSecond * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }

        UpdateUI();
    }

    public void SetBlocking(bool value)
    {
        if (value && currentStamina <= 0f)
        {
            Debug.Log("❌ STAMINA YOK - BLOCK YAPILAMIYOR!");
            return;
        }

        isBlocking = value;
    }

    public bool IsBlocking()
    {
        return isBlocking;
    }

    public bool CanBlock()
    {
        return currentStamina > 0f;
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
        Debug.Log("Enemy stamina resetlendi!");
    }

}