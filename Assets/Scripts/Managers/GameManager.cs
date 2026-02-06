using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Oyun Durumu")]
    public GameState currentState;

    [Header("Rakip Sistemi")]
    public EnemyData[] allEnemies;
    public int currentEnemyID = 1;
    public int unlockedEnemyID = 1;

    [Header("Round Sistemi")]
    public int currentRound = 1;
    public int roundsToWin = 3; // 🔥 3 ROUND KAZANAN MAÇ ALIR

    // 🔥 ROUND KAZANMA SAYACI
    private int playerRoundsWon = 0;
    private int enemyRoundsWon = 0;

    [Header("Referanslar")]
    public GameObject player;
    public GameObject enemy;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadProgress();
    }

    void Start()
    {
        currentState = GameState.Playing;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "FightScene")
        {
            Debug.Log("🎬 FightScene yüklendi! Countdown başlatılıyor...");

            // 🔥 YENİ MAÇ BAŞLADI - SAYAÇLARI SIFIRLA
            currentRound = 1;
            playerRoundsWon = 0;
            enemyRoundsWon = 0;
            Debug.Log($"🔄 Round sayaçları sıfırlandı! Player: {playerRoundsWon}, Enemy: {enemyRoundsWon}");

            // 🔥 ALTIN TAKİBİNİ SIFIRLA
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.ResetMatchGold();
            }

            StartCoroutine(StartCountdownAfterFrame());
        }
    }

    System.Collections.IEnumerator StartCountdownAfterFrame()
    {
        yield return null;

        if (RoundTransition.Instance != null)
        {
            RoundTransition.Instance.StartRoundTransition(currentRound);
        }
        else
        {
            Debug.LogError("❌ RoundTransition.Instance NULL!");
        }
    }

    // ============================================
    // 🔥 ROUND YÖNETİMİ (3 ROUND KAZANAN ALIR)
    // ============================================

    public void PlayerWonRound()
    {
        playerRoundsWon++; // 🔥 SAYACI ARTIR
        Debug.Log($"🎉 PLAYER ROUND {currentRound} KAZANDI! Toplam kazanılan round: {playerRoundsWon}/{roundsToWin}");

        // 🔥 ROUND KAZANINCA ALTIN KAZAN
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.RewardRoundWin();
        }

        // 🔥 3 ROUND KAZANILDIYSA MAÇ KAZANILDI
        if (playerRoundsWon >= roundsToWin)
        {
            Debug.Log($"🏆 PLAYER {roundsToWin} ROUND KAZANDI! MAÇ KAZANILDI! (Skor: {playerRoundsWon}-{enemyRoundsWon})");
            PlayerWonMatch();
        }
        else
        {
            // 🔥 Henüz 3 round kazanılmadı, devam et
            Debug.Log($"⏭️ Sonraki round'a geçiliyor... (Player: {playerRoundsWon}, Enemy: {enemyRoundsWon})");
            NextRound();
        }
    }

    public void EnemyWonRound()
    {
        enemyRoundsWon++; // 🔥 SAYACI ARTIR
        Debug.Log($"💀 ENEMY ROUND {currentRound} KAZANDI! Toplam kazanılan round: {enemyRoundsWon}/{roundsToWin}");

        // 🔥 Enemy 3 round kazandıysa maç kaybedildi
        if (enemyRoundsWon >= roundsToWin)
        {
            Debug.Log($"💀 ENEMY {roundsToWin} ROUND KAZANDI! MAÇ KAYBEDİLDİ! (Skor: {playerRoundsWon}-{enemyRoundsWon})");
            PlayerLostMatch();
        }
        else
        {
            Debug.Log($"⏭️ Sonraki round'a geçiliyor... (Player: {playerRoundsWon}, Enemy: {enemyRoundsWon})");
            NextRound();
        }
    }

    void NextRound()
    {
        currentRound++;
        Debug.Log($"📍 SONRAKI ROUND: {currentRound} (Player Kazandı: {playerRoundsWon}, Enemy Kazandı: {enemyRoundsWon})");

        StartCoroutine(NextRoundCoroutine());
    }

    System.Collections.IEnumerator NextRoundCoroutine()
    {
        // Önce resetle
        yield return StartCoroutine(ResetFightCoroutine());

        // Sonra countdown başlat
        currentState = GameState.Playing;

        if (RoundTransition.Instance != null)
        {
            RoundTransition.Instance.StartRoundTransition(currentRound);
        }
    }

    // ============================================
    // MAÇ SONUÇLARI
    // ============================================

    void PlayerWonMatch()
    {
        currentState = GameState.Victory;
        Debug.Log($"🏆 PLAYER MAÇ KAZANDI! Final Skor: {playerRoundsWon}-{enemyRoundsWon}");
        Debug.Log($"🔥 Rakip {currentEnemyID} yenildi!");

        // 🔥 MAÇ KAZANINCA BÜYÜK ÖDÜL
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.RewardMatchWin();
        }

        if (currentEnemyID < 10)
        {
            unlockedEnemyID = currentEnemyID + 1;
            SaveProgress();
            Debug.Log($"🔓 Rakip {unlockedEnemyID} unlock oldu!");
        }
        else
        {
            Debug.Log("🎊 OYUN TAMAMLANDI! TÜM RAKİPLER YENİLDİ!");
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowVictory();
        }
        else
        {
            Debug.LogError("❌ UIManager.Instance NULL! Victory paneli açılamıyor!");
        }
    }

    void PlayerLostMatch()
    {
        currentState = GameState.GameOver;
        Debug.Log($"💀 PLAYER MAÇ KAYBETTİ! Final Skor: {playerRoundsWon}-{enemyRoundsWon}");

        // Game Over paneli aç
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver();
        }
    }

    // ============================================
    // RESTART & NEXT
    // ============================================

    public void RestartFight()
    {
        currentRound = 1;
        playerRoundsWon = 0;
        enemyRoundsWon = 0;
        Time.timeScale = 1f;
        Debug.Log("🔄 Restart: Round sayaçları sıfırlandı!");
        StartCoroutine(RestartFightCoroutine());
    }

    System.Collections.IEnumerator RestartFightCoroutine()
    {
        // Önce resetle
        yield return StartCoroutine(ResetFightCoroutine());

        // Sonra countdown
        currentState = GameState.Playing;

        if (RoundTransition.Instance != null)
        {
            RoundTransition.Instance.StartRoundTransition(currentRound);
        }
    }

    public void NextEnemy()
    {
        Debug.Log("✅ NextEnemy() çağrıldı!");

        // Round'ları sıfırla
        currentRound = 1;
        playerRoundsWon = 0;
        enemyRoundsWon = 0;

        // MapScene'e geç
        Debug.Log("🗺️ Map Scene'e geçiliyor...");
        SceneManager.LoadScene("Map Scene");
    }

    // ============================================
    // RESET FIGHT
    // ============================================

    System.Collections.IEnumerator ResetFightCoroutine()
    {
        Debug.Log("🔄 ResetFight Coroutine başladı...");

        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.2f);

        // ============================================
        // PLAYER RESET
        // ============================================
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            playerObj = GameObject.Find("Player");
        }

        if (playerObj != null)
        {
            playerObj.SetActive(true);

            // Health
            PlayerHealth ph = playerObj.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.ResetHealth();
                Debug.Log("✅ Player Health resetlendi!");
            }

            // Stamina
            PlayerStamina ps = playerObj.GetComponent<PlayerStamina>();
            if (ps != null)
            {
                ps.ResetStamina();
                Debug.Log("✅ Player Stamina resetlendi!");
            }

            // Skill
            PlayerSkill psk = playerObj.GetComponent<PlayerSkill>();
            if (psk != null)
            {
                psk.ResetSkill();
                Debug.Log("✅ Player Skill resetlendi!");
            }
        }

        // ============================================
        // ENEMY RESET
        // ============================================
        GameObject enemyObj = GameObject.FindGameObjectWithTag("Enemy");
        if (enemyObj == null)
        {
            enemyObj = GameObject.Find("Enemy");
        }

        if (enemyObj != null)
        {
            enemyObj.SetActive(true);

            // Health
            EnemyHealth eh = enemyObj.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                eh.ResetHealth();
                Debug.Log("✅ Enemy Health resetlendi!");
            }

            // Stamina
            EnemyStamina es = enemyObj.GetComponent<EnemyStamina>();
            if (es != null)
            {
                es.ResetStamina();
                Debug.Log("✅ Enemy Stamina resetlendi!");
            }

            // Skill
            EnemySkill esk = enemyObj.GetComponent<EnemySkill>();
            if (esk != null)
            {
                esk.ResetSkill();
                Debug.Log("✅ Enemy Skill resetlendi!");
            }
        }

        Debug.Log("✅ ResetFight tamamlandı!");
    }

    // ============================================
    // SAVE / LOAD
    // ============================================

    void SaveProgress()
    {
        PlayerPrefs.SetInt("UnlockedEnemyID", unlockedEnemyID);
        PlayerPrefs.Save();
        Debug.Log($"💾 İLERLEME KAYDEDİLDİ: Rakip {unlockedEnemyID}'ye kadar açık");
    }

    void LoadProgress()
    {
        unlockedEnemyID = PlayerPrefs.GetInt("UnlockedEnemyID", 1);
        Debug.Log($"📂 İLERLEME YÜKLENDİ: Rakip {unlockedEnemyID}'ye kadar açık");
    }

    public void ResetProgress()
    {
        unlockedEnemyID = 1;
        currentRound = 1;
        playerRoundsWon = 0;
        enemyRoundsWon = 0;
        PlayerPrefs.DeleteAll();
        Debug.Log("🗑️ İLERLEME SİLİNDİ!");
    }

    // ============================================
    // HELPER METHODLAR
    // ============================================

    public EnemyData GetCurrentEnemy()
    {
        if (currentEnemyID > 0 && currentEnemyID <= allEnemies.Length)
        {
            return allEnemies[currentEnemyID - 1];
        }
        return null;
    }

    public bool IsEnemyUnlocked(int enemyID)
    {
        return enemyID <= unlockedEnemyID;
    }

    // 🔥 ROUND KAZANMA SAYILARINI DÖNDÜR
    public int GetPlayerRoundsWon()
    {
        return playerRoundsWon;
    }

    public int GetEnemyRoundsWon()
    {
        return enemyRoundsWon;
    }
}

public enum GameState
{
    MainMenu,
    Map,
    Playing,
    Victory,
    GameOver
}