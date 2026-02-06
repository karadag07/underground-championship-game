using UnityEngine;

public class PlayerKick : MonoBehaviour
{
    [Header("Kick Settings")]
    public int kickDamage = 15; // Yumruktan güçlü
    public float kickRange = 2.0f; // Daha uzun menzil
    public float kickCooldown = 0.8f; // Biraz yavaş

    float lastKickTime;
    PlayerStamina playerStamina;
    PlayerSkill playerSkill;

    void Start()
    {
        playerStamina = GetComponent<PlayerStamina>();
        playerSkill = GetComponent<PlayerSkill>();
    }

    void Update()
    {
        // 🔥 H TUŞUNA BASILINCA TEKME AT
        if (Input.GetKeyDown(KeyCode.H))
        {
            TryKick();
        }
    }

    void TryKick()
    {
        // Block yapılıyorsa tekme atma
        if (playerStamina != null && playerStamina.IsBlocking())
        {
            Debug.Log("⚠️ Block yapılıyor, tekme atılamıyor!");
            return;
        }

        // Cooldown kontrolü
        if (Time.time >= lastKickTime + kickCooldown)
        {
            PerformKick();
            lastKickTime = Time.time;
        }
        else
        {
            float remaining = kickCooldown - (Time.time - lastKickTime);
            Debug.Log($"⏳ Tekme cooldown! {remaining:F1}s kaldı");
        }
    }

    void PerformKick()
    {
        Debug.Log("🦶 TEKME ATILIYOR!");

        // Enemy'yi bul
        GameObject enemyObj = GameObject.FindGameObjectWithTag("Enemy");

        if (enemyObj == null)
        {
            Debug.LogWarning("⚠️ Enemy bulunamadı!");
            return;
        }

        // Mesafe kontrolü
        float distance = Vector2.Distance(transform.position, enemyObj.transform.position);
        Debug.Log($"🎯 Tekme menzili: {distance:F2} / {kickRange}");

        if (distance <= kickRange)
        {
            // Enemy'ye hasar ver
            EnemyHealth enemyHealth = enemyObj.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                Debug.Log($"💥 TEKME VURDU! Hasar: {kickDamage}");
                enemyHealth.TakeDamage(kickDamage);

                // Skill puanı kazan (tekme de puan kazandırır)
                if (playerSkill != null)
                {
                    playerSkill.AddSkillPoints(playerSkill.skillPointsPerHit);
                }
            }
        }
        else
        {
            Debug.Log($"❌ Çok uzak! Mesafe: {distance:F2}, Gerekli: {kickRange}");
        }
    }
}