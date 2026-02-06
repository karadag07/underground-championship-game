using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RoundTransition : MonoBehaviour
{
    public static RoundTransition Instance;

    [Header("Countdown Panel (3-2-1)")]
    public GameObject countdownPanel; // Karanlık panel
    public TextMeshProUGUI countdownText; // "3", "2", "1"
    public TextMeshProUGUI roundInfoText; // "ROUND 1", "ROUND 2"

    [Header("Fight Panel (FIGHT!)")]
    public GameObject fightPanel; // Ayrı bir panel (şeffaf/minimal)
    public TextMeshProUGUI fightText; // "FIGHT!" yazısı

    [Header("Ayarlar")]
    public float countdownDuration = 1f; // Her sayı kaç saniye
    public float fightDuration = 1f; // "FIGHT!" kaç saniye gösterilir

    bool isTransitioning = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Başlangıçta her iki panel kapalı
        if (countdownPanel != null)
            countdownPanel.SetActive(false);

        if (fightPanel != null)
            fightPanel.SetActive(false);
    }

    // ============================================
    // ROUND BAŞLATMA
    // ============================================

    public void StartRoundTransition(int roundNumber)
    {
        if (isTransitioning)
            return;

        StartCoroutine(RoundTransitionCoroutine(roundNumber));
    }

    IEnumerator RoundTransitionCoroutine(int roundNumber)
    {
        isTransitioning = true;

        // Oyunu durdur
        Time.timeScale = 0f;

        // ============================================
        // PHASE 1: COUNTDOWN PANEL (3-2-1)
        // ============================================

        if (countdownPanel != null)
            countdownPanel.SetActive(true);

        // Round bilgisi
        if (roundInfoText != null)
        {
            roundInfoText.text = $"ROUND {roundNumber}";
        }

        // 3-2-1 sayımı
        yield return ShowCountdown("3");
        yield return ShowCountdown("2");
        yield return ShowCountdown("1");

        // Countdown panel'i kapat
        if (countdownPanel != null)
            countdownPanel.SetActive(false);

        // ============================================
        // PHASE 2: FIGHT PANEL (FIGHT!)
        // ============================================

        if (fightPanel != null)
            fightPanel.SetActive(true);

        if (fightText != null)
            fightText.text = "FIGHT!";

        // FIGHT! yazısını göster
        yield return new WaitForSecondsRealtime(fightDuration);

        // Fight panel'i kapat
        if (fightPanel != null)
            fightPanel.SetActive(false);

        // ============================================
        // OYUN BAŞLASIN
        // ============================================

        Time.timeScale = 1f;
        isTransitioning = false;

        Debug.Log($"✅ ROUND {roundNumber} BAŞLADI!");
    }

    IEnumerator ShowCountdown(string text)
    {
        if (countdownText != null)
            countdownText.text = text;

        yield return new WaitForSecondsRealtime(countdownDuration);
    }

    // ============================================
    // HELPER
    // ============================================

    public bool IsTransitioning()
    {
        return isTransitioning;
    }
}