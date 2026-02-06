using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyNode : MonoBehaviour
{
    [Header("Enemy Info")]
    public int enemyID; // 1-10

    [Header("UI")]
    public Button button;
    public Image image;

    [Header("Colors")]
    public Color unlockedColor = Color.green; // 🔥 Beyaz yerine yeşil
    public Color lockedColor = Color.gray;

    void Start()
    {
        // 🔥 Button ve Image otomatik bul
        if (button == null)
            button = GetComponent<Button>();

        if (image == null)
            image = GetComponent<Image>();

        // 🔥 İlk açılışta renk güncelle
        UpdateNode();
    }

    void Update()
    {
        // 🔥 Her frame renk güncelle (unlock olunca otomatik yeşil olur)
        UpdateNode();
    }

    void UpdateNode()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("⚠️ GameManager.Instance NULL! Node güncellenemiyor.");
            return;
        }

        // 🔥 Kilit kontrolü
        bool unlocked = GameManager.Instance.IsEnemyUnlocked(enemyID);

        // 🔥 Button aktif/pasif
        button.interactable = unlocked;

        // 🔥 Renk değiştir
        image.color = unlocked ? unlockedColor : lockedColor;
    }

    public void OnClick()
    {
        Debug.Log($"🔴 Node {enemyID} tıklandı!");

        if (GameManager.Instance == null)
        {
            Debug.LogError("❌ GameManager.Instance NULL!");
            return;
        }

        // 🔥 Kilitli node'a tıklanmasın
        if (!GameManager.Instance.IsEnemyUnlocked(enemyID))
        {
            Debug.LogWarning($"⚠️ Rakip {enemyID} KİTLİ!");
            return;
        }

        Debug.Log($"✅ Rakip {enemyID} AÇIK! FightScene'e geçiliyor...");

        // 🔥 Hangi rakiple dövüşüleceğini ayarla
        GameManager.Instance.currentEnemyID = enemyID;

        // 🔥 FightScene'e geç
        SceneManager.LoadScene("FightScene");
    }
}