using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [Header("Currency Settings")]
    [SerializeField] private int startingGold = 500; // Başlangıç altını
    private int currentGold;
    private int goldEarnedThisMatch = 0; // Bu maçta kazanılan toplam altın

    [Header("Rewards")]
    public int goldPerRoundWin = 50;      // Round kazanınca
    public int goldPerMatchWin = 200;     // Maç kazanınca
    public int goldPerfectBonus = 100;    // Hasar almadan kazanınca

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGold();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ============================================
    // ALTIN KAZANMA
    // ============================================

    public void AddGold(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("⚠️ Negatif altın eklenemez!");
            return;
        }

        currentGold += amount;
        goldEarnedThisMatch += amount; // 🔥 Bu maça ekle
        SaveGold();
        Debug.Log($"💰 +{amount} Altın! Toplam: {currentGold}");
    }

    // ============================================
    // ALTIN HARCAMA
    // ============================================

    public bool SpendGold(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("⚠️ Negatif altın harcanamazsın!");
            return false;
        }

        if (currentGold >= amount)
        {
            currentGold -= amount;
            SaveGold();
            Debug.Log($"💸 -{amount} Altın harcandı! Kalan: {currentGold}");
            return true;
        }
        else
        {
            Debug.Log($"❌ Yetersiz altın! Var: {currentGold}, Gerekli: {amount}");
            return false;
        }
    }

    // ============================================
    // ALTIN SORGULAMA
    // ============================================

    public int GetGold()
    {
        return currentGold;
    }

    public bool HasEnoughGold(int amount)
    {
        return currentGold >= amount;
    }

    // ============================================
    // KAYDETME / YÜKLEME
    // ============================================

    void SaveGold()
    {
        PlayerPrefs.SetInt("PlayerGold", currentGold);
        PlayerPrefs.Save();
    }

    void LoadGold()
    {
        // İlk kez oynuyorsa başlangıç altınını ver
        currentGold = PlayerPrefs.GetInt("PlayerGold", startingGold);
        Debug.Log($"💰 Altın yüklendi: {currentGold}");
    }

    // ============================================
    // RESET
    // ============================================

    public void ResetCurrency()
    {
        currentGold = startingGold;
        SaveGold();
        Debug.Log($"🔄 Altın sıfırlandı! Yeni miktar: {currentGold}");
    }

    // ============================================
    // ÖDÜL FONKSİYONLARI
    // ============================================

    public void RewardRoundWin()
    {
        AddGold(goldPerRoundWin);
        Debug.Log($"🏆 Round kazandın! +{goldPerRoundWin} altın!");
    }

    public void RewardMatchWin()
    {
        AddGold(goldPerMatchWin);
        Debug.Log($"🎉 Maç kazandın! +{goldPerMatchWin} altın!");
    }

    public void RewardPerfectWin()
    {
        AddGold(goldPerfectBonus);
        Debug.Log($"⭐ Perfect! +{goldPerfectBonus} bonus altın!");
    }

    // ============================================
    // DEBUG
    // ============================================

    public void AddDebugGold()
    {
        AddGold(1000);
    }

    // ============================================
    // MAÇ ALTIN TAKİBİ
    // ============================================

    public int GetMatchGold()
    {
        return goldEarnedThisMatch;
    }

    public void ResetMatchGold()
    {
        goldEarnedThisMatch = 0;
        Debug.Log("🔄 Maç altını sıfırlandı!");
    }
}