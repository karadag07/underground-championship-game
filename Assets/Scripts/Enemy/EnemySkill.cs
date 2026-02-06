
using UnityEngine;
using UnityEngine.UI;

public class EnemySkill : MonoBehaviour
{
    [Header("Skill Settings")]
    public float maxSkillPoints = 100f;
    float currentSkillPoints = 0f;

    [Header("Skill Properties")]
    public int skillDamage = 50;
    public float skillPointsPerHit = 15f;

    [Header("UI")]
    public Image skillBar;

    [Header("AI Behavior")]
    public float useSkillChance = 0.8f; // %80 ihtimalle kullanır

    void Start()
    {
        currentSkillPoints = 0f;
        UpdateUI();
    }

    void Update()
    {
        // 🔥 AI otomatik skill kullanır (skill hazırsa)
        if (IsSkillReady())
        {
            TryUseSkill();
        }

        UpdateUI();
    }

    public void AddSkillPoints(float points)
    {
        currentSkillPoints += points;
        currentSkillPoints = Mathf.Clamp(currentSkillPoints, 0f, maxSkillPoints);

        Debug.Log($"⚡ Enemy Skill Points: {currentSkillPoints}/{maxSkillPoints}");
    }

    public bool IsSkillReady()
    {
        return currentSkillPoints >= maxSkillPoints;
    }

    void TryUseSkill()
    {
        // 🔥 %80 ihtimalle kullan (hemen kullanmasın)
        if (Random.value < useSkillChance)
        {
            UseSkill();
        }
    }

    void UseSkill()
    {
        Debug.Log("🔥 ENEMY ÖZEL SKILL KULLANILDI!");

        // 🔥 Player'a büyük hasar ver
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            PlayerHealth playerHealth = playerObj.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(skillDamage);
                Debug.Log($"💥 Enemy Skill Hasarı: {skillDamage}!");
            }
        }

        // 🔥 Skill barını sıfırla
        currentSkillPoints = 0f;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (skillBar != null)
            skillBar.fillAmount = currentSkillPoints / maxSkillPoints;
    }

    public void ResetSkill()
    {
        currentSkillPoints = 0f;
        UpdateUI();
    }
}