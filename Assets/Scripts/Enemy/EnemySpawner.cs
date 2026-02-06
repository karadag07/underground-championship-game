using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    void Start()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        // 🔥 GameManager'dan hangi rakiple dövüşeceğimizi al
        if (GameManager.Instance == null)
        {
            Debug.LogError("❌ GameManager.Instance NULL!");
            return;
        }

        int enemyID = GameManager.Instance.currentEnemyID;
        EnemyData enemyData = GameManager.Instance.GetCurrentEnemy();

        if (enemyData == null)
        {
            Debug.LogError($"❌ Enemy Data bulunamadı! Enemy ID: {enemyID}");
            return;
        }

        Debug.Log($"🎯 Rakip {enemyID} spawn ediliyor: {enemyData.enemyName}");

        // 🔥 Enemy objesini scene'den bul
        GameObject enemyObj = GameObject.FindGameObjectWithTag("Enemy");

        if (enemyObj == null)
        {
            Debug.LogError("❌ Enemy objesi bulunamadı! Tag'i 'Enemy' mi?");
            return;
        }

        // 🔥 Enemy'nin statlarını güncelle
        ApplyEnemyData(enemyObj, enemyData);
    }

    void ApplyEnemyData(GameObject enemyObj, EnemyData data)
    {
        // 🔥 HEALTH
        EnemyHealth health = enemyObj.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.maxHealth = data.maxHealth;
            health.ResetHealth(); // Can barını güncelle
            Debug.Log($"✅ Enemy Health: {data.maxHealth}");
        }

        // 🔥 AI (Movement & Attack)
        EnemyAI ai = enemyObj.GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.moveSpeed = data.moveSpeed;
            ai.attackDamage = data.attackDamage;
            Debug.Log($"✅ Enemy Speed: {data.moveSpeed}, Damage: {data.attackDamage}");
        }

        // 🔥 BLOCK
        EnemyBlockInput blockInput = enemyObj.GetComponent<EnemyBlockInput>();
        if (blockInput != null)
        {
            blockInput.blockChance = data.blockChance;
            blockInput.blockDuration = data.blockDuration;
            Debug.Log($"✅ Enemy Block Chance: {data.blockChance * 100}%");
        }

        // 🔥 STAMINA
        EnemyStamina stamina = enemyObj.GetComponent<EnemyStamina>();
        if (stamina != null)
        {
            stamina.maxStamina = data.maxStamina;
            stamina.drainPerSecond = data.staminaDrain;
            stamina.regenPerSecond = data.staminaRegen;
            stamina.ResetStamina();
            Debug.Log($"✅ Enemy Stamina: {data.maxStamina}");
        }

        // 🔥 SPRITE (opsiyonel - eğer sprite varsa değiştir)
        if (data.enemySprite != null)
        {
            SpriteRenderer spriteRenderer = enemyObj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = data.enemySprite;
                Debug.Log($"✅ Enemy Sprite değiştirildi!");
            }
        }

        Debug.Log($"🎉 {data.enemyName} hazır!");
    }
}