using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Victory Panel")]
    public GameObject victoryPanel;
    public TextMeshProUGUI victoryText;
    public Button nextButton;
    public TextMeshProUGUI goldEarnedText;

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    [Header("🔥 Player Round Göstergeleri (3 Kare)")]
    public Image[] playerRoundIndicators; // Player'ın can barının altında 3 kare

    [Header("🔥 Enemy Round Göstergeleri (3 Kare)")]
    public Image[] enemyRoundIndicators; // Enemy'nin can barının altında 3 kare

    [Header("Round Renkleri")]
    public Color wonColor = new Color(1f, 0.5f, 0f); // Turuncu (kazanılan round)
    public Color notWonColor = Color.gray; // Gri (kazanılmamış round)

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextButtonClicked);

        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartButtonClicked);

        // 🔥 Round göstergelerini başlangıçta güncelle
        UpdateRoundIndicators();
    }

    void Update()
    {
        // 🔥 Her frame round göstergelerini güncelle
        UpdateRoundIndicators();
    }

    // ============================================
    // 🔥 3 KARE ROUND GÖSTERGE SİSTEMİ
    // ============================================

    void UpdateRoundIndicators()
    {
        if (GameManager.Instance == null)
            return;

        int playerWins = GameManager.Instance.GetPlayerRoundsWon();
        int enemyWins = GameManager.Instance.GetEnemyRoundsWon();

        // 🔥 PLAYER ROUND GÖSTERGELERİ (3 KARE)
        if (playerRoundIndicators != null)
        {
            for (int i = 0; i < playerRoundIndicators.Length; i++)
            {
                if (playerRoundIndicators[i] != null)
                {
                    // Kazanılan roundlar turuncu, diğerleri gri
                    playerRoundIndicators[i].color = (i < playerWins) ? wonColor : notWonColor;
                }
            }
        }

        // 🔥 ENEMY ROUND GÖSTERGELERİ (3 KARE)
        if (enemyRoundIndicators != null)
        {
            for (int i = 0; i < enemyRoundIndicators.Length; i++)
            {
                if (enemyRoundIndicators[i] != null)
                {
                    // Kazanılan roundlar turuncu, diğerleri gri
                    enemyRoundIndicators[i].color = (i < enemyWins) ? wonColor : notWonColor;
                }
            }
        }
    }

    // ============================================
    // VICTORY PANEL
    // ============================================

    public void ShowVictory()
    {
        if (victoryPanel == null)
        {
            Debug.LogError("❌ Victory Panel NULL!");
            return;
        }

        victoryPanel.SetActive(true);
        Time.timeScale = 0f;

        // 🔥 KAZANILAN ALTINI GÖSTER
        if (goldEarnedText != null && CurrencyManager.Instance != null)
        {
            int earnedGold = CurrencyManager.Instance.GetMatchGold();
            goldEarnedText.text = $"+{earnedGold} G";
            Debug.Log($"💰 Victory panelinde gösterilen altın: {earnedGold}");
        }

        Debug.Log("🏆 VICTORY PANEL AÇILDI!");
    }

    void OnNextButtonClicked()
    {
        Debug.Log("✅ NEXT BUTTON CLICKED");

        // 🔥 Önce paneli kapat
        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        // 🔥 Time.timeScale'i aç
        Time.timeScale = 1f;

        // 🔥 MapScene'e dön
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NextEnemy();
        }
    }

    // ============================================
    // GAME OVER PANEL
    // ============================================

    public void ShowGameOver()
    {
        if (gameOverPanel == null)
        {
            Debug.LogError("❌ Game Over Panel NULL!");
            return;
        }

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        Debug.Log("💀 GAME OVER PANEL AÇILDI!");
    }

    void OnRestartButtonClicked()
    {
        Debug.Log("🔄 RESTART BUTTON CLICKED");

        // 🔥 Önce paneli kapat
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // 🔥 Time.timeScale'i aç (önemli!)
        Time.timeScale = 1f;

        // 🔥 Restart fonksiyonunu çağır
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartFight();
        }
    }

    // ============================================
    // HELPER
    // ============================================

    public void HideAllPanels()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Time.timeScale = 1f;
    }
}