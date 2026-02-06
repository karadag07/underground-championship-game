using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;

    public Image healthBar;

    [Header("Block Settings")]
    public float blockDamageMultiplier = 0.5f;

    PlayerStamina playerStamina;
    bool isDead = false; // 🔥 Ölü kontrolü

    void Start()
    {
        currentHealth = maxHealth;
        playerStamina = GetComponent<PlayerStamina>();
        isDead = false;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        // 🔥 Ölüyse hasar alma
        if (isDead) return;

        if (playerStamina != null && playerStamina.IsBlocking())
        {
            damage = Mathf.RoundToInt(damage * blockDamageMultiplier);
            Debug.Log("Block yapıldı, hasar azaltıldı: " + damage);
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Player HP: " + currentHealth);
        UpdateHealthBar();

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    void Die()
    {
        isDead = true; // 🔥 Ölü işaretle
        Debug.Log("💀 Player öldü - Enemy round kazandı!");

        // GameManager'a bildir: Enemy round kazandı
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemyWonRound();
        }

        // Objeyi deaktif etme, sadece işaretle
        // gameObject.SetActive(false); // ❌ BUNU KALDIRDIK
    }

    // 🔥 Round başında canı resetle
    public void ResetHealth()
    {
        isDead = false; // 🔥 Diriltme
        currentHealth = maxHealth;
        gameObject.SetActive(true);
        UpdateHealthBar();
        Debug.Log("✅ Player canı resetlendi!");
    }
}