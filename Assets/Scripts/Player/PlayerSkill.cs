using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    [Header("Skill Settings")]
    public float maxSkillPoints = 100f;
    float currentSkillPoints = 0f;

    [Header("Skill Properties")]
    public int skillDamage = 50; // Özel skill hasarı
    public float skillPointsPerHit = 15f; // Her vuruşta kazanılan skill puanı

    [Header("UI")]
    public Image skillBar;
    public GameObject skillReadyIndicator; // "READY!" yazısı (opsiyonel)

    void Start()
    {
        currentSkillPoints = 0f;
        UpdateUI();

        if (skillReadyIndicator != null)
            skillReadyIndicator.SetActive(false);
    }

    void Update()
    {
        // 🔥 L tuşuna basınca skill kullan
        if (Input.GetKeyDown(KeyCode.L) && IsSkillReady())
        {
            UseSkill();
        }

        UpdateUI();
    }

    // 🔥 Her vuruşta çağrılacak (PlayerAttack'tan)
    public void AddSkillPoints(float points)
    {
        currentSkillPoints += points;
        currentSkillPoints = Mathf.Clamp(currentSkillPoints, 0f, maxSkillPoints);

        Debug.Log($"⚡ Skill Points: {currentSkillPoints}/{maxSkillPoints}");

        // Skill hazır mı?
        if (IsSkillReady() && skillReadyIndicator != null)
        {
            skillReadyIndicator.SetActive(true);
        }
    }

    public bool IsSkillReady()
    {
        return currentSkillPoints >= maxSkillPoints;
    }

    void UseSkill()
    {
        Debug.Log("🔥 PLAYER ÖZEl SKILL KULLANILDI!");

        // 🔥 Enemy'ye büyük hasar ver
        GameObject enemyObj = GameObject.FindGameObjectWithTag("Enemy");
        if (enemyObj != null)
        {
            EnemyHealth enemyHealth = enemyObj.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(skillDamage);
                Debug.Log($"💥 Skill Hasarı: {skillDamage}!");
            }
        }

        // 🔥 Skill barını sıfırla
        currentSkillPoints = 0f;

        if (skillReadyIndicator != null)
            skillReadyIndicator.SetActive(false);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (skillBar != null)
            skillBar.fillAmount = currentSkillPoints / maxSkillPoints;
    }

    // 🔥 Round resetinde skill sıfırla (opsiyonel)
    public void ResetSkill()
    {
        currentSkillPoints = 0f;
        if (skillReadyIndicator != null)
            skillReadyIndicator.SetActive(false);
        UpdateUI();
    }
}
