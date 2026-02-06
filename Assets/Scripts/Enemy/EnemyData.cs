using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Underground Championship/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Rakip Bilgileri")]
    public int enemyID; // 1-10 arası
    public string enemyName; // "Çırak", "Acemi Dövüşçü", "Boss" vs.
    public Sprite enemyPortrait; // Harita ekranındaki resim
    public Sprite enemySprite; // Dövüş ekranındaki sprite

    [Header("İstatistikler")]
    public int maxHealth = 100;
    public int attackDamage = 10;
    public float moveSpeed = 2f;

    [Header("AI Davranışı")]
    [Range(0f, 1f)]
    public float blockChance = 0.3f; // Block yapma ihtimali
    public float blockDuration = 0.5f; // Block süresi

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float staminaDrain = 20f;
    public float staminaRegen = 15f;
}