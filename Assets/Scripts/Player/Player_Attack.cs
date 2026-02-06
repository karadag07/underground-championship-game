using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 0.5f;

    float lastAttackTime;
    PlayerStamina playerStamina;
    PlayerSkill playerSkill;

    void Start()
    {
        Debug.Log("✅ PlayerAttack Start() çağrıldı!");
        playerStamina = GetComponent<PlayerStamina>();
        playerSkill = GetComponent<PlayerSkill>();

        if (playerStamina == null)
            Debug.LogWarning("⚠️ PlayerStamina component bulunamadı!");
        if (playerSkill == null)
            Debug.LogWarning("⚠️ PlayerSkill component bulunamadı!");
    }

    void Update()
    {
        // 🔍 HER FRAME KONTROL
        Debug.Log($"⏰ Update çalışıyor - Time.timeScale: {Time.timeScale}");

        // 🔍 J TUŞU DURUMU
        bool jPressed = Input.GetKeyDown(KeyCode.J);
        Debug.Log($"🎮 J tuşu durumu: {jPressed}");

        if (jPressed)
        {
            Debug.Log("🔴🔴🔴 J TUŞUNA BASILDI! 🔴🔴🔴");
            TryAttack();
        }

        // 🔍 DİĞER INPUT TESTLERİ
        if (Input.anyKeyDown)
        {
            Debug.Log($"Bir tuşa basıldı! Input.inputString: '{Input.inputString}'");
        }
    }

    void TryAttack()
    {
        Debug.Log("🎯 TryAttack() fonksiyonu çağrıldı!");

        // 🔍 BLOCK KONTROLÜ
        bool isBlocking = playerStamina != null && playerStamina.IsBlocking();
        Debug.Log($"Block durumu: {isBlocking}");

        if (isBlocking)
        {
            Debug.Log("⚠️ Block yapılıyor, saldırı yapılamıyor!");
            return;
        }

        // 🔍 COOLDOWN KONTROLÜ
        float timeSinceLastAttack = Time.time - lastAttackTime;
        Debug.Log($"Son saldırıdan beri geçen süre: {timeSinceLastAttack:F2}s | Cooldown: {attackCooldown}s");

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("✅ Cooldown tamam, PerformAttack() çağrılıyor!");
            PerformAttack();
            lastAttackTime = Time.time;
        }
        else
        {
            float remaining = attackCooldown - timeSinceLastAttack;
            Debug.Log($"⏳ Cooldown bekleniyor! {remaining:F1}s kaldı");
        }
    }

    void PerformAttack()
    {
        Debug.Log("💥 PerformAttack() çağrıldı!");

        GameObject enemyObj = GameObject.FindGameObjectWithTag("Enemy");

        if (enemyObj == null)
        {
            Debug.LogError("❌ Enemy objesi bulunamadı! Tag kontrol et!");

            // 🔍 Tüm objeleri listele
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            Debug.Log($"Scene'deki toplam obje sayısı: {allObjects.Length}");

            return;
        }

        Debug.Log($"✅ Enemy objesi bulundu: {enemyObj.name}");

        float distance = Vector2.Distance(transform.position, enemyObj.transform.position);
        Debug.Log($"🎯 Player pozisyon: {transform.position}");
        Debug.Log($"🎯 Enemy pozisyon: {enemyObj.transform.position}");
        Debug.Log($"🎯 Mesafe: {distance:F2} | Attack Range: {attackRange}");

        if (distance <= attackRange)
        {
            Debug.Log("✅ Mesafe uygun! Enemy'ye saldırılıyor...");

            EnemyHealth enemyHealth = enemyObj.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                Debug.Log($"💥💥💥 SALDIRI BAŞARILI! Hasar: {attackDamage} 💥💥💥");
                enemyHealth.TakeDamage(attackDamage);

                // Skill puanı kazan
                if (playerSkill != null)
                {
                    playerSkill.AddSkillPoints(playerSkill.skillPointsPerHit);
                    Debug.Log("⚡ Skill puanı eklendi!");
                }
            }
            else
            {
                Debug.LogError("❌ EnemyHealth component yok!");

                // 🔍 Enemy'deki tüm componentleri listele
                Component[] components = enemyObj.GetComponents<Component>();
                Debug.Log($"Enemy'deki componentler ({components.Length}):");
                foreach (Component comp in components)
                {
                    Debug.Log($"  - {comp.GetType().Name}");
                }
            }
        }
        else
        {
            Debug.Log($"❌ Enemy çok uzak! Mesafe: {distance:F2}, Gerekli: {attackRange}");
            Debug.Log("💡 Enemy'ye yaklaş (A/D tuşları) ve tekrar dene!");
        }
    }

    // 🔍 Component aktif mi kontrol et
    void OnEnable()
    {
        Debug.Log("✅ PlayerAttack component aktif edildi!");
    }

    void OnDisable()
    {
        Debug.Log("❌ PlayerAttack component deaktif edildi!");
    }
}