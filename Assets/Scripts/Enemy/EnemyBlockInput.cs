using UnityEngine;

public class EnemyBlockInput : MonoBehaviour
{
    EnemyStamina enemyStamina;

    [Header("Block AI")]
    public float blockChance = 0.3f; // %30 ihtimalle block yapar
    public float blockDuration = 0.5f; // 0.5 saniye block'ta kalır

    void Start()
    {
        enemyStamina = GetComponent<EnemyStamina>();
    }

    // Enemy vurulduğunda EnemyHealth burayı çağıracak
    public void TryBlock()
    {
        if (enemyStamina == null)
            return;

        // Zaten block yapıyorsa tekrar başlatma
        if (enemyStamina.IsBlocking())
            return;

        // %30 ihtimalle block yap
        if (Random.value < blockChance)
        {
            enemyStamina.SetBlocking(true);
            Debug.Log("ENEMY BLOCK BAŞLADI!");

            // Belirli süre sonra block'u kapat
            Invoke("StopBlock", blockDuration);
        }
    }

    void StopBlock()
    {
        if (enemyStamina != null)
        {
            enemyStamina.SetBlocking(false);
            Debug.Log("ENEMY BLOCK BİTTİ!");
        }
    }

    // Manuel olarak block'u durdurmak için (opsiyonel)
    public void ForceStopBlock()
    {
        CancelInvoke("StopBlock"); // Bekleyen Invoke'ları iptal et
        StopBlock();
    }
}