
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;

    public Image enemyHealthBar;

    [Header("Block Settings")]
    public float blockMultiplier = 0.5f; // 🔥 Player ile aynı yaptık (%50)

    EnemyStamina enemyStamina;
    EnemyBlockInput enemyBlockInput;
    bool isDead = false; // 🔥 Ölü kontrolü

    void Start()
    {
        currentHealth = maxHealth;
        enemyStamina = GetComponent<EnemyStamina>();
        enemyBlockInput = GetComponent<EnemyBlockInput>();
        isDead = false;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        // 🔥 Ölüyse hasar alma
        if (isDead) return;

        // 🔥 VURULUNCA BLOCK DENEMESI YAP
        enemyBlockInput?.TryBlock();

        // Block yapılıyorsa hasar azalt
        if (enemyStamina != null && enemyStamina.IsBlocking())
        {
            damage = Mathf.RoundToInt(damage * blockMultiplier);
            Debug.Log("ENEMY BLOCK! Hasar azaltıldı: " + damage);
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        Debug.Log("Enemy HP: " + currentHealth);

        if (currentHealth <= 0 && !isDead)
            Die();
    }

    public bool IsBlocking()
    {
        return enemyStamina != null && enemyStamina.IsBlocking();
    }

    void UpdateHealthBar()
    {
        if (enemyHealthBar != null)
            enemyHealthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    void Die()
    {
        isDead = true; // 🔥 Ölü işaretle
        Debug.Log("🎉 Enemy öldü - Player round kazandı!");

        // GameManager'a bildir: Player round kazandı
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerWonRound();
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
        Debug.Log("✅ Enemy canı resetlendi!");
    }
}